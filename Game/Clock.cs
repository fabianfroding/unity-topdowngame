using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public static Clock instance;

    [SerializeField]
    private GameObject dayNightSymbol;
    [SerializeField]
    private Sprite dayImage;
    [SerializeField]
    private Sprite nightImage;

    [SerializeField]
    private GameObject clockDay;
    [SerializeField]
    private Sprite clockImageDay2;
    [SerializeField]
    private Sprite clockImageDay3;

    [SerializeField] private GameObject godsEye;

    private const float SEC_PER_DAY = 60f; //1039.8f;
    private static float day;
    private static int secPassed = 0;

    private bool daySymbol = true;

    private bool day1Passed = false;
    private bool day2Passed = false;

    //==================== PUBLIC ====================//
    public float GetTimeElapsed()
    {
        return day;
    }

    public static void SetTime(float time)
    {
        day = time;
    }

    //==================== PRIVATE ====================//
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(Tick());
        StartCoroutine(Shake(SEC_PER_DAY * 0.75f));

    }

    private void FixedUpdate()
    {
        day += Time.deltaTime / SEC_PER_DAY * (EquipmentMenu.instance != null && EquipmentMenu.instance.IsEquipped("ChronoSeal") ? 0.5f : 1f);
        
        godsEye.transform.localScale = new Vector3(0.33f + day * 0.8f, 0.33f + day * 0.8f, 0);

        SetDayNightSymbol(day);
        SetDaySymbol(day);

        float dayNorm = day % 1f;
        float rot = 180f;

        transform.eulerAngles = new Vector3(0, 0, -dayNorm * rot - 90f);
    }

    private IEnumerator Tick()
    {
        yield return new WaitForSeconds(1f);
        secPassed++;
        Debug.Log("Ticking: " + secPassed);
        StartCoroutine(Tick());
    }

    private IEnumerator Shake(float tick)
    {
        yield return new WaitForSeconds(tick);

        CameraShake.StartShake();
        float shakeFrequency = (SEC_PER_DAY * 3 / (secPassed + 1)) + 2;
        StartCoroutine(Shake(shakeFrequency));
    }

    private void SetDayNightSymbol(float time)
    {
        if (daySymbol && ((time >= 1 && time < 2) || (time >= 3 && time < 4) || (time >= 5)))
        {
            daySymbol = false;
            dayNightSymbol.GetComponent<Image>().sprite = nightImage;
        }
        else if (!daySymbol && ((time < 1) || (time >= 2 && time < 3) || (time >= 4 && time < 5)))
        {
            daySymbol = true;
            dayNightSymbol.GetComponent<Image>().sprite = dayImage;
        }

        dayNightSymbol.GetComponent<Image>().transform.eulerAngles = new Vector3(0, 0, 0f);
    }

    private void SetDaySymbol(float time)
    {
        if (!day1Passed && time > 2)
        {
            day1Passed = true;
            clockDay.GetComponent<Image>().sprite = clockImageDay2;
        }
        else if (!day2Passed && time > 4)
        {
            day2Passed = true;
            clockDay.GetComponent<Image>().sprite = clockImageDay3;
        }

    }

}
