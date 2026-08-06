using System.Linq;
using UnityEngine;
using static ShwarmaCooking;

public class SauceSpoon : MonoBehaviour
{
    public KeyCode GrabSauseKey;
    public float rayDistance;
    public GameObject SauseModel;
    public GameObject UiPodskazka;
    public string SauseTag;
    public string ShawaTag;

    private Transform StaticGrab;

    private void Start()
    {
        if(UiPodskazka == null)
        {
            UiPodskazka = FindInactiveGameObjectWithTag("SauseSpoonPodsk");
        }
        StaticGrab = GameObject.FindGameObjectWithTag("GrabPosStatic")?.transform;
        if (StaticGrab == null)
            Debug.LogError("SauceSpoon: не найден объект с тегом GrabPosStatic!");
    }

    void Update()
    {
        // Если ложка не прикреплена к руке – выключаем подсказку и ничего не делаем
        if (transform.parent != StaticGrab)
        {
            if (UiPodskazka != null) ;
                UiPodskazka.SetActive(false);
            return;
        }

        // Ложка в руке – показываем подсказку
        UiPodskazka.SetActive(true);

        // Обработка нажатия клавиши
        if (Input.GetKeyDown(GrabSauseKey))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
            {
                if (hit.collider.CompareTag(SauseTag) && SauseModel.active == false)
                {
                    Debug.Log("Sause");
                    var sauseContainer = hit.collider.GetComponent<SauseContainer>();

                    if (sauseContainer.Souse < sauseContainer.GrabSubstract)
                        return;

                    sauseContainer.UpdateSousePosition();

                    if (SauseModel != null)
                        SauseModel.SetActive(true);
                }
                else if (hit.collider.CompareTag(ShawaTag) && SauseModel != null && SauseModel.activeSelf)
                {
                    // Добавляем ингредиент в шаурму
                    ShwarmaCooking shwarma = hit.collider.GetComponent<ShwarmaCooking>();
                    if (shwarma != null)
                        shwarma.AddIngredient(this.gameObject);
                    else
                        Debug.LogWarning("На объекте шаурмы нет компонента ShwarmaCooking!");

                    SauseModel.SetActive(false);
                }
            }
        }
    }

    public static GameObject FindInactiveGameObjectWithTag(string tag)
    {
        // Retrieves all GameObjects including prefabs and inactive ones
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        return allObjects.FirstOrDefault(go =>
            go.hideFlags == HideFlags.None && // Excludes internal Unity editor objects
            //!UnityEditor.AssetDatabase.Contains(go) && // Excludes project prefabs (Editor only)
            go.CompareTag(tag));
    }

    private void OnDrawGizmosSelected()
    {
        if (Camera.main == null) return;
        Gizmos.color = Color.red;
        Vector3 direction = Camera.main.transform.forward;
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + direction * rayDistance);
    }
}