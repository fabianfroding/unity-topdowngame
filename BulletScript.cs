using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public float speed = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Invoke("DestroySelf", 2f);
    }

    public void DestroySelf()
    {
        spriteRenderer.enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        Destroy(this.gameObject);
    }

    public void SetDirection(Vector2 moveDir)
    {
        rb.velocity = new Vector2(moveDir.x * speed, moveDir.y * speed);
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            int health = col.gameObject.GetComponent<Unit>().health--;
            Debug.Log(health);
            if (health <= 0)
            {
                col.gameObject.GetComponent<Unit>().DestroySelf();
            }
            Invoke("DestroySelf", 0f);
        }
        else if (col.CompareTag("Environment"))
        {
            Invoke("DestroySelf", 0f);
        }
    }

}
