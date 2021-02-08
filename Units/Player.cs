using System.Collections;
using UnityEngine;

public class Player : Unit
{
    public bool barrierOnCD = false;

    [SerializeField] private GameObject barrier;
    [SerializeField] private GameObject hitSoundRef;
    [SerializeField] private GameObject barrierSoundRef;
    [SerializeField] private GameObject barrierEndSoundRef;

    //==================== PUBLIC ====================//
    public void BarrierStart()
    {
        barrierOnCD = true;
        barrier.SetActive(true);
        GameObject sound = Instantiate(barrierSoundRef, barrier.transform.position, Quaternion.identity);
        Destroy(sound, sound.GetComponent<AudioSource>().clip.length);
        StartCoroutine(BarrierEnd(EquipmentMenu.instance.IsEquipped("ArgonBarrier") ? 3f : 1.5f, true));
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
        StopCoroutine(BarrierEnd(0f, false)); BarrierEnd(0f, false);
        Invoke("Revive", 4f);
    }

    //==================== PRIVATE ====================//
    protected override void Start()
    {
        base.Start();
        health = 4;
    }

    private IEnumerator BarrierEnd(float delay, bool playSound)
    {
        yield return new WaitForSeconds(delay);
        barrier.SetActive(false);
        if (playSound)
        {
            GameObject sound = Instantiate(barrierEndSoundRef, barrier.transform.position, Quaternion.identity);
            Destroy(sound, sound.GetComponent<AudioSource>().clip.length);
        }
        Invoke("ResetBarrierCD", (EquipmentMenu.instance != null && EquipmentMenu.instance.IsEquipped("BarrierRefresh") ? 7.5f : 15f));
    }

    private void ResetBarrierCD()
    {
        Debug.Log("Reset Barrier");
        barrierOnCD = false;
        // TODO: Add SFX to indicate when cooldown ends.
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
        CancelInvoke("ResetBarrierCD"); ResetBarrierCD();
        health = 3;
    }
}