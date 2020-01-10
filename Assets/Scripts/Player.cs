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
    private PlayerInput pInput;
    private Transform playerTrans;

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

    private void Awake()
    {
        player = this;
        playerTrans = this.transform;
        pAttack = GetComponent<PlayerAttack>();
        pMove = GetComponent<PlayerMove>();
        pAnim= GetComponent<Animator>();
        pAttack.Init(this);
        pMove.Init(this);
        pInput.Init(this);
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
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
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
}
