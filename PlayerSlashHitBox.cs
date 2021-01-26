using UnityEngine;

public class PlayerSlashHitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Unit>().TakeDamage(1);
        }
    }
}
