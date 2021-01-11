using UnityEngine;

public class Enemy : Unit
{
    [SerializeField]
    private GameObject enemyProjectile;

    [SerializeField]
    private float attackCooldown;
    private bool attackOnCooldown = false;

    private FieldOfView fov;

    protected override void Start()
    {
        base.Start();

        fov = GetComponent<FieldOfView>();

        Patrol();
    }

    private void FixedUpdate()
    {
        if (health <= 0)
        {
            DestroySelf();
        }
        
        if (fov.visiblePlayers.Count > 0)
        {
            // Set rotation to player direction.
            Transform target = fov.visiblePlayers.ToArray()[0].gameObject.transform;
            Vector3 dir = target.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            rb.rotation = angle;

            if (!attackOnCooldown)
            {
                attackOnCooldown = true;
                Attack();
            }
        }
    }

    private void Attack()
    {
        attackOnCooldown = true;

        GameObject target = fov.visiblePlayers.ToArray()[0].gameObject;

        GameObject projectile = Instantiate(enemyProjectile, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().source = this.gameObject;
        projectile.GetComponent<Projectile>().SetDirection(target.transform.position);
        projectile.GetComponent<Projectile>().InvokeDestroySelf(3f);

        Invoke("ResetAttackCooldown", attackCooldown);
    }

    private void Patrol()
    {
        int[] iArr = new int[] { -1, 1 };
        Vector2 moveDir = new Vector2(iArr[Random.Range(0, iArr.Length)], iArr[Random.Range(0, iArr.Length)]).normalized;
        rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);

        Vector2 dir = rb.velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90;

        Invoke("StopPatrol", Random.Range(1.5f, 3f));
    }

    private void StopPatrol()
    {
        rb.velocity = new Vector2(0, 0);
        Invoke("Patrol", Random.Range(3f, 8f));
    }

    private void ResetAttackCooldown()
    {
        attackOnCooldown = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Environment"))
        {
            CancelInvoke("StopPatrol");
            StopPatrol();
        }
    }
}
