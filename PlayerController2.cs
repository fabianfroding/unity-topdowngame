using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    private const float MOVE_SPEED = 8f;
    private const float JUMP_SPEED = 3f;
    private const float JUMP_FALL_MULTIPLIER = 2f;
    private const float JUMP_LOW_MULTIPLIER = 1.5f;

    public static bool isEnabled = true;

    [SerializeField] GameObject slashHitBox;
    [SerializeField] GameObject slashUpHitBox;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 moveDir;
    private bool hasJumped = false;
    private bool facingLeft = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (isEnabled)
        {
            Move();
        }
    }

    private void Update()
    {
        if (isEnabled)
        {
            ProcessInputs();
        }
    }

    private void ProcessInputs()
    {
        float x = Input.GetAxisRaw("Horizontal");
        moveDir = new Vector2(x, 0).normalized;

        SetFacing(x);

        if (!hasJumped && Input.GetKeyDown(KeyCode.Space))
        {
            hasJumped = true;
            rb.velocity = new Vector2(rb.velocity.x * 2f, 1) * JUMP_SPEED * MOVE_SPEED; // TODO: Move to Move()
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (Input.GetKey(KeyCode.W))
            {
                animator.Play("PlayerAttackUp");
                slashUpHitBox.GetComponent<BoxCollider2D>().enabled = true;
                Invoke("ResetAnimToIdle", 0.06f);
            }
            else
            {
                animator.Play("PlayerAttack");
                slashUpHitBox.GetComponent<BoxCollider2D>().enabled = true;
                Invoke("ResetAnimToIdle", 0.06f);
            }
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveDir.x * MOVE_SPEED, rb.velocity.y);

        if (rb.velocity.y < 0)
        {
            Debug.Log("vel<0");
            rb.velocity += new Vector2(rb.velocity.x, 1) * Physics2D.gravity.y * (JUMP_FALL_MULTIPLIER - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            Debug.Log("vel>0");
            rb.velocity += new Vector2(rb.velocity.x, 1) * Physics2D.gravity.y * (JUMP_LOW_MULTIPLIER - 1) * Time.deltaTime;
        }
    }

    private void SetFacing(float xVelocity)
    {
        if (xVelocity > 0 && !facingLeft)
        {
            facingLeft = true;
            spriteRenderer.flipX = true;
        }
        else if (xVelocity < 0 && facingLeft)
        {
            facingLeft = false;
            spriteRenderer.flipX = false;
        }
    }

    private void ResetAnimToIdle()
    {
        // TODO: Reset to whatever was playing before attack.
        slashHitBox.GetComponent<BoxCollider2D>().enabled = false;
        slashUpHitBox.GetComponent<BoxCollider2D>().enabled = false;
        animator.Play("PlayerIdle");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            hasJumped = false;
            Debug.Log("Collide with ground.");
        }
    }
}
