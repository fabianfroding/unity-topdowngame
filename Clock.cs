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

    private const float SEC_PER_DAY = 10f;
    private Transform transform;
    private float day;

    private bool daySymbol = true;

    private bool day1Passed = false;
    private bool day2Passed = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        transform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        day += Time.deltaTime / SEC_PER_DAY;

        SetDayNightSymbol(day);
        SetDaySymbol(day);

        float dayNorm = day % 1f;
        float rot = 180f;

        transform.eulerAngles = new Vector3(0, 0, -dayNorm * rot - 90f);
    }

    public float GetTimeOfDay()
    {
        return day;
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
