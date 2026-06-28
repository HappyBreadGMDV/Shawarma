using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class Localizate : MonoBehaviour
{
    public TextMeshProUGUI ButtonText;          // надпись на кнопке
    public GameObject SelectLan;                // панель выбора языка (необязательно)

    private IEnumerator Start()
    {
        // Ждём, пока система локализации загрузит все локали
        yield return LocalizationSettings.InitializationOperation;

        var locales = LocalizationSettings.AvailableLocales.Locales;
        if (locales.Count == 0)
        {
            Debug.LogError("Нет доступных локалей!");
            yield break;
        }

        // Пытаемся загрузить сохранённый язык
        string savedCode = PlayerPrefs.GetString("Language", "");

        if (!string.IsNullOrEmpty(savedCode))
        {
            // Ищем локаль по коду
            for (int i = 0; i < locales.Count; i++)
            {
                if (locales[i].Identifier.Code == savedCode)
                {
                    LocalizationSettings.SelectedLocale = locales[i];
                    break;
                }
            }
        }
        else
        {
            // Первый запуск – ставим язык по умолчанию (например, русский под индексом 1)
            // и **сразу сохраняем**
            int defaultIndex = Mathf.Clamp(1, 0, locales.Count - 1);
            LocalizationSettings.SelectedLocale = locales[defaultIndex];
            PlayerPrefs.SetString("Language", locales[defaultIndex].Identifier.Code);
            PlayerPrefs.Save();
        }

        UpdateButtonText();
    }

    /// <summary>
    /// Выбор языка по индексу (для кнопок RU/EN)
    /// </summary>
    public void Select(int localeIndex)
    {
        if (localeIndex < 0 || localeIndex >= LocalizationSettings.AvailableLocales.Locales.Count)
            return;

        var locale = LocalizationSettings.AvailableLocales.Locales[localeIndex];
        LocalizationSettings.SelectedLocale = locale;
        PlayerPrefs.SetString("Language", locale.Identifier.Code);
        PlayerPrefs.Save();

        UpdateButtonText();

        if (SelectLan != null)
            SelectLan.SetActive(false);
    }

    /// <summary>
    /// Переключение на следующий язык (опционально)
    /// </summary>
    public void ToggleLanguage()
    {
        var locales = LocalizationSettings.AvailableLocales.Locales;
        if (locales.Count == 0) return;

        int currentIndex = locales.IndexOf(LocalizationSettings.SelectedLocale);
        int newIndex = (currentIndex + 1) % locales.Count;
        Select(newIndex);
    }

    private void UpdateButtonText()
    {
        if (ButtonText == null) return;
        var locale = LocalizationSettings.SelectedLocale;
        if (locale != null)
        {
            ButtonText.text = locale.Identifier.Code.ToUpper();
        }
    }
}