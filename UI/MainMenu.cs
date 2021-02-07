using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void BTNStart_Click()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Shrine");
    }
}
