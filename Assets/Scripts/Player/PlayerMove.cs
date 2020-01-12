using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    public enum CharacterState
    {
        Idle,
        Running,
        Roll,
        Attack1,
        Attack2,
        SpecialAttack
    }
    [SerializeField]
    private PlayerMove.CharacterState _characterState;

    #region private parameter 
    private Player pController;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private Transform cameraTransform;
    private bool isControllable = true;
    private RaycastHit groundHit;
    private float verticalSpeed;
    private float moveSpeed;
    private float walkSpeed;
    private bool isMoving;
    private bool playOnce;
    private Vector3 m_CamForward;
    #endregion

    [Header("LayerMask")]
    public LayerMask groundLayer;
    public LayerMask obsLayer;

    private bool IsGrounded()
    {
        return this.groundHit.distance <= this.groundHeight;
    }
    public bool IsAttacking()
    {
        return this.GetCharState() == PlayerMove.CharacterState.Attack1 
            || this.GetCharState() == PlayerMove.CharacterState.Attack2
            || this.GetCharState() == PlayerMove.CharacterState.SpecialAttack;
    }
    public bool CanRoll()
    {
        return this.GetCharState() == PlayerMove.CharacterState.Idle || this.GetCharState() == PlayerMove.CharacterState.Running || this.IsAttacking();
    }
    public bool CanAttack()
    {
        return !this.IsAttacking();
    }
    private void Raycast()
    {
        if (Physics.Raycast(pController.GetPlayerTransform().position + Vector3.up * 0.1f, -pController.GetPlayerTransform().up, out this.groundHit, 10f, groundLayer))
        {
            //Debug.Log(groundHit.distance);
        }
    }
    [Header("Movement")]
    [SerializeField]
    private float groundHeight = 0.15f;
    [SerializeField]
    private float gravity = 20f;
    [SerializeField]
    private float stopMovementVelocity = 1f;
    [SerializeField]
    private float normalRunSpeed = 10f;
    [Header("Lerp")]
    [SerializeField]
    private bool lerpable = true;
    [SerializeField]
    private float rotateSpeed = 360;
    [SerializeField]
    private float smoothRotation = 100f;
    [SerializeField]
    private float speedSmoothing = 100f;



    public void SetCharState(PlayerMove.CharacterState c)
    {
        if (c != this._characterState)
        {
            //Debug.Log(c.ToString());
            this._characterState = c;
            this.playOnce = false;
        }
    }
    public PlayerMove.CharacterState GetCharState()
    {
        return this._characterState;
    }
    public void ResetCharState(PlayerMove.CharacterState c)
    {
        this._characterState = c;
        this.playOnce = false;
    }
    public float GetSpeed()
    {
        return this.moveSpeed;
    }

    public void Init(Player player)
    {
        this.pController = player;
        this.moveDirection = base.transform.TransformDirection(Vector3.forward);
        this.controller = base.GetComponent<CharacterController>();
        this.cameraTransform = Camera.main.transform;

        this.SetAnimSpeeds();
    }

    public void SetAnimSpeeds()
    {
    }
    public void SetSpeed(float speed)
    {
        if (speed == 0f)
        {
            this.walkSpeed = 0f;
            this.moveSpeed = 0f;
            this.ResetCharState(PlayerMove.CharacterState.Idle);
        }
        else
        {
            if (this._characterState != PlayerMove.CharacterState.Roll && this._characterState != PlayerMove.CharacterState.Running&&!IsAttacking())
            {
                this.walkSpeed = normalRunSpeed;
                this._characterState = PlayerMove.CharacterState.Running;
            }
        }
    }
    private bool IsColliding(Vector3 dir, bool isKnockBack = false)
    {
        if (this._characterState == PlayerMove.CharacterState.Roll || isKnockBack)
        {
            Vector3 origin = Player.player.transform.position + new Vector3(0f, 0.2f, 0f);
            RaycastHit raycastHit;
            if (Physics.Raycast(origin, dir, out raycastHit, 1f))
            {
                if (0!=(obsLayer&1 << raycastHit.transform.gameObject.layer))
                {
                    //Debug.Log(raycastHit.transform.gameObject.layer);
                    return true;
                }
            }
        }
        return false;
    }


    #region roll
    private float m_rollTimer;
    public float rollDuration = 0.5f;
    private void FixedUpdate()
    {
        this.Raycast();//check ground
                       //roll
        if (this._characterState == PlayerMove.CharacterState.Roll)
        {
            if (this.IsColliding(pController.GetPlayerTransform().forward, false))
            {
                this.walkSpeed = 0f;
            }
            else
            {
                this.walkSpeed = normalRunSpeed;
                this.walkSpeed *= 1.5f;
            }
            if (m_rollTimer < rollDuration)
            {
                m_rollTimer += Time.deltaTime;
            }
            else
            {
                Player.player.PlayIdle();
                m_rollTimer = 0;
            }
        }
    }
    #endregion

    #region move
    private float h, v,hRaw,vRaw;
    private void ApplyGravity()
    {
        if (this.isControllable)
        {
            if (this.IsGrounded())
            {
                this.verticalSpeed = 0f;
            }
            else
            {
                this.verticalSpeed -= this.gravity * Time.deltaTime;
            }
        }
    }
    private void UpdateWASD()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        vRaw = Input.GetAxisRaw("Vertical");
        hRaw = Input.GetAxisRaw("Horizontal");
    }
    private void UpdateSmoothedMovementDirection()
    {
        Vector3 a = this.cameraTransform.TransformDirection(Vector3.forward);
        Vector3 vector = new Vector3(a.x, 0f, a.z);
        a = vector.normalized;
        Vector3 a2 = new Vector3(a.z, 0f, -a.x);
        Vector3 vector2 = h * a2 + v * a;
        if (vector2 != Vector3.zero)
        {
            if(lerpable)
                this.moveDirection = Vector3.Slerp(this.moveDirection, vector2, this.rotateSpeed * Time.deltaTime);
            else
                this.moveDirection = vector2;
            this.moveDirection = this.moveDirection.normalized;
            this.playOnce = false;
        }

        float t = this.speedSmoothing * Time.deltaTime;
        if (this._characterState == PlayerMove.CharacterState.Roll && !this.IsColliding(pController.GetPlayerTransform().forward, false))
        {
            if (lerpable)
                this.moveSpeed = Mathf.Lerp(this.moveSpeed, this.walkSpeed, t);
            else
                this.moveSpeed = walkSpeed;
        }
        //this.playOnce = false;
        if (lerpable)
            this.moveSpeed = Mathf.Lerp(this.moveSpeed, this.walkSpeed, t);
        else
            this.moveSpeed = walkSpeed;
    }
    public void UpdateMoveDir()
    {
        this.moveDirection = base.transform.TransformDirection(Vector3.forward);
    }
    private void Update()
    {
        this.UpdateWASD();
        this.UpdateSmoothedMovementDirection();
        this.ApplyGravity();
        Vector3 vector = this.moveDirection * this.moveSpeed + new Vector3(0f, this.verticalSpeed, 0f);
        this.controller.Move(vector * Time.deltaTime);
         if (pController.GetPlayerAnim() && !this.playOnce)
        {
            this.playOnce = true;
            if (this._characterState == PlayerMove.CharacterState.Attack1)
            {
                pController.GetPlayerAnim().Play("Attack1");
            }
            else if (this._characterState == PlayerMove.CharacterState.Attack2)
            {
                pController.GetPlayerAnim().Play("Attack2");
            }
            else if (this._characterState == PlayerMove.CharacterState.SpecialAttack)
            {
                pController.GetPlayerAnim().Play("SpecialAttack");
            }
            else if (this._characterState == PlayerMove.CharacterState.Roll)
            {
                ResetDash();
                pController.GetPlayerAnim().Play("Roll");
            }
            else
            {
                if (this.controller.velocity.sqrMagnitude < this.stopMovementVelocity)
                {
                    if (!pController.GetPlayerAnim().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    {
                        pController.GetPlayerAnim().Play("Idle");
                    }
                }
                if (this._characterState == PlayerMove.CharacterState.Running )
                {
                    if (this.controller.velocity.sqrMagnitude >0.2f&& !this.IsColliding(pController.GetPlayerTransform().forward, false))
                    {
                        pController.GetPlayerAnim().Play("Run");
                    }
                }
            }
        }
        if (!IsAttacking())
        {
            if ((v != 0 || h != 0))
            {
                this.Rotating(h, v);
            }
        }
    }
    private void Rotating(float horizontal, float vertical)
    {
        Vector3 forward = new Vector3(horizontal, 0f, vertical);
        this.m_CamForward = Vector3.Scale(this.cameraTransform.forward, new Vector3(1f, 0f, 1f)).normalized;
        forward = this.m_CamForward * vertical + this.cameraTransform.right * horizontal;
        Quaternion to = Quaternion.LookRotation(forward, Vector3.up);
        Quaternion rotation = to;
        if (lerpable)
             rotation = Quaternion.Lerp(base.transform.rotation, to, this.smoothRotation * Time.deltaTime);
        base.transform.rotation = rotation;
    }
    #endregion

    #region attackDash
    private bool canAttackDash = true;
    private Coroutine dashCor;
    private void ResetDash()
    {
        canAttackDash = true;
        if (Player.player.GetPlayerAttack().GetCurrentCoro() != null)
            StopCoroutine(Player.player.GetPlayerAttack().GetCurrentCoro());
        if (dashCor != null)
            StopCoroutine(dashCor);
    }
    public void AttackDash(Vector3 dist, float dashTime)
    {
        this.UpdateMoveDir();
        if (this.canAttackDash)
        {
            dashCor= base.StartCoroutine(this.DashFront(dist, dashTime));
        }
    }
    private IEnumerator DashFront(Vector3 slamDistance, float dashTime)
    {
        float slamTime = Time.time;
        this.canAttackDash = false;
        while (slamTime + dashTime > Time.time)
        {
            this.controller.Move(slamDistance * (slamTime + dashTime - Time.time) / dashTime * Time.deltaTime);
            yield return null;
        }
        this.canAttackDash = true;
        yield break;
    }
    #endregion
}
