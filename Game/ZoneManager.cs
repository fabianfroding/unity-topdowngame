using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneManager : MonoBehaviour
{
    [SerializeField]
    private string zone;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(EditorConstants.TAG_PLAYER))
        {
            SceneManager.LoadScene(zone);
        }
    }
}
