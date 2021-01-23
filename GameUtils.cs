using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUtils : MonoBehaviour
{
    private void Start()
    {
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
        if (Clock.instance.GetTimeOfDay() >= 5.99)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
