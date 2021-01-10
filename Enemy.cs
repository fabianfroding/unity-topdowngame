using UnityEngine;

public class Enemy : Unit
{
    [SerializeField]
    private GameObject enemyProjectile;

    private FieldOfView fov;

    private bool attackOnCooldown = false;
    private bool attack0AnimPlaying = false;

    protected override void Start()
    {
        base.Start();
        health = 5;

        fov = new FieldOfView();
    }

    private void FixedUpdate()
    {
        if (health <= 0)
        {
            DestroySelf();
        }

        if (fov.visiblePlayers.Count > 0)
        {
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

        Invoke("ResetAttackCooldown", 5f);
    }

    private void ResetAttackCooldown()
    {
        attackOnCooldown = false;
    }
}
