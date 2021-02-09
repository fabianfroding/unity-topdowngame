using UnityEngine;

public class Player : Unit
{
    public GameObject barrier;
    public GameObject novaRef;
    [SerializeField] private GameObject hitSoundRef;

    //==================== PUBLIC ====================//
    public void BarrierStart()
    {
        if (!barrier.GetComponent<Barrier>().onCD) barrier.GetComponent<Barrier>().Activate();
    }

    public void NovaStart()
    {
        GameObject nova = Instantiate(novaRef, transform.position, Quaternion.identity);
        nova.GetComponent<Nova>().Activate(gameObject);
    }

    public override void TakeDamage(GameObject source, int amt)
    {
        if (!invulnerable && !barrier.activeSelf && source.CompareTag("Enemy"))
        {
            invulnerable = true;
            GetComponent<TimeStop>().StopTime(0.1f, 10, 2f);
            GameObject hitSound = Instantiate(hitSoundRef, transform.position, Quaternion.identity);
            Destroy(hitSound, hitSound.GetComponent<AudioSource>().clip.length);
            if (PlayerController.GetState() == PlayerController.State.Dashing)
                PlayerController.SetState(PlayerController.State.Normal);
            base.TakeDamage(source, amt);
            Invoke("ResetInvulnerability", 1f);
        }
        else if (source.CompareTag("Environment"))
        {
            GameObject hitSound = Instantiate(hitSoundRef, transform.position, Quaternion.identity);
            Destroy(hitSound, hitSound.GetComponent<AudioSource>().clip.length);
            if (PlayerController.GetState() == PlayerController.State.Dashing)
                PlayerController.SetState(PlayerController.State.Normal);
            base.TakeDamage(source, 100);
            if (!invulnerable)
            {
                invulnerable = true;
                Invoke("ResetInvulnerability", 1f);
            }
        }
    }

    public override void DestroySelf(float delay = 0f)
    {
        spriteRenderer.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        barrier.GetComponent<Barrier>().Deactivate(true);
        Invoke("Revive", 4f);
    }

    //==================== PRIVATE ====================//
    protected override void Start()
    {
        base.Start();
        health = 4;
    }

    private void ResetInvulnerability()
    {
        invulnerable = false;
    }

    private void Revive()
    {
        transform.position = RevivePoint.revivePoint.transform.position;
        spriteRenderer.enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<CapsuleCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        barrier.GetComponent<Barrier>().ResetCD();
        health = 3;
    }
}