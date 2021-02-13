using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject source;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    [SerializeField] protected float speed;
    [SerializeField] protected float dur;
    [SerializeField] protected int dmgAmt;

    //==================== PROTECTED ====================//
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(EditorConstants.TAG_ENVIRONMENT)) DestroySelf();
        else if (!other.isTrigger &&
            ((source.CompareTag(EditorConstants.TAG_PLAYER) && other.gameObject.CompareTag(EditorConstants.TAG_ENEMY)) || 
            (source.CompareTag(EditorConstants.TAG_ENEMY) && other.gameObject.CompareTag(EditorConstants.TAG_PLAYER))))
        {
            other.GetComponent<Unit>().TakeDamage(source, dmgAmt);
            DestroySelf();
        }
    }

    protected void DestroySelf()
    {
        Destroy(gameObject);
    }
}
