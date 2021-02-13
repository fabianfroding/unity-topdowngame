using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUtils : MonoBehaviour
{
    public static GameUtils instance;

    private void Start()
    {
        instance = this;
        StartCoroutine("GameOverCheck", .2f);
    }

    IEnumerator GameOverCheck(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            GameOver();
        }
    }

    private void GameOver()
    {
        if (Clock.instance.GetTimeElapsed() >= 5.99)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
