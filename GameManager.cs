using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefsManager.LoadTime();
        StartCoroutine("GameOverCheck", .2f);
        Debug.Log("Application start.");
    }

    private void GameOver()
    {
        if (Clock.instance.GetTimeElapsed() >= 5.99)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private void OnApplicationQuit()
    {
        StorePlayerData();
        Debug.Log("Application terminated.");
    }

    private void StorePlayerData()
    {
        PlayerPrefsManager.StoreTime();
    }

    IEnumerator GameOverCheck(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            GameOver();
        }
    }
}
