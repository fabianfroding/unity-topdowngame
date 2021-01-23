using UnityEngine;

public class MaterialItem : MonoBehaviour
{
    [SerializeField] private int materialTypeId;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < Inventory.instance.materialTypes; i++)
            {
                if (materialTypeId == i)
                {
                    Inventory.instance.materials[i]++;
                    Inventory.instance.DebugMaterials();
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
