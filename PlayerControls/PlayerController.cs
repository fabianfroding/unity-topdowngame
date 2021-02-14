using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float MOVE_SPEED = 8f;
    private const float JUMP_SPEED = 12.5f;
    private const float JUMP_FALL_MULTIPLIER = 1f;
    private const float JUMP_LOW_MULTIPLIER = 0.5f;
    private const float JUMP_TIME_MAX = 0.35f;

    public static bool isEnabled = true;

    [SerializeField] private GameObject slashHitBoxLeft;
    [SerializeField] private GameObject slashHitBoxRight;
    [SerializeField] private GameObject slashUpHitBox;
    [SerializeField] private GameObject swingSoundRef;
    [SerializeField] private GameObject dashEffect;
    [SerializeField] private Transform[] groundCheck;
    [SerializeField] private Transform[] sideCollisionCheck;
    [SerializeField] private Transform[] upCollisionCheck;

    private static Rigidbody2D rb;
    private static Vector2 moveDir;
    private static Player player;
    private static float defGravity;
    private static bool isJumping = false;
    private static bool isGrounded;
    private static bool attackOnCD = false; // This can also be flag for while attacking.
    private static State state;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector3 dashDir;
    private bool isColliding;
    private bool facingLeft = true;
    private float dashSpeed;
    private float jumpTimeCounter;

    public enum State
    {
        Normal,
        Dashing,
    }

    //==================== PUBLIC ====================//
    public bool IsGrounded()
    {
        return isGrounded;
    }

    public static State GetState()
    {
        return state;
    }

    public static void SetState(State newState)
    {
        if (state == State.Dashing && newState != State.Dashing) rb.gravityScale = defGravity;
        state = newState;
    }

    public static bool isIdle()
    {
        return moveDir.x == 0 &&
            moveDir.y == 0 &&
            state == State.Normal &&
            !isJumping &&
            isGrounded &&
            !attackOnCD;
    }

    //==================== PRIVATE ====================//
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        defGravity = rb.gravityScale;
        state = State.Normal;
    }

    // Physics-related should always go in Fixed.
    private void FixedUpdate()
    {
        if (isEnabled && GetComponent<Player>().health > 0)
        {
            Move(); // TODO: Do not allow movement if player is being knockbacked.
            if (!player.invulnerable)
            {
                GameObject collidingEnemy = CheckEnemyCollision();
                if (collidingEnemy != null) player.TakeDamage(collidingEnemy, 1);
            }
        }
    }

    // Input-related always goes in Update.
    private void Update()
    {
        isGrounded = CheckGroundCollision();
        isColliding = CheckSideCollision();
        if (isEnabled && GetComponent<Player>().health > 0) ProcessInputs();
    }

    private bool CheckGroundCollision()
    {
        for (int i = 0; i < groundCheck.Length; i++)
        {
            if (Physics2D.Linecast(transform.position, groundCheck[i].position, 1 << LayerMask.NameToLayer(EditorConstants.LAYER_GROUND))) return true;
        }
        return false;
    }

    private bool CheckSideCollision()
    {
        for (int i = 0; i < sideCollisionCheck.Length; i++)
        {
            if (Physics2D.Linecast(transform.position, sideCollisionCheck[i].position, 1 << LayerMask.NameToLayer(EditorConstants.LAYER_GROUND))) return true;
        }
        return false;
    }

    private GameObject CheckEnemyCollision()
    {
        // Reuse ground and side checks but on different layer to identify enemies.
        RaycastHit2D hit;
        for (int i = 0; i < sideCollisionCheck.Length; i++)
        {
            hit = Physics2D.Linecast(transform.position, sideCollisionCheck[i].position, 1 << LayerMask.NameToLayer(EditorConstants.TAG_ENEMY));
            if (hit && !hit.collider.isTrigger) return hit.collider.gameObject;
        }
        for (int i = 0; i < groundCheck.Length; i++)
        {
            hit = Physics2D.Linecast(transform.position, groundCheck[i].position, 1 << LayerMask.NameToLayer(EditorConstants.TAG_ENEMY));
            if (hit && !hit.collider.isTrigger) return hit.collider.gameObject;
        }
        for (int i = 0; i < upCollisionCheck.Length; i++)
        {
            hit = Physics2D.Linecast(transform.position, upCollisionCheck[i].position, 1 << LayerMask.NameToLayer(EditorConstants.TAG_ENEMY));
            if (hit && !hit.collider.isTrigger) return hit.collider.gameObject;
        }
        return null;
    }

    private void ProcessInputs()
    {
        switch (state)
        {
            case State.Normal:
                //----- Move -----//
                float x = Input.GetAxisRaw(EditorConstants.INPUT_AXIS_HORIZONTAL);
                moveDir = new Vector2(x, 0).normalized;
                SetFacing(x);
                if (isGrounded && x != 0)
                {
                    animator.Play("Player_Walk");
                }
                else if (!attackOnCD) animator.Play("Player_Idle");

                //----- Jump -----//
                if (isGrounded && Input.GetKeyDown(KeyCodeConstants.KEYCODE_JUMP))
                {
                    isJumping = true;
                    jumpTimeCounter = JUMP_TIME_MAX;
                    rb.velocity = new Vector2(rb.velocity.x * 0.8f, 1) * JUMP_SPEED; // TODO: Move to Move()
                }

                if (Input.GetKey(KeyCodeConstants.KEYCODE_JUMP) && isJumping)
                {
                    if (jumpTimeCounter > 0)
                    {
                        rb.velocity = new Vector2(rb.velocity.x * 0.8f, 1) * JUMP_SPEED; // TODO: Move to Move()
                        jumpTimeCounter -= Time.deltaTime;
                    }
                    else isJumping = false;
                }

                if (Input.GetKeyUp(KeyCodeConstants.KEYCODE_JUMP)) isJumping = false;

                //----- Attack -----//
                if (!attackOnCD && Input.GetKeyDown(KeyCodeConstants.KEY_CODE_ATTACK))
                {
                    attackOnCD = true;
                    GameObject swingSound = Instantiate(swingSoundRef, transform.position, Quaternion.identity);
                    Destroy(swingSound, swingSound.GetComponent<AudioSource>().clip.length);
                    if (Input.GetAxisRaw(EditorConstants.INPUT_AXIS_VERTICAL) > 0)
                    {
                        animator.Play("Player_AttackUp");
                        slashUpHitBox.SetActive(true);
                    }
                    else
                    {
                        animator.Play("Player_Attack");
                        if (facingLeft) slashHitBoxRight.SetActive(true);
                        else slashHitBoxLeft.SetActive(true);
                    }
                    Invoke("ResetAttack", 0.5f);
                }

                //----- Dash -----//
                if (Input.GetKeyDown(KeyCodeConstants.KEY_CODE_DASH))
                {
                    isJumping = false;
                    dashDir = GetInputDirection();
                    if (dashDir != (Vector3)Vector2.up && ((Vector2)dashDir == Vector2.zero || rb.velocity == Vector2.zero))
                    {
                        dashDir = (facingLeft) ? new Vector2(1, 0) : new Vector2(-1, 0);
                    }
                    DashEffect.CreateDashEffect(transform.position, -dashDir, Vector3.Distance(transform.position, dashDir), dashEffect.transform);
                    dashSpeed = MOVE_SPEED * 3;
                    rb.gravityScale = 0;
                    state = State.Dashing;
                }

                //----- Barrier -----//
                if (Input.GetKeyDown(KeyCodeConstants.KEY_CODE_BARRIER)) player.BarrierStart();

                //----- Nova -----//
                if (Input.GetKeyDown(KeyCodeConstants.KEY_CODE_NOVA)) player.NovaStart();

                break;
            case State.Dashing:
                float dashSpeedDropMultiplier = 2f;
                if (rb.velocity.y > 0) dashSpeedDropMultiplier = 3f;
                dashSpeed -= dashSpeed * dashSpeedDropMultiplier * Time.deltaTime;

                float dashSpeedMinimum = MOVE_SPEED;
                if (dashSpeed < dashSpeedMinimum || state == State.Normal)
                {
                    rb.gravityScale = defGravity;
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
                //----- Move -----//
                rb.velocity = new Vector2((!isGrounded && isColliding) ? 0 : moveDir.x * MOVE_SPEED, rb.velocity.y);

                //----- Jump -----//
                if (rb.velocity.y < 0 && !isJumping)
                    rb.velocity += new Vector2(rb.velocity.x, 1) * Physics2D.gravity.y * JUMP_FALL_MULTIPLIER * Time.deltaTime;
                else if (rb.velocity.y > 0 && !isJumping)
                    rb.velocity += new Vector2(rb.velocity.x, 1) * Physics2D.gravity.y * JUMP_LOW_MULTIPLIER * Time.deltaTime;
                else if (rb.velocity.y > 0 && isJumping)
                    rb.velocity -= new Vector2(rb.velocity.x, 1) * Physics2D.gravity.y * JUMP_LOW_MULTIPLIER * Time.deltaTime;
                break;
            case State.Dashing:
                //----- Dash -----//
                rb.velocity = dashDir * dashSpeed;
                break;
        }
    }

    private void SetFacing(float xVelocity)
    {
        if (xVelocity > 0 && !facingLeft)
        {
            facingLeft = true;
            spriteRenderer.flipX = false;
        }
        else if (xVelocity < 0 && facingLeft)
        {
            facingLeft = false;
            spriteRenderer.flipX = true;
        }
    }

    private Vector2 GetInputDirection()
    {
        float x = Input.GetAxisRaw(EditorConstants.INPUT_AXIS_HORIZONTAL);
        float y = Input.GetAxisRaw(EditorConstants.INPUT_AXIS_VERTICAL);
        if (x == 0 && y > 0) return new Vector2(0, 1); // North
        else if (x > 0 && y > 0) return new Vector2(1, 1).normalized; // Northeast
        else if (x > 0 && y == 0) return new Vector2(1, 0); // East
        else if (x > 0 && y < 0) return new Vector2(1, -1).normalized; // Southeast
        else if (x == 0 && y < 0) return new Vector2(0, -1); // South
        else if (x < 0 && y < 0) return new Vector2(-1, -1).normalized; // Southwest
        else if (x < 0 && y == 0) return new Vector2(-1, 0); // West
        else if (x < 0 && y > 0) return new Vector2(-1, 1).normalized; // Northwest
        return Vector2.zero;
    }

    private void ResetAttack()
    {
        slashHitBoxLeft.SetActive(false);
        slashHitBoxRight.SetActive(false);
        slashUpHitBox.SetActive(false);
        animator.Play("Player_Idle");
        attackOnCD = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(EditorConstants.LAYER_GROUND))
        {
            // Change to cancel on collision with all environment objects, not just ground. (Or do we really need to?)
            isJumping = false;
            if (state == State.Dashing) SetState(State.Normal);
        }
    }
}
