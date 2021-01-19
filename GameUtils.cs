using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUtils : MonoBehaviour
{
    [SerializeField]
    private GameObject clock;

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
        if (clock.GetComponent<Clock>().GetTimeOfDay() >= 5.98)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
