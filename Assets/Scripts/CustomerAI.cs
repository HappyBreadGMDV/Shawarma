//using System;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.Localization;

//public class CustomerAI : Interaction
//{
//    [Serializable]
//    public class ShawarmaIngredients
//    {
//        public GameObject Prefab;
//        public LocalizedString name;
//    }

//    [Header("UI")]
//    public TextMeshPro OrderText;
//    public TypewriterTMP typewriter;
//    public GameObject OrderTextPanel;

//    [Header("Localized messages")]
//    public LocalizedString RepeatMessage;
//    public LocalizedString StartMessage;
//    public LocalizedString PerfectMessage;
//    public LocalizedString UnhappyMessage;   // единое недовольство

//    [Header("Customer model")]
//    public Transform ChapterSpawnPoint;
//    public List<GameObject> chaptersModels = new List<GameObject>();
//    private GameObject chapterModel;

//    [Header("Ingredients pool")]
//    public List<ShawarmaIngredients> shawarmaIngredients = new List<ShawarmaIngredients>();
//    public List<ShawarmaIngredients> AiShawarmaIngredients = new List<ShawarmaIngredients>();

//    private Coroutine restoreCoroutine;

//    public override void Click()
//    {
//        string text = RepeatMessage.GetLocalizedString();
//        for (int i = 0; i < AiShawarmaIngredients.Count; i++)
//        {
//            text += AiShawarmaIngredients[i].name.GetLocalizedString();
//            if (i < AiShawarmaIngredients.Count - 1)
//                text += ", ";
//        }
//        text += "                 ";

//        OrderTextPanel.SetActive(true);
//        OrderText.text = text;
//        typewriter.StartTyping(text);
//    }

//    public void AIRestart()
//    {
//        if (shawarmaIngredients.Count == 0)
//        {
//            OrderText.text = "Ингредиентов нет :(";
//            return;
//        }

//        AiShawarmaIngredients.Clear();
//        int count = UnityEngine.Random.Range(1, shawarmaIngredients.Count + 1);
//        List<ShawarmaIngredients> shuffled = new List<ShawarmaIngredients>(shawarmaIngredients);
//        for (int i = 0; i < shuffled.Count; i++)
//        {
//            int j = UnityEngine.Random.Range(i, shuffled.Count);
//            var temp = shuffled[i];
//            shuffled[i] = shuffled[j];
//            shuffled[j] = temp;
//        }
//        for (int i = 0; i < count; i++)
//            AiShawarmaIngredients.Add(shuffled[i]);

//        DisplayOrder(StartMessage.GetLocalizedString());

//        if (chaptersModels.Count > 0)
//            chapterModel = Instantiate(chaptersModels[UnityEngine.Random.Range(0, chaptersModels.Count)], ChapterSpawnPoint);
//    }

//    private void DisplayOrder(string prefix)
//    {
//        if (AiShawarmaIngredients.Count == 0) return;

//        string text = prefix;
//        for (int i = 0; i < AiShawarmaIngredients.Count; i++)
//        {
//            text += AiShawarmaIngredients[i].name.GetLocalizedString();
//            if (i < AiShawarmaIngredients.Count - 1)
//                text += ", ";
//        }
//        text += "                 ";

//        OrderTextPanel.SetActive(true);
//        OrderText.text = text;
//        typewriter.StartTyping(text);
//    }

//    private void Start()
//    {
//        AIRestart();
//    }

//    private void OnTriggerEnter(Collider other) => Check(other);
//    private void OnCollisionEnter(Collision collision) => Check(collision.collider);

//    public void Check(Collider other)
//    {
//        ShwarmaCooked shawaScript = other.GetComponent<ShwarmaCooked>();
//        if (shawaScript == null) return;

//        // Проверка количества
//        if (shawaScript.cookingData.Count != AiShawarmaIngredients.Count)
//        {
//            Destroy(other.gameObject);
//            HandleMistake();
//            return;
//        }

//        // Проверка на лишние
//        foreach (var cookedItem in shawaScript.cookingData)
//        {
//            bool found = false;
//            foreach (var required in AiShawarmaIngredients)
//            {
//                if (cookedItem.Object != null && required.Prefab != null &&
//                    cookedItem.Object.name == required.Prefab.name)
//                {
//                    found = true;
//                    break;
//                }
//            }
//            if (!found)
//            {
//                Destroy(other.gameObject);
//                HandleMistake();
//                return;
//            }
//        }

//        // Проверка на нехватку
//        foreach (var required in AiShawarmaIngredients)
//        {
//            bool found = false;
//            foreach (var cookedItem in shawaScript.cookingData)
//            {
//                if (cookedItem.Object != null && required.Prefab != null &&
//                    cookedItem.Object.name == required.Prefab.name)
//                {
//                    found = true;
//                    break;
//                }
//            }
//            if (!found)
//            {
//                Destroy(other.gameObject);
//                HandleMistake();
//                return;
//            }
//        }

//        // Всё верно
//        Destroy(other.gameObject);
//        Destroy(chapterModel);
//        if (restoreCoroutine != null) StopCoroutine(restoreCoroutine);

//        string perfect = PerfectMessage.GetLocalizedString();
//        OrderTextPanel.SetActive(true);
//        OrderText.text = perfect;
//        typewriter.StartTyping(perfect);

//        Invoke(nameof(AIRestart), 5.0f);
//    }

//    private void HandleMistake()
//    {
//        if (restoreCoroutine != null) StopCoroutine(restoreCoroutine);

//        string message = UnhappyMessage.GetLocalizedString();
//        OrderTextPanel.SetActive(true);
//        OrderText.text = message;
//        typewriter.StartTyping(message);

