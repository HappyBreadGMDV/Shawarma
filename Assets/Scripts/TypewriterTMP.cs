using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TypewriterTMP : MonoBehaviour
{
    [Header("Target")]
    public TMP_Text textComponent;   // работает и с TextMeshPro (3D), и с TextMeshProUGUI (UI)

    [Header("Settings")]
    public float charactersPerSecond = 30f;
    public bool playOnStart = true;
    public bool useUnscaledTime = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip typeSound;

    [Header("Events")]
    public UnityEvent onTypeFinished;

    private string fullText;
    private Coroutine typingCoroutine;

    private void Start()
    {
        if (textComponent == null)
            textComponent = GetComponent<TMP_Text>();

        if (playOnStart && textComponent != null && !string.IsNullOrEmpty(textComponent.text))
            StartTyping(textComponent.text);
    }

    /// <summary>
    /// Запустить анимацию печатания. Можно передать новый текст.
    /// </summary>
    public void StartTyping(string newText = null)
    {
        if (textComponent == null)
        {
            Debug.LogWarning("TypewriterTMP: TMP_Text не назначен!");
            return;
        }

        if (newText != null)
            fullText = newText;
        else
            fullText = textComponent.text;

        textComponent.text = "";
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    /// <summary>
    /// Пропустить анимацию и сразу показать весь текст
    /// </summary>
    public void SkipToEnd()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        textComponent.text = fullText;
        onTypeFinished?.Invoke();
    }

    private IEnumerator TypeText()
    {
        float delay = 1f / Mathf.Max(1f, charactersPerSecond);
        bool insideTag = false;
        string currentText = "";
        int length = fullText.Length;

        for (int i = 0; i < length; i++)
        {
            char c = fullText[i];
            currentText += c;

            if (c == '<') insideTag = true;
            if (c == '>') insideTag = false;

            textComponent.text = currentText;

            if (!insideTag && c != '>' && c != '<')
            {
                if (audioSource != null && typeSound != null && c != ' ')
                    audioSource.PlayOneShot(typeSound);

                if (useUnscaledTime)
                    yield return new WaitForSecondsRealtime(delay);
                else
                    yield return new WaitForSeconds(delay);
            }
        }

        textComponent.text = fullText;
        typingCoroutine = null;
        onTypeFinished?.Invoke();
    }

    /// <summary>
    /// Очистить текст мгновенно
    /// </summary>
    public void ClearText()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        textComponent.text = "";
        fullText = "";
    }
}