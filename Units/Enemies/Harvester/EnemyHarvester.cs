using UnityEngine;

public class EnemyHarvester : Enemy
{
    private const float DODGE_OFFSET = 12f;

    [SerializeField] private GameObject castPoint;
    [SerializeField] private Collider2D dmgCollider;
    [SerializeField] private Collider2D dodgeCollider;
    [SerializeField] private GameObject harvesterOrbRef;
    [SerializeField] private GameObject dodgeSoundRef;
    [SerializeField] private float dodgeCD = 8f;
    private bool dodgeOnCD = false;

    //==================== PUBLIC ====================//
    public override void TakeDamage(GameObject src, int amt)
    {
        base.TakeDamage(src, amt);
        CancelInvoke("ResetDodgeCD");
        ResetDodgeCD();
    }

    //==================== PRIVATE ====================//
    private void FixedUpdate()
    {
        if (CanSeePlayer(4f)) Attack();
    }

    private void Attack()
    {
        if (!attackOnCD)
        {
            attackOnCD = true;
            animator.Play("Harvester_Attack");
            Invoke("ResetAnimToIdle", 0.75f);
            Invoke("ResetAttackCD", attackCD);
        }
    }

    private void AttackAnimationEvent()
    {
        GameObject orb = Instantiate(harvesterOrbRef);
        orb.GetComponent<HarvesterOrb>().source = this.gameObject;
        if (facingRight) orb.GetComponent<SpriteRenderer>().flipX = true;
        orb.transform.position = castPoint.transform.position;
    }

    private bool CanSeePlayer(float dist)
    {
        Vector3 endPos = castPoint.transform.position + Vector3.right * (!facingRight ? -dist : dist);
        RaycastHit2D hit = Physics2D.Linecast(castPoint.transform.position, endPos, 1 << LayerMask.NameToLayer(EditorConstants.LAYER_PLAYER));
        if (hit.collider != null && 
            !hit.collider.isTrigger && 
            hit.collider.gameObject.CompareTag(EditorConstants.TAG_PLAYER)) return true;
        //Debug.DrawLine(castPoint.transform.position, endPos, Color.red);

        return false;
    }

    private void Dodge(GameObject attacker, float offset)
    {
        dodgeOnCD = true;
        dodgeCollider.enabled = false;
        animator.Play("Harvester_Dodge");

        GameObject sound = Instantiate(dodgeSoundRef, transform.position, Quaternion.identity);
        Destroy(sound, sound.GetComponent<AudioSource>().clip.length);

        // TODO: Do a raycast in env layer. If hit, then set offest to hit - const, so that enemy doesnt tp into env.
        transform.position = new Vector3(attacker.transform.position.x + offset, transform.position.y, transform.position.z);
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        facingRight = !facingRight;

        Invoke("ResetDodgeCD", dodgeCD);
        Invoke("ResetAnimToIdle", 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(EditorConstants.TAG_PLAYER)) // TODO: Add check for player projectile.
        {
            if (!dodgeOnCD)
            {
                Dodge(other.gameObject, facingRight ? DODGE_OFFSET : -DODGE_OFFSET);
                Attack();
            }
        }
    }

    private void ResetDodgeCD()
    {
        dodgeCollider.enabled = true;
        dodgeOnCD = false;
    }

    private void ResetAnimToIdle()
    {
        animator.Play("Harvester_Idle");
    }
}