//        restoreCoroutine = StartCoroutine(RestoreOrderAfterDelay(3f));
//    }

//    private IEnumerator RestoreOrderAfterDelay(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        DisplayOrder(StartMessage.GetLocalizedString());
//    }
//}


using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class CustomerAI : Interaction
{
    [Serializable]
    public class ShawarmaIngredients
    {
        public GameObject Prefab;
        public LocalizedString name;
    }

    [Header("UI")]
    public TextMeshPro OrderText;
    public TypewriterTMP typewriter;
    public GameObject OrderTextPanel;

    [Header("Localized messages")]
    public LocalizedString RepeatMessage;
    public LocalizedString StartMessage;
    public LocalizedString PerfectMessage;
    public LocalizedString UnhappyMessage;

    [Header("Customer model")]
    public Transform ChapterSpawnPoint;
    public List<GameObject> chaptersModels = new List<GameObject>();
    private GameObject chapterModel;

    [Header("Timings")]
    public float newCustomerDelay = 5f;   // задержка перед появлением следующего клиента

    [Header("Ingredients pool")]
    public List<ShawarmaIngredients> shawarmaIngredients = new List<ShawarmaIngredients>();
    public List<ShawarmaIngredients> AiShawarmaIngredients = new List<ShawarmaIngredients>();

    private Coroutine restoreCoroutine;

    public override void Click()
    {
        string text = RepeatMessage.GetLocalizedString();
        for (int i = 0; i < AiShawarmaIngredients.Count; i++)
        {
            text += AiShawarmaIngredients[i].name.GetLocalizedString();
            if (i < AiShawarmaIngredients.Count - 1)
                text += ", ";
        }
        text += "                 ";

        OrderTextPanel.SetActive(true);
        OrderText.text = text;
        typewriter.StartTyping(text);
    }

    public void AIRestart()
    {
        if (shawarmaIngredients.Count == 0)
        {
            OrderText.text = "Ингредиентов нет :(";
            return;
        }

        AiShawarmaIngredients.Clear();
        int count = UnityEngine.Random.Range(1, shawarmaIngredients.Count + 1);
        List<ShawarmaIngredients> shuffled = new List<ShawarmaIngredients>(shawarmaIngredients);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, shuffled.Count);
            var temp = shuffled[i];
            shuffled[i] = shuffled[j];
            shuffled[j] = temp;
        }
        for (int i = 0; i < count; i++)
            AiShawarmaIngredients.Add(shuffled[i]);

        DisplayOrder(StartMessage.GetLocalizedString());

        // Создаём модель нового клиента
        if (chaptersModels.Count > 0)
            chapterModel = Instantiate(chaptersModels[UnityEngine.Random.Range(0, chaptersModels.Count)], ChapterSpawnPoint);
    }

    private void DisplayOrder(string prefix)
    {
        if (AiShawarmaIngredients.Count == 0) return;

        string text = prefix;
        for (int i = 0; i < AiShawarmaIngredients.Count; i++)
        {
            text += AiShawarmaIngredients[i].name.GetLocalizedString();
            if (i < AiShawarmaIngredients.Count - 1)
                text += ", ";
        }
        text += "                 ";

        OrderTextPanel.SetActive(true);
        OrderText.text = text;
        typewriter.StartTyping(text);
    }

    private void Start()
    {
        AIRestart();
    }

    private void OnTriggerEnter(Collider other) => Check(other);
    private void OnCollisionEnter(Collision collision) => Check(collision.collider);

    public void Check(Collider other)
    {
        ShwarmaCooked shawaScript = other.GetComponent<ShwarmaCooked>();
        if (shawaScript == null) return;

        // Проверка количества
        if (shawaScript.cookingData.Count != AiShawarmaIngredients.Count)
        {
            Destroy(other.gameObject);
            HandleMistake();
            return;
        }

        // Проверка на лишние
        foreach (var cookedItem in shawaScript.cookingData)
        {
            bool found = false;
            foreach (var required in AiShawarmaIngredients)
            {
                if (cookedItem.Object != null && required.Prefab != null &&
                    cookedItem.Object.name == required.Prefab.name)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                Destroy(other.gameObject);
                HandleMistake();
                return;
            }
        }

        // Проверка на нехватку
        foreach (var required in AiShawarmaIngredients)
        {
            bool found = false;
            foreach (var cookedItem in shawaScript.cookingData)
            {
                if (cookedItem.Object != null && required.Prefab != null &&
                    cookedItem.Object.name == required.Prefab.name)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                Destroy(other.gameObject);
                HandleMistake();
                return;
            }
        }

        // Всё верно — клиент доволен
        Destroy(other.gameObject);
        Destroy(chapterModel);
        if (restoreCoroutine != null) StopCoroutine(restoreCoroutine);

        string perfect = PerfectMessage.GetLocalizedString();
        OrderTextPanel.SetActive(true);
        OrderText.text = perfect;
        typewriter.StartTyping(perfect);

        // Следующий клиент через задержку
        Invoke(nameof(AIRestart), newCustomerDelay);
    }

    private void HandleMistake()
    {
        // Удаляем модель текущего недовольного клиента
        if (chapterModel != null)
        {
            Destroy(chapterModel);
            chapterModel = null;
        }

        if (restoreCoroutine != null) StopCoroutine(restoreCoroutine);

        string message = UnhappyMessage.GetLocalizedString();
        OrderTextPanel.SetActive(true);
        OrderText.text = message;
        typewriter.StartTyping(message);

        // Ждём и запускаем нового клиента
        Invoke(nameof(AIRestart), newCustomerDelay);
    }

    private IEnumerator WaitAndSpawnNewCustomer()
    {
        yield return new WaitForSeconds(newCustomerDelay);
        AIRestart();
    }
}