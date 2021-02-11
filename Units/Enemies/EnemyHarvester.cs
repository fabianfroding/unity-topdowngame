using UnityEngine;

public class EnemyHarvester : Enemy
{
    private const float DODGE_CD = 8f;
    private const float DODGE_OFFSET = 12f;

    [SerializeField] private Collider2D dmgCollider;
    [SerializeField] private Collider2D dodgeCollider;
    private bool dodgeOnCD = false;

    //==================== PRIVATE ====================//
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) // TODO: Add check for player projectile.
        {
            // Teleport behind player.
            if (!dodgeOnCD)
            {
                Dodge(other.gameObject, facingRight ? DODGE_OFFSET : -DODGE_OFFSET);
            }
        }
    }

    private void Dodge(GameObject attacker, float offset)
    {
        dodgeOnCD = true;
        dodgeCollider.enabled = false;
        animator.Play("Harvester_Dodge");
        transform.position = new Vector3(attacker.transform.position.x + offset, transform.position.y, transform.position.z);
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        facingRight = !facingRight;

        Invoke("ResetDodgeCD", DODGE_CD);
        Invoke("ResetAnimToIdle", 0.5f);
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
