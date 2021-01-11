using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Unit
{
    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Camera cam;

    protected override void Start()
    {
        base.Start();
        health = 3;
        healthText.text = "Health: " + health + "/3";
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
            DestroySelf();
            Invoke("GameOver", 3f);
        }
    }

    public override void DestroySelf()
    {
        spriteRenderer.enabled = false;
        rb.velocity = new Vector2(0, 0);
        GetComponent<CircleCollider2D>().enabled = false;
    }

    private void GameOver()
    {
        SceneManager.LoadScene("MainMenu");
    }
}