using UnityEngine;

public class Nova : MonoBehaviour
{
    private GameObject source;
    private bool dmgDealt = false;

    //==================== PUBLIC ====================//
    public void Activate(GameObject source)
    {
        this.source = source;
        if (EquipmentMenu.instance != null && EquipmentMenu.instance.IsEquipped("NovaRange")) transform.localScale *= 2f;
        Destroy(gameObject, 2.23f); // Sound clip length
        Invoke("SetDmgDealt", 0.1f);
    }

    //==================== PRIVATE ====================//
    private void SetDmgDealt()
    {
        dmgDealt = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!dmgDealt && other.gameObject.CompareTag(EditorConstants.TAG_ENEMY))
        {
            other.gameObject.GetComponent<Unit>().TakeDamage(source, 1);
        }
    }
}
