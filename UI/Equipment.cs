using UnityEngine;

public class Equipment : MonoBehaviour
{
    public bool collected = false;
    public string description;

    [HideInInspector] public bool equipped = false;
    [HideInInspector] public Transform orgPos;
}
