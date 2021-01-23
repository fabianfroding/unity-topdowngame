using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject itemBTN;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < Inventory.instance.slots.Length; i++)
            {
                if (!Inventory.instance.hasItem[i])
                {
                    // Add item
                    Inventory.instance.hasItem[i] = true;
                    Instantiate(itemBTN, Inventory.instance.slots[i].transform, false);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
