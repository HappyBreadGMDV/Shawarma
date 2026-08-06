using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public TMP_Text MoneyTmp;
    public int Money;
    public Color positiveColor = Color.white;
    public Color negativeColor = Color.red;
    public float colorFadeDuration = 0.3f;   // длительность перехода цвета

    private NumberAnimation numberAnimation;
    private Tween colorTween;

    private void Start()
    {
        numberAnimation = GetComponent<NumberAnimation>();
        if (numberAnimation == null && MoneyTmp != null)
            numberAnimation = MoneyTmp.GetComponent<NumberAnimation>();

        UpdateTmp();
    }

    /// <summary>Мгновенно обновляет текст (без анимации).</summary>
    public void UpdateTmp()
    {
        MoneyTmp.text = $"{Money}$";
        // Мгновенно возвращаем базовый цвет, убивая активную анимацию цвета
        colorTween?.Kill();
        MoneyTmp.color = positiveColor;
    }

    /// <summary>Изменить деньги и плавно анимировать число и цвет.</summary>
    public void SetMoneySmooth(int newMoney, float duration = -1f)
    {
        int oldMoney = Money;
        Money = newMoney;

        // Запускаем плавный переход цвета
        Color targetColor = (newMoney < 0) ? negativeColor : positiveColor;
        colorTween?.Kill();
        colorTween = MoneyTmp.DOColor(targetColor, colorFadeDuration);

        // Анимация самого числа
        if (numberAnimation != null)
            numberAnimation.SetValue(Money, duration);
        else
            UpdateTmp();
    }

    [Command(typeof(int))]
    /// <summary>Добавить деньги и плавно анимировать.</summary>
    public void AddMoney(int amount, float duration = -1f)
    {
        SetMoneySmooth(Money + amount, duration);
    }

    private void OnDestroy()
    {
        colorTween?.Kill();
    }
}