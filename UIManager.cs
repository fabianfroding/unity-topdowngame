using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject ClockUI;
    public GameObject HealthUI;

    public void HealthUISetActive(bool flag)
    {
        HealthUI.SetActive(flag);
    }

    public void ClockUISetActive(bool flag)
    {
        ClockUI.SetActive(flag); // TODO: Make sure clock doesnt update while its deactivated.
    }

}
