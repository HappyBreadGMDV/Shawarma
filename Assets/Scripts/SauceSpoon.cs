using UnityEngine;

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
        StaticGrab = GameObject.FindGameObjectWithTag("GrabPosStatic")?.transform;
        if (StaticGrab == null)
            Debug.LogError("SauceSpoon: не найден объект с тегом GrabPosStatic!");
    }

    void Update()
    {
        // Если ложка не прикреплена к руке – выключаем подсказку и ничего не делаем
        if (transform.parent != StaticGrab)
        {
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
                if (hit.collider.CompareTag(SauseTag))
                {
                    Debug.Log("Sause");
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

    private void OnDrawGizmosSelected()
    {
        if (Camera.main == null) return;
        Gizmos.color = Color.red;
        Vector3 direction = Camera.main.transform.forward;
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + direction * rayDistance);
    }
}