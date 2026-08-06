using UnityEngine;
using TMPro;

public class ConsoleListener : MonoBehaviour
{
    public TMP_Text consoleText;          // UI-текст дл€ вывода логов
    public int maxChars = 5000;          // ограничение длины текста

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    public void FixedUpdate()
    {
        Application.logMessageReceived -= HandleLog;
        Application.logMessageReceived += HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (consoleText == null) return;

        // ƒобавл€ем цвет в зависимости от типа сообщени€
        string colorTag;
        switch (type)
        {
            case LogType.Error:
            case LogType.Assert:
            case LogType.Exception:
                colorTag = "<color=red>";
                break;
            case LogType.Warning:
                colorTag = "<color=yellow>";
                break;
            default:
                colorTag = "<color=white>";
                break;
        }

        string entry = $"{colorTag}[{type}] {logString}</color>\n";
        consoleText.text += entry;
    }
}