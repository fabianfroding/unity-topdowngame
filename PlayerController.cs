using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;

    public GameObject bulletRef;

    private Vector2 moveDir;

    private bool attackOnCooldown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        attackOnCooldown = false;
    }

    void Update()
    {
        if (GetComponent<Unit>().health > 0)
        {
            ProcessInputs();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ProcessInputs()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        moveDir = new Vector2(x, y).normalized;

        if (x != 0 || y != 0)
        {
            Vector3 moveVector = Vector3.zero;
            moveVector.x = Input.GetAxis("Horizontal");
            moveVector.y = Input.GetAxis("Vertical");
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

    }

    private void Move()
    {
        rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }

    private void ResetAttackCooldown()
    {
        attackOnCooldown = false;
    }
}
