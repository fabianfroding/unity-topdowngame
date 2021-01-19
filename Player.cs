using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Unit
{
    private Sprite defaultSprite;

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    Sprite[] sprite;

    protected override void Start()
    {
        base.Start();
        health = 3;
        healthText.text = "Health: " + health + "/3";

        defaultSprite = GetComponent<SpriteRenderer>().sprite;
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

    public void SetSpriteAngle(Vector3 vec)
    {
        float angle = Vector3.Angle(vec, Vector3.right) * -Vector3.Cross(vec, Vector3.right).normalized.z;
        int spriteIndex = 0;
        if (angle >= 67.5 && angle < 112.5) spriteIndex = 0;
        else if (angle >= 22.5 && angle < 67.5) spriteIndex = 1;
        else if (vec.x == 1 && (angle >= -22.5 && angle < 22.5)) spriteIndex = 2;
        else if (angle >= -67.5 && angle < -22.5) spriteIndex = 3;
        else if (angle >= -112.5 && angle < -67.5) spriteIndex = 4;
        else if (angle >= -157.5 && angle < -112.5) spriteIndex = 5;
        else if (vec.x == -1 && (angle < 22.5 || angle < -157.5)) spriteIndex = 6;
        else if (angle >= 112.5 && angle < 157.5) spriteIndex = 7;
        else ResetSpriteAngle();

        spriteRenderer.sprite = sprite[spriteIndex];
        defaultSprite = sprite[spriteIndex];
    }

    private void ResetSpriteAngle() {
        spriteRenderer.sprite = defaultSprite;
    }

    private void GameOver()
    {
        SceneManager.LoadScene("MainMenu");
    }
}