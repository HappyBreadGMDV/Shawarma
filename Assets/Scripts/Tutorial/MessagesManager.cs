using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

public class MessagesManager : MonoBehaviour
{
    public GameObject typewriterPanel;
    public TypewriterTMP typewriter;
    public List<Message> Messages = new List<Message>();

    [System.Serializable]
    public class Message
    {
        public string title;
        public bool playOnStart;
        public float messageDelay;          // задержка после полного появления сообщения
        public UnityEvent onTypeFinished;
        public LocalizedString[] messages;
    }

    private bool isPlaying;

    private void Start()
    {
        foreach (var item in Messages)
        {
            if (item.playOnStart)
            {
                PlayMessage(item.title);
            }
        }
    }

    /// <summary>Запускает показ цепочки сообщений с заданным заголовком.</summary>
    public void PlayMessage(string titleMessage)
    {
        if (isPlaying) return;                // не запускаем, пока идёт предыдущая цепочка

        var ms = Messages.Find(m => m.title == titleMessage);
        if (ms == null)
        {
            Debug.LogWarning($"Сообщение с заголовком '{titleMessage}' не найдено.");
            return;
        }

        StartCoroutine(PlayMessagesCoroutine(ms));
    }

    private IEnumerator PlayMessagesCoroutine(Message ms)
    {
        isPlaying = true;
        typewriterPanel.SetActive(true);

        foreach (var localizedStr in ms.messages)
        {
            string text = localizedStr.GetLocalizedString();
            typewriter.StartTyping(text);

            // Ждём, пока закончится печатание
            while (typewriter.typingCoroutine == null)
                yield return null;

            // Дополнительная пауза после показа сообщения
            if (ms.messageDelay > 0f)
                yield return new WaitForSeconds(ms.messageDelay);
        }

        typewriterPanel.SetActive(false);
        isPlaying = false;

        ms.onTypeFinished.Invoke();
    }
}