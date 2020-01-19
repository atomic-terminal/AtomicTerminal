using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player player;
    private Animator pAnim;
    private PlayerAttack pAttack;
    private PlayerMove pMove;
    [SerializeField]
    private PlayerAttributes pAttribute;
    private PlayerStats pStats;
    [SerializeField]
    private PlayerInput pInput;
    private Transform playerTrans;
    [SerializeField]
    private PlayerBars playerBar;

    private float currentHP;
    private bool deathConfirmed;
    private Room room;
    #region get
    public bool IsDead()
    {
        return this.deathConfirmed;
    }

    public PlayerMove GetAsuraMove()
    {
        return this.pMove;
    }
    public PlayerAttack GetPlayerAttack()
    {
        return this.pAttack;
    }
    public PlayerInput GetPlayerInput()
    {
        return this.pInput;
    }
    public Animator GetPlayerAnim()
    {
        return this.pAnim;
    }
    public Transform GetPlayerTransform()
    {
        return this.playerTrans;
    }
    public PlayerAttributes GetAttribute()
    {
        return this.pAttribute;
    }
    public PlayerStats GetPlayerStats()
    {
        return this.pStats;
    }
    public PlayerBars GetPlayerBar()
    {
        return this.playerBar;
    }
    #endregion
    private void Awake()
    {
        player = this;
        playerTrans = this.transform;
        pAttack = GetComponent<PlayerAttack>();
        pMove = GetComponent<PlayerMove>();
        pAnim= GetComponent<Animator>();
        pStats= GetComponent<PlayerStats>();
        pAttribute.Init(this);
        pAttack.Init(this);
        pMove.Init(this);
        pInput.Init(this);

        this.currentHP = GetAttribute().Health;
        GetPlayerBar().InitHearts(GetPlayerStats().MaxHP);
        UpdateHearts(true);
    }

    public void SetPlayerRoom(Room p)
    {
        if (p == null && this.room != null)
        {
            this.room.RoomEnd();
        }
        this.room = p;
    }
    public void RoomTrigger(bool isStart = false, Room room = null)
    {
        this.SetPlayerRoom(null);
    }
    public void PlayIdle()
    {
        this.pMove.SetCharState(PlayerMove.CharacterState.Idle);
    }
    public void Stop(bool animReset = true)
    {
        if (this.pMove.GetSpeed() > 0f)
        {
            this.pMove.SetSpeed(0f);
            if (animReset)
            {
                this.PlayIdle();
            }
        }
    }

    public void LookAt(Vector3 _dir)
    {
        this.playerTrans.LookAt(new Vector3(_dir.x, this.playerTrans.position.y, _dir.z));
    }

    public void Attack(bool special)
    {
        this.Stop(false);
        this.GetPlayerAttack().Attack(special);
    }

    private void Update()
    {
        this.pInput.InputProcess();
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (this.pMove.GetCharState() == PlayerMove.CharacterState.Roll)
        {
            this.pMove.UpdateMoveDir();
            return;
        }
        if (vertical == 0f && horizontal == 0f)
        {
            this.Stop(true);
        }
        else
        {
            this.MoveTo();
        }
    }
    public void MoveTo()
    {
        this.pMove.SetSpeed(1f);
    }
    public void UpdateHearts(bool updateHearts = true)
    {
        if (updateHearts)
        {
            this.GetPlayerBar().UpdateHearts(this.currentHP);
        }
    }
    public void HPLoss(float dmg)
    {
        float dmg2 = dmg;
        if (this.GetAttribute().Invulerable)
        {
            return;
        }
        this.currentHP -= dmg2;
        this.GetPlayerBar().HpLost(this.currentHP, dmg2);
    }
    public float GetHpDiff()
    {
        return this.GetAttribute().Health - this.currentHP;
    }
    public void BloodHeal(float heal)
    {
        if (heal >= GetPlayerStats().MaxHP)
        {
            heal = Mathf.Round(this.GetHpDiff());
        }
        float hpGained = Mathf.Clamp(heal, 0f, this.GetAttribute().Health - this.currentHP);
        this.currentHP += heal;
        if (this.currentHP > this.GetAttribute().Health)
        {
            this.currentHP = this.GetAttribute().Health;
        }
        this.GetPlayerBar().HpGained(this.currentHP, hpGained);
        this.UpdateHearts(true);
    }
}
