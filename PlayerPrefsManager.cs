using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public static void LoadTime()
    {
        Clock.SetTime(PlayerPrefs.GetFloat("Time"));
    }

    public static void StoreTime()
    {
        PlayerPrefs.SetFloat("Time", Clock.instance.GetTimeElapsed());
    }
}
