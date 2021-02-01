using UnityEngine;

public class RevivePoint : MonoBehaviour
{
    public static GameObject revivePoint;

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Revive point updated");
            revivePoint = this.gameObject;
        }
    }
}
