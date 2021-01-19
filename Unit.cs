using UnityEngine;

public class Unit : MonoBehaviour
{
    public int health;
    public float moveSpeed;
    public Material matDefault;
    public Material matWhite;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        matDefault = spriteRenderer.material;
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
    }

    public void TakeDamage()
    {
        spriteRenderer.material = matWhite;
        Invoke("ResetMaterial", .1f);
        Debug.Log(gameObject.name + " take dmg");
    }

    public void ResetMaterial()
    {
        spriteRenderer.material = matDefault;
    }

    public virtual void DestroySelf()
    {
        Destroy(gameObject);
    }
}
