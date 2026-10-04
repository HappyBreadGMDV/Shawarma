using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Playables;

public class CustomerAI : Interaction
{
    [Serializable]
    public class ShawarmaIngredients
    {
        public GameObject Prefab;
        public LocalizedString name;
    }

    public MoneyManager MoneyManagerScript;
    public int Salary = 50;
    public int Fine = 20;

    [Header("UI")]
    public TextMeshPro OrderText;
    public TypewriterTMP typewriter;
    public GameObject OrderTextPanel;

    [Header("Localized messages")]
    public LocalizedString RepeatMessage;
    public LocalizedString StartMessage;
    public LocalizedString PerfectMessage;
    public LocalizedString UnhappyMessage;

    [Header("Customers Animation")]
    public PlayableDirector EnterAnim;
    public PlayableDirector ExitAnim;

    [Header("Customer model")]
    public Transform ChapterSpawnPoint;
    public List<GameObject> chaptersModels = new List<GameObject>();
    public RuntimeAnimatorController animatorController;
    private GameObject chapterModel;
    private Animator animatorCustomer;
    public string FloatAnimation;
    public float WalkAnim;
    public float IdleAnim;

    [Header("Timings")]
    public float newCustomerDelay = 2f;

    [Header("Ingredients pool")]
    public List<ShawarmaIngredients> shawarmaIngredients = new List<ShawarmaIngredients>();
    public List<ShawarmaIngredients> AiShawarmaIngredients = new List<ShawarmaIngredients>();

    private Coroutine serveCoroutine;

    private TableReference oldNameTable;
    private TableEntryReference oldNameEntry;
    private TableReference oldDescTable;
    private TableEntryReference oldDescEntry;

    private void Start()
    {
        if (Name != null)
        {
            oldNameTable = Name.TableReference;
            oldNameEntry = Name.TableEntryReference;
        }
        if (Description != null)
        {
            oldDescTable = Description.TableReference;
            oldDescEntry = Description.TableEntryReference;
        }
        AIRestart();
    }

    public void Delete()
    {
        AiShawarmaIngredients.Clear();
        Destroy(chapterModel);
        chapterModel = null;
        animatorCustomer = null;
    }

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

        if (chapterModel != null)
            Destroy(chapterModel);

        if (chaptersModels.Count > 0)
        {
            chapterModel = Instantiate(chaptersModels[UnityEngine.Random.Range(0, chaptersModels.Count)], ChapterSpawnPoint);
            animatorCustomer = chapterModel.GetComponent<Animator>();
            if (animatorCustomer != null && animatorController != null)
                animatorCustomer.runtimeAnimatorController = animatorController;
            // Отключаем Root Motion, чтобы Idle не двигал персонажа
            if (animatorCustomer != null)
                animatorCustomer.applyRootMotion = false;
        }

        if (EnterAnim != null)
        {
            EnterAnim.Play();
            // Останавливаем входную анимацию после её завершения, чтобы не зацикливалась
            StartCoroutine(StopEnterAnimAfterDelay((float)EnterAnim.duration));
        }

        StartCoroutine(ShowOrderAfterDelay(1f));
    }

    private IEnumerator StopEnterAnimAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (EnterAnim != null)
            EnterAnim.Stop();
    }
    private IEnumerator ShowOrderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DisplayOrder(StartMessage.GetLocalizedString());
    }

    private void DisplayOrder(string prefix)
    {
        Name = new LocalizedString(oldNameTable, oldNameEntry);
        Description = new LocalizedString(oldDescTable, oldDescEntry);

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

    private void OnTriggerEnter(Collider other) => Check(other);
    private void OnCollisionEnter(Collision collision) => Check(collision.collider);

    public void Check(Collider other)
    {
        if (serveCoroutine != null) return;

        ShwarmaCooked shawaScript = other.GetComponent<ShwarmaCooked>();
        if (shawaScript == null) return;

        // Проверка количества
        if (shawaScript.cookingData.Count != AiShawarmaIngredients.Count)
        {
            Destroy(other.gameObject);
            serveCoroutine = StartCoroutine(HandleMistake());
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
                serveCoroutine = StartCoroutine(HandleMistake());
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
                serveCoroutine = StartCoroutine(HandleMistake());
                return;
            }
        }

        // Всё верно
        Destroy(other.gameObject);
        serveCoroutine = StartCoroutine(HandleSuccess());
    }

    private IEnumerator HandleSuccess()
    {
        // Отключаем коллайдеры клиента, чтобы не обслужить повторно
        if (chapterModel != null)
        {
            foreach (var col in chapterModel.GetComponentsInChildren<Collider>())
                col.enabled = false;
        }

        string perfect = PerfectMessage.GetLocalizedString();
        OrderTextPanel.SetActive(true);
        OrderText.text = perfect;
        typewriter.StartTyping(perfect);

        MoneyManagerScript?.AddMoney(Salary);
        
        Name = new LocalizedString();
        Description = new LocalizedString();

        if (ExitAnim != null)
        {
            ExitAnim.Play();
            yield return new WaitForSeconds((float)ExitAnim.duration + newCustomerDelay);
        }
        else
        {
            yield return new WaitForSeconds(newCustomerDelay);
        }

        AIRestart();
        serveCoroutine = null;
    }

    private IEnumerator HandleMistake()
    {
        if (chapterModel != null)
        {
            foreach (var col in chapterModel.GetComponentsInChildren<Collider>())
                col.enabled = false;
        }

        string message = UnhappyMessage.GetLocalizedString();
        OrderTextPanel.SetActive(true);
        OrderText.text = message;
        typewriter.StartTyping(message);

        MoneyManagerScript?.AddMoney(-Fine);
        
        Name = new LocalizedString();
        Description = new LocalizedString();

        if (ExitAnim != null)
        {
            ExitAnim.Play();
            yield return new WaitForSeconds((float)ExitAnim.duration + newCustomerDelay);
        }
        else
        {
            yield return new WaitForSeconds(newCustomerDelay);
        }

        AIRestart();
        serveCoroutine = null;
    }

    public void PlayAnimation(string name)
    {
        if (animatorCustomer != null)
            if (name == "Walking")
                {
                animatorCustomer.SetFloat(FloatAnimation, WalkAnim);
                Debug.Log(name);
            }
            else
            {
                animatorCustomer.SetFloat(FloatAnimation, IdleAnim);
                Debug.Log(name);
            }
    }

    public void EndAnim()
    {
        DisplayOrder(StartMessage.GetLocalizedString());
    }
}