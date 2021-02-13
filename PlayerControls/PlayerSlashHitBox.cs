using UnityEngine;

public class PlayerSlashHitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(EditorConstants.TAG_ENEMY) && !other.isTrigger)
        {
            other.GetComponent<Unit>().TakeDamage(transform.parent.gameObject, 1);
        }
    }
}
