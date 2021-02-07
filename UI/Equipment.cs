using UnityEngine;

public class Equipment : MonoBehaviour
{
    public bool collected = false;
    public string description;

    [HideInInspector] public Transform orgPos;
}
