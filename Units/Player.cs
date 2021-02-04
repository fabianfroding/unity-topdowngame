using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Unit
{
    [SerializeField] private GameObject hitSoundRef;

    private Sprite defaultSprite;
    private bool isHit = false;

    //==================== PUBLIC ====================//
    public override void TakeDamage(GameObject source, int amount)
    {
        GetComponent<TimeStop>().StopTime(0.1f, 10, 2f);
        if (PlayerController.GetState() == PlayerController.State.Dashing)
            PlayerController.SetState(PlayerController.State.Normal);
        base.TakeDamage(source, amount);
    }

    public override void DestroySelf(float delay = 0f)
    {
        spriteRenderer.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Invoke("Revive", 4f); //Invoke("GameOver", 3f);
    }

    //==================== PRIVATE ====================//
    protected override void Start()
    {
        base.Start();
        health = 4;
        defaultSprite = GetComponent<SpriteRenderer>().sprite;
    }

    private void ResetSpriteAngle() {
        spriteRenderer.sprite = defaultSprite;
    }

    private void GameOver()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Revive()
    {
        transform.position = RevivePoint.revivePoint.transform.position;
        spriteRenderer.enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<CapsuleCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        health = 3;
    }

    private void ResetHit()
    {
        isHit = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isHit && other.gameObject.CompareTag("Enemy"))
        {
            isHit = true;
            Invoke("ResetHit", 1f);
            GameObject hitSound = Instantiate(hitSoundRef, transform.position, Quaternion.identity); // Move to take dmg?
            Destroy(hitSound, hitSound.GetComponent<AudioSource>().clip.length);
            TakeDamage(other.gameObject, 1);
        }
    }

}