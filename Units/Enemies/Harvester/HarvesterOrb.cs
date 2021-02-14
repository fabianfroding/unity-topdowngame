using UnityEngine;

public class HarvesterOrb : Projectile
{
    [SerializeField] private GameObject orbSoundRef;

    //==================== PROTECTED ====================//
    protected override void Start()
    {
        base.Start();
        GameObject sound = Instantiate(orbSoundRef, transform.position, Quaternion.identity);
        Destroy(sound, sound.GetComponent<AudioSource>().clip.length);
        rb.velocity = new Vector2(source.GetComponent<Unit>().facingRight ? speed : -speed, rb.velocity.y);
        Invoke("DestroySelf", dur);
    }
}
