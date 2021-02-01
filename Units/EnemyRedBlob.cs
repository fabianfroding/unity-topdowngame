using UnityEngine;

public class EnemyRedBlob : Unit
{
    [SerializeField] private GameObject enemyProjectile;
    [SerializeField] private AudioSource aggroSound;
    [SerializeField] private float attackCooldown;
    private bool attackOnCooldown = false;
    private FieldOfView fov;
    private bool seenPlayer = false;

    //==================== PUBLIC ====================//
    public void FaceTarget(GameObject target)
    {
        Vector3 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rb.rotation = angle;
    }

    //==================== PUBLIC ====================//
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
        
        if (GetTarget() != null && GetTarget().CompareTag("Player"))
        {
            StopPatrol();

            if (!seenPlayer) {
                aggroSound.Play();
            }
            seenPlayer = true;

            FaceTarget(fov.visiblePlayers.ToArray()[0].gameObject);

            if (!attackOnCooldown)
            {
                Attack();
            }
        }
        else
        {
            seenPlayer = false;
        }
    }

    private void Attack()
    {
        attackOnCooldown = true;

        GameObject projectile = Instantiate(enemyProjectile, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().source = this.gameObject;
        projectile.GetComponent<Projectile>().SetDirection(fov.visiblePlayers.ToArray()[0].gameObject.transform.position);
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
        CancelInvoke("Patrol");
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
            StopPatrol();
        }
    }

    private GameObject GetTarget()
    {
        return fov.FindVisiblePlayer();
    }
}
