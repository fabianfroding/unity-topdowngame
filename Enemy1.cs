using UnityEngine;

public class Enemy1 : Unit
{
    [SerializeField]
    Transform castPoint;

    Animator animator;
    Object darkOrbRef;

    private bool attackOnCooldown = false;
    private bool attack0AnimPlaying = false;

    protected override void Start()
    {
        base.Start();
        health = 10;
        //animator = GetComponent<Animator>();
        //matWhite = (Material)Resources.Load("EnemyHarvesterWhiteFlash", typeof(Material));
        //darkOrbRef = Resources.Load("DarkOrb");
    }

    private void FixedUpdate()
    {
        if (health <= 0)
        {
            Invoke("DestroySelf", 0f);
        }
        if (CanSeePlayer(5f) && !attackOnCooldown)
        {
            /*
            Debug.Log("Sees player");
            Attack();
            */
        }
    }

    public void Dodge(GameObject attacker, float offset)
    {
        /*
        animator.Play("Harvester_Dodge");
        Vector3 newPos = new Vector3(attacker.transform.position.x + offset, attacker.transform.position.y, 0);
        transform.position = newPos;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);

        if (isFacingLeft)
        {
            isFacingLeft = false;
        }
        else
        {
            isFacingLeft = true;
        }

        Invoke("ResetAnim", 0.5f);
        */
    }

    private void Attack()
    {
        /*
        Debug.Log("Attacking");
        attackOnCooldown = true;

        animator.Play("Harvester_Attack0");
        attack0AnimPlaying = true;
        Invoke("ResetAnim", 0.15f);
        GameObject darkOrb = (GameObject)Instantiate(darkOrbRef);
        darkOrb.GetComponent<DarkOrb>().source = this.gameObject;
        float x = -0.6f;

        if (spriteRenderer.flipX)
        {
            x = 0.6f;
            darkOrb.GetComponent<SpriteRenderer>().flipX = true;
        }
        darkOrb.transform.position = new Vector3(transform.position.x + x, transform.position.y + -0.03f, -1);

        Invoke("ResetAttackCooldown", 5f);
        */
    }

    private bool CanSeePlayer(float dist)
    {
        /*
        Vector2 endPos;

        if (isFacingLeft)
        {
            endPos = castPoint.position + Vector3.right * -dist;
        }
        else
        {
            endPos = castPoint.position + Vector3.right * dist;
        }

        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Action"));

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }
        }

        Debug.DrawLine(castPoint.position, endPos, Color.red);

        */
        return false;
    }

    private void ResetAttackCooldown()
    {
        attackOnCooldown = false;
    }

    private void ResetAnim()
    {
        /*
        animator.Play("Harvester_Idle");
        if (attack0AnimPlaying)
        {
            attack0AnimPlaying = false;
        }
        */
    }

}
