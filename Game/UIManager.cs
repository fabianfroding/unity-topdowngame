using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject ClockUI;
    [SerializeField] private GameObject HealthUI;
    [SerializeField] private GameObject player; // TODO: Remove and make player hp static.
    [SerializeField] private int numOfTears;
    [SerializeField] private Image[] tears;
    [SerializeField] private Sprite tearFilled;
    [SerializeField] private Sprite tearEmpty;

    //==================== PUBLIC ====================//
    public void HealthUISetActive(bool flag)
    {
        HealthUI.SetActive(flag);
    }

    public void ClockUISetActive(bool flag)
    {
        ClockUI.SetActive(flag); // TODO: Make sure clock doesnt update while its deactivated.
    }

    //==================== PRIVATE ====================//
    private void Start()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        int hp = player.GetComponent<Player>().health;
        if (hp > numOfTears)
        {
            hp = numOfTears;
        }

        for (int i = 0; i < tears.Length; i++)
        {
            if (i < hp)
            {
                tears[i].sprite = tearFilled;
            }
            else
            {
                tears[i].sprite = tearEmpty;
            }

            if (i < numOfTears)
            {
                tears[i].enabled = true;
            }
            else
            {
                tears[i].enabled = false;
            }
        }
    }
}
