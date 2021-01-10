using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject source;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void InvokeDestroySelf(float duration)
    {
        Invoke("DestroySelf", duration);
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if ((source.CompareTag("Player") && col.CompareTag("Enemy")) || 
            (source.CompareTag("Enemy") && col.CompareTag("Player")))
        {
            int health = col.gameObject.GetComponent<Unit>().health--;
            if (health <= 0)
            {
                col.gameObject.GetComponent<Unit>().DestroySelf();
            }
            DestroySelf();
        }
        else if (col.CompareTag("Environment"))
        {
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        spriteRenderer.enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        Destroy(this.gameObject);
    }
}
