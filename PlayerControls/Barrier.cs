using UnityEngine;

public class Barrier : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile") && other.GetComponent<Projectile>().source.CompareTag("Enemy"))
        {
            // TODO: Find a way to play projectile death sound before destroying it (add method CreateDeathSound).
            Destroy(other.gameObject);
        }
    }
}