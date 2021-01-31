﻿using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Unit
{
    [SerializeField] private TextMeshProUGUI healthTextMesh;
    [SerializeField] private Camera cam;
    [SerializeField] private Sprite[] sprite;
    [SerializeField] private GameObject hitSoundRef;

    private Sprite defaultSprite;
    private bool isHit = false;

    //==================== PUBLIC ====================//
    public override void TakeDamage(GameObject source, int amount)
    {
        GetComponent<TimeStop>().StopTime(0.05f, 10, 2f);
        if (GetComponent<PlayerController2>().GetState() == PlayerController2.State.Dashing)
        {
            GetComponent<PlayerController2>().SetState(PlayerController2.State.Normal);
        }
        base.TakeDamage(source, amount);
    }

    public override void DestroySelf(float delay = 0f)
    {
        spriteRenderer.enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        Invoke("GameOver", 3f);
    }

    public void SetSpriteAngle(Vector3 vec)
    {
        float angle = Vector3.Angle(vec, Vector3.right) * -Vector3.Cross(vec, Vector3.right).normalized.z;
        int spriteIndex = 0;
        if (angle >= 67.5 && angle < 112.5) spriteIndex = 0;
        else if (angle >= 22.5 && angle < 67.5) spriteIndex = 1;
        else if (vec.x == 1 && (angle >= -22.5 && angle < 22.5)) spriteIndex = 2;
        else if (angle >= -67.5 && angle < -22.5) spriteIndex = 3;
        else if (angle >= -112.5 && angle < -67.5) spriteIndex = 4;
        else if (angle >= -157.5 && angle < -112.5) spriteIndex = 5;
        else if (vec.x == -1 && (angle < 22.5 || angle < -157.5)) spriteIndex = 6;
        else if (angle >= 112.5 && angle < 157.5) spriteIndex = 7;
        else ResetSpriteAngle();

        spriteRenderer.sprite = sprite[spriteIndex];
        defaultSprite = sprite[spriteIndex];
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

    private void ResetHit()
    {
        isHit = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isHit && other.gameObject.CompareTag("Enemy"))
        {
            // TODO: Add some knackback effect or something.
            isHit = true;
            Invoke("ResetHit", 1f);
            GameObject hitSound = Instantiate(hitSoundRef, transform.position, Quaternion.identity); // Move to take dmg?
            Destroy(hitSound, hitSound.GetComponent<AudioSource>().clip.length);
            TakeDamage(other.gameObject, 1);
        }
    }

}