using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject ClockUI;
    public GameObject HealthUI;

    private void Awake()
    {
        instance = this;
    }

    public void HealthUISetActive(bool flag)
    {
        HealthUI.SetActive(flag);
    }

    public void ClockUISetActive(bool flag)
    {
        ClockUI.SetActive(flag); // TODO: Make sure clock doesnt update while its deactivated.
    }

}
