using UnityEngine;

public class Nova : MonoBehaviour
{
    private GameObject source;

    //==================== PUBLIC ====================//
    public void Activate(GameObject source)
    {
        this.source = source;
        if (EquipmentMenu.instance != null && EquipmentMenu.instance.IsEquipped("NovaRange")) transform.localScale *= 2f;
        Destroy(gameObject, 2.23f); // Sound clip length
    }

    //==================== PRIVATE ====================//
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(GameUtils.TAG_ENEMY))
        {
            other.gameObject.GetComponent<Unit>().TakeDamage(source, 1);
        }
    }
}
