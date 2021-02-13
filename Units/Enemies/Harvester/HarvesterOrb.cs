using UnityEngine;

public class HarvesterOrb : Projectile
{
    //==================== PROTECTED ====================//
    protected override void Start()
    {
        base.Start();
        rb.velocity = new Vector2(source.GetComponent<Unit>().facingRight ? speed : -speed, rb.velocity.y);
        Invoke("DestroySelf", dur);
    }
}
