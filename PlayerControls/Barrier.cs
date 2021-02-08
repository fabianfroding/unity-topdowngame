using UnityEngine;

public class Barrier : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.activeSelf && EquipmentMenu.instance != null && EquipmentMenu.instance.IsEquipped("BarrierReflect"))
        {
            GameObject source = null;
            if (IsColliderHostileProjectile(other))
            {
                source = other.gameObject.GetComponent<Projectile>().source;
            }
            else if (other.CompareTag("Enemy"))
            {
                source = other.gameObject;
            }
            if (source != null)
            {
                gameObject.SetActive(false);
                source.GetComponent<Unit>().TakeDamage(gameObject.transform.parent.gameObject, 1); // TODO: Get amount actually dealt? Unit dmg field?
            }
        }

        if (IsColliderHostileProjectile(other))
        {
            // TODO: Find a way to play projectile death sound before destroying it (add method CreateDeathSound).
            Destroy(other.gameObject);
        }
    }

    private bool IsColliderHostileProjectile(Collider2D other)
    {
        return other.CompareTag("Projectile") && other.GetComponent<Projectile>().source.CompareTag("Enemy");
    }
}