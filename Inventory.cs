using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public bool[] hasItem;
    public GameObject[] slots;

    private void Awake()
    {
        instance = this;
    }
}
