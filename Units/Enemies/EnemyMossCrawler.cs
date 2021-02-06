using UnityEngine;

public class EnemyMossCrawler : Enemy
{
    [SerializeField] private Transform groundCheckFront;
    [SerializeField] private Transform sideCheckFront;

    private bool movingRight = false;
    private Vector3 baseScale;

    //==================== PUBLIC ====================//
    protected override void Start()
    {
        base.Start();
        baseScale = transform.localScale;
    }

    //==================== PRIVATE ====================//
    private void FixedUpdate()
    {
        Patrol();
    }

    private void Patrol()
    {
        rb.velocity = new Vector2(movingRight ? moveSpeed : -moveSpeed, rb.velocity.y);
        RaycastHit2D groundHit = Physics2D.Linecast(transform.position, groundCheckFront.position, 1 << LayerMask.NameToLayer("Ground"));

        if (!groundHit)
        {
            Debug.Log("TRIG");
            ChangeFacingDirection();
        }
    }

    private void ChangeFacingDirection()
    {
        Vector3 newScale = baseScale;
        if (movingRight) newScale.x = baseScale.x;
        else newScale.x = -baseScale.x;
        transform.localScale = newScale;
        movingRight = !movingRight;
    }
}
