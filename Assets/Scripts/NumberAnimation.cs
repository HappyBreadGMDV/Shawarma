using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Плавная анимация числа для TextMeshProUGUI.
/// </summary>
public class NumberAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text targetText;
    [SerializeField] private float defaultDuration = 0.5f;
    [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private string numberFormat = "F1";   // "F0" для целых, "F2" для двух знаков

    private Coroutine currentRoutine;
    private float currentDisplayedValue;

    private void Awake()
    {
        if (targetText == null)
            targetText = GetComponent<TMP_Text>();

        // Пытаемся распарсить стартовое значение из текста
        if (targetText != null && float.TryParse(targetText.text, out float startVal))
            currentDisplayedValue = startVal;
    }

    /// <summary>
    /// Запустить анимацию числа до нового значения.
    /// </summary>
    /// <param name="targetValue">Целевое значение.</param>
    /// <param name="duration">Длительность. Если отрицательное – используется defaultDuration.</param>
    public void SetValue(float targetValue, float duration = -1f)
    {
        if (duration < 0f) duration = defaultDuration;

        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(AnimateValue(targetValue, duration));
    }

    private IEnumerator AnimateValue(float target, float duration)
    {
        float startValue = currentDisplayedValue;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float curvedT = curve.Evaluate(t);

            currentDisplayedValue = Mathf.Lerp(startValue, target, curvedT);
            UpdateText();

            yield return null;
        }

        currentDisplayedValue = target;
        UpdateText();
        currentRoutine = null;
    }

    private void UpdateText()
    {
        if (targetText != null)
            targetText.text = currentDisplayedValue.ToString(numberFormat);
    }
}