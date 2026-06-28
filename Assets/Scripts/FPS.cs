using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    public TMP_Text displayText;
    public float updateInterval = 0.2f;
    public int targetFps = 60;

    private float timer;
    private float smoothedFps;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFps;
        smoothedFps = 1f / Time.unscaledDeltaTime; // начальное приближение
    }

    private void Update()
    {
        timer += Time.unscaledDeltaTime;

        // Обновляем сглаженное значение FPS каждый кадр (экспоненциальное среднее)
        float instantFps = 1f / Time.unscaledDeltaTime;
        smoothedFps = Mathf.Lerp(smoothedFps, instantFps, 0.1f); // плавность

        if (timer >= updateInterval && displayText != null)
        {
            displayText.text = Mathf.RoundToInt(smoothedFps).ToString();
            timer = 0f;
        }
    }
}