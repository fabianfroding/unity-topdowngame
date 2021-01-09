using UnityEngine;

public class Unit : MonoBehaviour
{
    public int health;
    public Material matDefault;
    public Material matWhite;
    public SpriteRenderer spriteRenderer;
    public bool isFacingLeft;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
