using UnityEngine;

public class Unit : MonoBehaviour
{
    public int health;
    public float moveSpeed;
    public Material matDefault;
    public Material matWhite;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;

    //========== PUBLIC METHODS ==========//
    public void TakeDamage(int amount)
    {
        Debug.Log(gameObject.name + " take dmg");
        health -= amount;
        spriteRenderer.material = matWhite;
        Invoke("ResetMaterial", .1f);
        if (health <= 0)
        {
            health = 0;
            DestroySelf();
        }
    }

    public virtual void DestroySelf()
    {
        Destroy(gameObject);
    }

    //========== PRIVATE METHODS ==========//
    private void ResetMaterial()
    {
        spriteRenderer.material = matDefault;
    }

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        matDefault = spriteRenderer.material;
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
    }

}
