using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    private const string PLAYER_PREFS_TIME = "Time";

    public static void LoadTime()
    {
        Clock.SetTime(PlayerPrefs.GetFloat(PLAYER_PREFS_TIME));
    }

    public static void StoreTime()
    {
        PlayerPrefs.SetFloat(PLAYER_PREFS_TIME, Clock.instance.GetTimeElapsed());
    }
}
