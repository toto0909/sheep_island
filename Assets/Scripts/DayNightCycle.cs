using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light directionalLight;

    void Update()
    {
        if (directionalLight == null || GameManager.Instance == null) return;

        float secondsPerDay = GameManager.Instance.secondsPerDay;
        if (secondsPerDay <= 0f) return;

        float timeOfDay = GameManager.Instance.GetGameTimeInSeconds() % secondsPerDay;
        float normalizedTime = timeOfDay / secondsPerDay;

        if (float.IsNaN(normalizedTime) || float.IsInfinity(normalizedTime)) return;

        // 🌞 太陽の角度：一日で360度回転（-90度 = 夜明け）
        float sunAngle = normalizedTime * 360f - 90f;
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3(sunAngle, 170f, 0f));

        // 🌗 光の強さと色
        Color dayColor = Color.white;
        Color nightColor = new Color(0.1f, 0.1f, 0.35f);
        float lightFactor = Mathf.Clamp01(Mathf.Sin(normalizedTime * Mathf.PI));

        directionalLight.color = Color.Lerp(nightColor, dayColor, lightFactor);
        directionalLight.intensity = Mathf.Lerp(0.1f, 1.2f, lightFactor);
    }
}
