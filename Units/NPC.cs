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
        if (playerInRange && !UIManager.instance.isAnyMenuActive())
        {
            if (Input.GetKey(KeyCodeConstants.KEY_CODE_INTERACT) && !Dialog.isActive() && PlayerController.isIdle())
            {
                Debug.Log("Start dialog with " + this.gameObject.name);
                dialog1.StartDialog();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(EditorConstants.TAG_PLAYER))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(EditorConstants.TAG_PLAYER))
        {
            playerInRange = false;
        }
    }
}
