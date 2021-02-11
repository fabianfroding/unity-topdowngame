using UnityEngine;

public class Unit : MonoBehaviour
{
    public int health;
    public float moveSpeed;
    public bool invulnerable = false;

    [SerializeField] protected GameObject hitSoundRef;
    [SerializeField] protected bool facingRight;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;
    protected Animator animator;

    private Material matDefault;
    private Material matWhite;

    //==================== PUBLIC ====================//
    public virtual void TakeDamage(GameObject src, int amt)
    {
        Debug.Log(src.name + " dealt " + amt + " damage to " + name);
        spriteRenderer.material = matWhite;
        Invoke("ResetMaterial", .1f);
        health -= amt;
        if (health <= 0)
        {
            health = 0;
            DestroySelf();
        }

        //StartCoroutine(Knockback(src, 1, 15)); Move somewhere else, all dmg does not knockback.
    }

    public virtual void DestroySelf(float delay = 0f)
    {
        if (delay > 0f) Destroy(gameObject, delay);
        else Destroy(gameObject);
    }

    //==================== PROTECTED ====================//
    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        matDefault = spriteRenderer.material;
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
    }

    //==================== PRIVATE ====================//
    private void ResetMaterial()
    {
        spriteRenderer.material = matDefault;
    }

    /*private IEnumerator Knockback(GameObject src, float duration, float power)
    {
        if (CompareTag("Player") && GetComponent<PlayerController>().IsGrounded())
            power *= 10f;
        else if (CompareTag("Enemy") && rb.gravityScale > 0)
            power *= 100f;

        float timer = 0;
        while (duration > timer)
        {
            timer += Time.deltaTime;
            Vector2 dir = (transform.position - src.transform.position);
            rb.AddForce(dir * power);
        }
        yield return 0;
    }*/
}
