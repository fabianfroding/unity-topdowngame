using UnityEngine;

public class Enemy : Unit
{
    [SerializeField]
    private GameObject enemyProjectile;

    private FieldOfView fov;

    private bool attackOnCooldown = false;

    protected override void Start()
    {
        base.Start();
        health = 5;
        moveSpeed = 1.5f;

        fov = GetComponent<FieldOfView>();

        Patrol();
    }

    private void FixedUpdate()
    {
        if (health <= 0)
        {
            DestroySelf();
        }
        
        if (fov.visiblePlayers.Count > 0 && !attackOnCooldown)
        {
            attackOnCooldown = true;
            Attack();
        }
    }

    private void Attack()
    {
        Debug.Log("Attacking");
        attackOnCooldown = true;

        GameObject target = fov.visiblePlayers.ToArray()[0].gameObject;

        GameObject projectile = Instantiate(enemyProjectile, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().source = this.gameObject;
        projectile.GetComponent<Projectile>().SetDirection(target.transform.position);
        projectile.GetComponent<Projectile>().InvokeDestroySelf(3f);

        Invoke("ResetAttackCooldown", 5f);
    }

    private void Patrol()
    {
        Debug.Log("Patrol");
        int[] iArr = new int[] { -1, 1 };
        Vector2 moveDir = new Vector2(iArr[Random.Range(0, iArr.Length)], iArr[Random.Range(0, iArr.Length)]).normalized;
        //rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);

        float rand = Random.Range(1, 360);
        rb.SetRotation(rand);
        rb.velocity = transform.right.normalized * moveSpeed;

        Invoke("StopPatrol", Random.Range(1.5f, 3f));
    }

    private void StopPatrol()
    {
        Debug.Log("Stop Patrol");
        rb.velocity = new Vector2(0, 0);
        Invoke("Patrol", Random.Range(3f, 8f));
    }

    private void ResetAttackCooldown()
    {
        attackOnCooldown = false;
    }
}
