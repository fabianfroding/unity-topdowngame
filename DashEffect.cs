using UnityEngine;

public class DashEffect : MonoBehaviour
{
    public static void CreateDashEffect(Vector3 position, Vector3 dir, float dashSize, Transform dashEffect)
    {
        Transform dashTransform = Instantiate(dashEffect, position + new Vector3(0, 1, 0), Quaternion.identity);
        dashTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
        dashTransform.localScale = new Vector3(dashSize * 0.001f, 0.3f, 1);
    }

    private static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
}
