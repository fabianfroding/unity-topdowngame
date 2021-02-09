using UnityEngine;

public class NPC : MonoBehaviour
{
    private const KeyCode KEY_INTERACT = KeyCode.W;
    private bool playerInRange;
    private Dialog dialog1;

    private void Start()
    {
        dialog1 = GetComponent<Dialog>();
    }

    private void FixedUpdate()
    {
        if (playerInRange)
        {
            if (Input.GetKey(KEY_INTERACT) && !dialog1.isActive() && PlayerController.isIdle())
            {
                Debug.Log("Start dialog with " + this.gameObject.name);
                dialog1.StartDialog();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
