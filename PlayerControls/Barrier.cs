using System.Collections;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public bool onCD = false;
    [SerializeField] private GameObject barrierSoundRef;
    [SerializeField] private GameObject barrierEndSoundRef;

    //==================== PUBLIC ====================//
    public void Activate()
    {
        onCD = true;
        gameObject.SetActive(true);
        GameObject sound = Instantiate(barrierSoundRef, transform.position, Quaternion.identity);
        Destroy(sound, sound.GetComponent<AudioSource>().clip.length);
        StartCoroutine(BarrierEnd(
            EquipmentMenu.instance != null && EquipmentMenu.instance.IsEquipped("ArgonBarrier") ? 3f : 1.5f,
            true));
    }

    public void Deactivate(bool playSound)
    {
        gameObject.SetActive(false);
        if (playSound)
        {
            GameObject sound = Instantiate(barrierEndSoundRef, transform.position, Quaternion.identity);
            Destroy(sound, sound.GetComponent<AudioSource>().clip.length);
        }
        Invoke("ResetCD", (EquipmentMenu.instance != null && EquipmentMenu.instance.IsEquipped("BarrierRefresh") ? 7.5f : 15f));
    }

    public void ResetCD()
    {
        CancelInvoke("ResetCD");
        Debug.Log("Reset Barrier CD");
        GetComponent<Barrier>().onCD = false;
        // TODO: Add SFX to indicate when cooldown ends.
    }

    //==================== PRIVATE ====================//
    private IEnumerator BarrierEnd(float delay, bool playSound)
    {
        yield return new WaitForSeconds(delay);
        if (gameObject.activeSelf) Deactivate(playSound);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.activeSelf && EquipmentMenu.instance != null && EquipmentMenu.instance.IsEquipped("BarrierReflect"))
        {
            GameObject source = null;
            if (IsColliderHostileProjectile(other))
            {
                source = other.gameObject.GetComponent<Projectile>().source;
            }
            else if (other.gameObject.CompareTag(EditorConstants.TAG_ENEMY)) source = other.gameObject;
            if (source != null)
            {
                Deactivate(true); // Cooldown still activates in Invoke in Player, but it's not a big issue.
                source.GetComponent<Unit>().TakeDamage(gameObject.transform.parent.gameObject, 1); // TODO: Get amount actually dealt? Unit dmg field?
            }
        }

        if (IsColliderHostileProjectile(other))
        {
            // TODO: Find a way to play projectile death sound before destroying it (add method CreateDeathSound).
            Destroy(other.gameObject);
        }
    }

    private bool IsColliderHostileProjectile(Collider2D other)
    {
        return other.gameObject.CompareTag(EditorConstants.TAG_PROJECTILE) && 
            other.GetComponent<Projectile>().source.CompareTag(EditorConstants.TAG_ENEMY);
    }
}