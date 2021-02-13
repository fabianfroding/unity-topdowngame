using UnityEngine;

public class EnemyMossCrawler : Enemy
{
    [SerializeField] private Transform groundCheckFront;
    [SerializeField] private Transform sideCheckFront;
    private Vector3 baseScale;

    //==================== PRIVATE ====================//
    protected override void Start()
    {
        baseScale = transform.localScale;
        base.Start();
    }

    private void FixedUpdate()
    {
        Patrol();
    }

    private void Patrol()
    {
        rb.velocity = new Vector2(facingRight ? moveSpeed : -moveSpeed, rb.velocity.y);
        if (!Physics2D.Linecast(transform.position, groundCheckFront.position, 1 << LayerMask.NameToLayer(EditorConstants.LAYER_GROUND))) ChangeFacingDirection();
    }

    private void ChangeFacingDirection()
    {
        Vector3 newScale = baseScale;
        if (facingRight) newScale.x = baseScale.x;
        else newScale.x = -baseScale.x;
        transform.localScale = newScale;
        facingRight = !facingRight;
    }
}
