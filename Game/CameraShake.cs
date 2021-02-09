using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static float power = 0.7f;
    public static float duration = 1.0f;
    public Transform _camera;
    public static float slowDownAmount = 1.0f;
    public static bool shouldShake = false;

    Vector3 startPosition;
    float initialDuration;

    //==================== PUBLIC ====================//
    public static void StartShake(float pow = 0.7f, float dur = 1.0f, float slowDownAmt = 1.0f)
    {
        power = pow;
        duration = dur;
        slowDownAmount = slowDownAmt;
        shouldShake = true;
    }

    //==================== PRIVATE ====================//
    private void Start()
    {
        _camera = Camera.main.transform;
        startPosition = _camera.localPosition;
        initialDuration = duration;
    }

    private void Update()
    {
        if (shouldShake)
        {
            if (duration > 0)
            {
                _camera.localPosition = startPosition + Random.insideUnitSphere * power;
                duration -= Time.deltaTime * slowDownAmount;
            }
            else
            {
                shouldShake = false;
                duration = initialDuration;
                _camera.localPosition = startPosition;
                // TODO: Reset fields modified in startshake function
            }
        }
    }
}
