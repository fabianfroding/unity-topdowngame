using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float scrollSensitivity = 2f;
    private float minFOV = 1.75f;
    private float maxFOV = 3.5f;

    private void Update()
    {
        float fov = Camera.main.orthographicSize;
        fov += Input.GetAxis("Mouse ScrollWheel") * -scrollSensitivity;
        fov = Mathf.Clamp(fov, minFOV, maxFOV);
        Camera.main.orthographicSize = fov;
    }
}
