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

    public void SetDirection(Vector2 destination)
    {
        float angle = Mathf.Atan2(destination.y - transform.position.y, destination.x - transform.position.x);
        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        GetComponent<Rigidbody2D>().velocity = new Vector2(dir.x * speed, dir.y * speed);
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if ((source.CompareTag("Player") && col.CompareTag("Enemy")) || 
            (source.CompareTag("Enemy") && col.CompareTag("Player")))
        {
            col.GetComponent<Unit>().TakeDamage(source, 1);
            if (col.CompareTag("Enemy")) col.GetComponent<Enemy>().FaceTarget(source);
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
