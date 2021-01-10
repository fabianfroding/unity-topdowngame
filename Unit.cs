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
    }

    public void ResetMaterial()
    {
        spriteRenderer.material = matDefault;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
