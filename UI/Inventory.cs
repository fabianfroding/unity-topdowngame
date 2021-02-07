using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public int[] materials;
    /* 1: Monster Claw
     * 2: Beast Hide
     * 3: Bone Horn
     * 4: Pristine Ore
     * 5: Sticky Blob
     * 6: Crimson Feather
     * 7: Frost Crystal
     * 8: Dark Skull
     * 9: Amber Relic
     * 10: Desert Thistle
     * 11: Crimson Feather
     * 12: Lizard Scale
     * 13: Dragon Hide
     * 14: Ancient Flower
     * 15: Sky Stone
     */
    public readonly int materialTypes = 14;

    public bool[] hasItem;
    public GameObject[] slots;

    public void DebugMaterials()
    {
        for (int i = 0; i < materialTypes; i++)
        {
            Debug.Log("Material[" + i + "]: " + materials[i]);
        }
    }

    private void Awake()
    {
        instance = this;
        materials = new int[15];
    }
}
