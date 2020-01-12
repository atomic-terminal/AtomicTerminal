using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ball : MonoBehaviour {

    [Header("Settings")]
    public float speed = 10f;//速度
    public float gravity = 20f;

    private CharacterController controller;
    private Vector3 velocity, moveDirection;
    private float squareVelocity;
    private float verticalSpeed;

    void Awake() {
        controller = GetComponent<CharacterController>();
        player = Player.player.GetPlayerTransform();
    }

    private bool shootPrepare;
    private bool shootStart;
    private bool shooted;
    private Transform player;

    private void Start()
    {
        setShootPrepare();
    }

    void Update() {
        if (controller.enabled)
        {
            velocity = controller.velocity;
            squareVelocity = Mathf.Abs(velocity.sqrMagnitude);
        }
        else
        {
            squareVelocity = 0;
        }

        if (shootStart && squareVelocity < 1)
        {
            shootStart = false;
            shooted = true;
            moveDirection = (transform.position - player.position).normalized;
        }
        // 弹射移动
        this.verticalSpeed -= this.gravity * Time.deltaTime;

        if (controller.enabled)
        {
            moveDirection = moveDirection.normalized * speed;
            controller.Move((moveDirection + new Vector3(0f, this.verticalSpeed, 0f)) * Time.fixedDeltaTime);
        }
        // 掉落
        if (transform.position.y < -4)
        {
        }
    }

    public void setShootPrepare()
    {
        shootPrepare = true;
        shootStart = false;
    }

    public void Shoot()
    {
        if (shootPrepare)
        {
            shootPrepare = false;
            shootStart = true;
        }
    }

    [Header("LayerMask")]
    public LayerMask ignoreLayers;
    public LayerMask playerLayer;
    public LayerMask hitBoxLayer;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == Mathf.Log(ignoreLayers.value, 2)) return;
        if (shooted)
        {
            Vector3 normal = (hit.normal).normalized;
            normal.y = 0;
            moveDirection = Vector3.Reflect(moveDirection, normal);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (0 != (playerLayer & 1 << other.gameObject.layer)|| 0 != (hitBoxLayer & 1 << other.gameObject.layer))
        {
            if (!shooted)
            {
                Shoot();
            }
        }
        if (0 != (hitBoxLayer & 1 << other.gameObject.layer))
        {
            if (shooted)
            {
                moveDirection = Vector3.Reflect(moveDirection, Vector3.forward);
            }
        }
    }
    void OnMouseEnter()
    {
        CursorManager._instance.SetAttack();
    }
    void OnMouseExit()
    {
        CursorManager._instance.SetNormal();
    }
}