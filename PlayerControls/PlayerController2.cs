﻿using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    private const float MOVE_SPEED = 8f;
    private const float JUMP_SPEED = 12.5f;
    private const float JUMP_FALL_MULTIPLIER = 2f;
    private const float JUMP_LOW_MULTIPLIER = 1.5f;
    private const float JUMP_TIME_MAX = 0.35f;
    private const KeyCode KEY_CODE_DASH = KeyCode.O;

    public static bool isEnabled = true;

    [SerializeField] private GameObject slashHitBoxLeft;
    [SerializeField] private GameObject slashHitBoxRight;
    [SerializeField] private GameObject slashUpHitBox;
    [SerializeField] private GameObject swingSoundRef;
    [SerializeField] private GameObject dashEffect;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform groundCheckLeft;
    [SerializeField] Transform groundCheckRight;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 moveDir;
    private Vector3 dashDir;
    private bool isGrounded;
    private bool isJumping = false;
    private bool facingLeft = true;
    private bool attackOnCD = false;
    private float dashSpeed;
    private float defGravity;
    private float jumpTimeCounter;
    private State state;

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

    public State GetState()
    {
        return state;
    }

    public void SetState(State newState)
    {
        if (state == State.Dashing && newState != State.Dashing)
        {
            rb.gravityScale = defGravity;
        }
        state = newState;
    }

    //==================== PRIVATE ====================//
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        defGravity = rb.gravityScale;
        state = State.Normal;
    }

    private void FixedUpdate()
    {
        if (isEnabled && GetComponent<Player>().health > 0)
        {
            Move();
        }
    }

    private void Update()
    {
        if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics2D.Linecast(transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics2D.Linecast(transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isEnabled && GetComponent<Player>().health > 0)
        {
            ProcessInputs();
        }
    }

    private void ProcessInputs()
    {
        switch (state)
        {
            case State.Normal:
                //----- Move -----//
                float x = Input.GetAxisRaw("Horizontal");
                moveDir = new Vector2(x, 0).normalized;
                SetFacing(x);

                //----- Jump -----//
                if (isGrounded && Input.GetKeyDown(KeyCode.Space))
                {
                    isJumping = true;
                    jumpTimeCounter = JUMP_TIME_MAX;
                    rb.velocity = new Vector2(rb.velocity.x * 0.8f, 1) * JUMP_SPEED; // TODO: Move to Move()
                }

                if (Input.GetKey(KeyCode.Space) && isJumping)
                {
                    if (jumpTimeCounter > 0)
                    {
                        rb.velocity = new Vector2(rb.velocity.x * 0.8f, 1) * JUMP_SPEED; // TODO: Move to Move()
                        jumpTimeCounter -= Time.deltaTime;
                    }
                    else
                    {
                        isJumping = false;
                    }
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    isJumping = false;
                }

                //----- Attack -----//
                if (!attackOnCD && Input.GetKeyDown(KeyCode.K))
                {
                    attackOnCD = true;
                    GameObject swingSound = Instantiate(swingSoundRef, transform.position, Quaternion.identity);
                    Destroy(swingSound, swingSound.GetComponent<AudioSource>().clip.length);
                    if (Input.GetKey(KeyCode.W))
                    {
                        animator.Play("PlayerAttackUp");
                        slashUpHitBox.SetActive(true);
                    }
                    else
                    {
                        animator.Play("PlayerAttack");
                        if (facingLeft) slashHitBoxRight.SetActive(true);
                        else slashHitBoxLeft.SetActive(true);
                    }
                    Invoke("ResetAttack", 0.5f);
                }

                //----- Dash -----//
                if (Input.GetKeyDown(KEY_CODE_DASH))
                {
                    isJumping = false;
                    dashDir = GetInputDirection();
                    if (dashDir != (Vector3)Vector2.up && ((Vector2)dashDir == Vector2.zero || rb.velocity == Vector2.zero))
                    {
                        if (facingLeft)
                        {
                            dashDir = new Vector2(1, 0);
                        }
                        else
                        {
                            dashDir = new Vector2(-1, 0);
                        }
                    }
                    DashEffect.CreateDashEffect(transform.position, -dashDir, Vector3.Distance(transform.position, dashDir), dashEffect.transform);
                    dashSpeed = MOVE_SPEED * 3;
                    rb.gravityScale = 0;
                    state = State.Dashing;
                }
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
                rb.velocity = new Vector2(moveDir.x * MOVE_SPEED, rb.velocity.y);

                //----- Jump -----//
                if (rb.velocity.y < 0 && !isJumping)
                {
                    rb.velocity += new Vector2(rb.velocity.x, 1) * Physics2D.gravity.y * (JUMP_FALL_MULTIPLIER - 1) * Time.deltaTime;
                }
                else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space) && !isJumping)
                {
                    rb.velocity += new Vector2(rb.velocity.x, 1) * Physics2D.gravity.y * (JUMP_LOW_MULTIPLIER - 1) * Time.deltaTime;
                }

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
            spriteRenderer.flipX = true;
        }
        else if (xVelocity < 0 && facingLeft)
        {
            facingLeft = false;
            spriteRenderer.flipX = false;
        }
    }

    private Vector2 GetInputDirection()
    {
        // North
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            return new Vector2(0, 1);
        }
        // Northeast
        else if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            return new Vector2(1, 1).normalized;
        }
        // East
        else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            return new Vector2(1, 0);
        }
        // Southeast
        else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            return new Vector2(1, -1).normalized;
        }
        // South
        else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            return new Vector2(0, -1);
        }
        // Southwest
        else if (!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            return new Vector2(-1, -1).normalized;
        }
        // West
        else if (!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            return new Vector2(-1, 0);
        }
        // Northwest
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            return new Vector2(-1, 1).normalized;
        }

        return Vector2.zero;
    }

    private void ResetAttack()
    {
        slashHitBoxLeft.SetActive(false);
        slashHitBoxRight.SetActive(false);
        slashUpHitBox.SetActive(false);
        animator.Play("PlayerIdle");
        attackOnCD = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            // Change to cancel on collision with all environment objects, not just ground.
            isJumping = false;
            if (state == State.Dashing)
            {
                SetState(State.Normal);
            }
        }
    }
}