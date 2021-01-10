using UnityEngine;
using UnityEngine.UI;

public class Player : Unit
{
    [SerializeField]
    private Text healthText;

    protected override void Start()
    {
        base.Start();
        health = 3;
    }

    public void UpdateHealthText()
    {
        int currHealth = health <= 0 ? 0 : health;
        healthText.text = "Health: " + currHealth + "/3";
    }

    private void FixedUpdate()
    {
        if (health <= 0)
        {
            Invoke("DestroySelf", 0f);
        }
    }

}