using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    public Rigidbody2D rb;

    public GameObject bulletRef;

    [SerializeField] private bool isEnabled;
    [SerializeField] private LayerMask dashLayerMask;

    private Vector2 moveDir;
    private Vector3 rollDir;
    private Vector3 lastMoveDir;

    private bool attackOnCooldown;
    private bool isDashButtonDown;
    private State state;

    private float rollSpeed = 3.5f;

    private enum State
    {
        Normal,
        Rolling,
    }

    //========== PUBLIC METHODS ==========//
    public void SetActive(bool flag)
    {
        isEnabled = flag;
    }

    //========== PRIVATE METHODS ==========//
    private void Awake()
    {
        instance = this;
        state = State.Normal;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isEnabled = true;
        attackOnCooldown = false;
    }

    private void Update()
    {
        if (isEnabled && GetComponent<Unit>().health > 0)
        {
            ProcessInputs();
        }
    }

    private void FixedUpdate()
    {
        if (GetComponent<Unit>().health > 0)
        {
            Move();
        }
    }

    private void ProcessInputs()
    {
        switch (state)
        {
            case State.Normal:
                float x = Input.GetAxisRaw("Horizontal");
                float y = Input.GetAxisRaw("Vertical");
                moveDir = new Vector2(x, y).normalized;

                if (x != 0 || y != 0)
                {
                    Vector3 moveVector = Vector3.zero;
                    moveVector.x = Input.GetAxis("Horizontal");
                    moveVector.y = Input.GetAxis("Vertical");
                    lastMoveDir = moveDir;
                    GetComponent<Player>().SetSpriteAngle(moveVector);
                }

                if (Input.GetMouseButtonDown(0) && !attackOnCooldown)
                {
                    attackOnCooldown = true;

                    GameObject projectile = Instantiate(bulletRef, transform.position, Quaternion.identity);
                    projectile.GetComponent<Projectile>().source = this.gameObject;
                    projectile.GetComponent<Projectile>().SetDirection(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    projectile.GetComponent<Projectile>().InvokeDestroySelf(3f);

                    Invoke("ResetAttackCooldown", 1f);
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isDashButtonDown = true;
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    rollDir = lastMoveDir;
                    rollSpeed = 5f;
                    state = State.Rolling;
                }
                break;
            case State.Rolling:
                float rollSpeedDropMultiplier = 0.5f;
                rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;

                float rollSpeedMinimum = 3.5f;
                if (rollSpeed < rollSpeedMinimum)
                {
                    state = State.Normal;
                }
                break;
        }
    }

    private void Move()
    {
        switch (state)
        {
            case State.Normal:
                rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);

                if (isDashButtonDown)
                {
                    float dashAmount = 3f;
                    Vector3 dashPosition = transform.position + (Vector3)lastMoveDir * dashAmount;
                    RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, lastMoveDir, dashAmount, dashLayerMask);
                    if (raycastHit2D.collider != null)
                    {
                        dashPosition = raycastHit2D.point;
                    }

                    // Spawn visual effect
                    // DashEffect.CreateDashEffect(transform.position, moveDir, Vector3.Distance(transform.position, dashPosition));

                    rb.MovePosition(dashPosition);
                    isDashButtonDown = false;
                }
                break;
            case State.Rolling:
                rb.velocity = rollDir * rollSpeed;
                break;
        }

    }

    private void ResetAttackCooldown()
    {
        attackOnCooldown = false;
    }
}
