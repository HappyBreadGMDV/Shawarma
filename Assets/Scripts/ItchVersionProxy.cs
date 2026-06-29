using System;
using System.Collections;
using TMPro;   // если хочешь выводить UI
using UnityEngine;
using UnityEngine.Networking;

public class ItchUpdateChecker : MonoBehaviour
{
    [Tooltip("URL твоего Google Apps Script")]
    public string scriptUrl = "https://script.google.com/macros/s/AKfycbz_-Ds4fPc9XdVpYecRuiSeG-5JqQ3HbjWjKph17iRGmhE_oukmgb8Um-gJjoB5FdcS0g/exec";

    [Header("UI (опционально)")]
    public GameObject updateAvailablePanel;   // панель с сообщением
    public TMP_Text versionText;              // "Доступна версия X.X.X"

    private void Start()
    {
        StartCoroutine(CheckVersion());
    }

    IEnumerator CheckVersion()
    {
        using (UnityWebRequest req = UnityWebRequest.Get(scriptUrl))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("Не удалось проверить обновления: " + req.error);
                yield break;
            }

            string json = req.downloadHandler.text;
            VersionData data;
            try
            {
                data = JsonUtility.FromJson<VersionData>(json);
            }
            catch (Exception e)
            {
                Debug.LogError("Ошибка парсинга JSON: " + e.Message);
                yield break;
            }

            if (string.IsNullOrEmpty(data.version) || data.version == "0.0.0")
            {
                Debug.Log("Версия с сервера не получена (возможно, нет загрузок).");
                yield break;
            }

            string currentVersion = Application.version;   // берём из Player Settings
            Debug.Log($"Текущая версия: {currentVersion}, доступная: {data.version}");

            if (IsNewerVersion(data.version, currentVersion))
            {
                Debug.Log("Доступна новая версия!");
                if (updateAvailablePanel) updateAvailablePanel.SetActive(true);
                if (versionText) versionText.text += $" {data.version}";
            }
            else
            {
                Debug.Log("У вас последняя версия.");
            }
        }
    }

    // Сравнение строк версий (поддерживает формат 1.2.3)
    private bool IsNewerVersion(string serverVersion, string localVersion)
    {
        string[] sParts = serverVersion.Split('.');
        string[] lParts = localVersion.Split('.');
        for (int i = 0; i < Mathf.Max(sParts.Length, lParts.Length); i++)
        {
            int sNum = i < sParts.Length && int.TryParse(sParts[i], out int s) ? s : 0;
            int lNum = i < lParts.Length && int.TryParse(lParts[i], out int l) ? l : 0;
            if (sNum > lNum) return true;
            if (sNum < lNum) return false;
        }
        return false; // равны
    }

    [Serializable]
    class VersionData
    {
        public string version;
    }
}