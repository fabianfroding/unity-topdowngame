using UnityEngine;

public class NPC : MonoBehaviour
{
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
            if (Input.GetKey(KeyCode.E) && !dialog1.isActive())
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
