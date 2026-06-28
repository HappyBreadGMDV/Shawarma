using UnityEngine;

public class SauceSpoon : MonoBehaviour
{
    public KeyCode GrabSauseKey;
    public float rayDistance;
    public GameObject SauseModel;
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
        if (Input.GetKeyDown(GrabSauseKey))
        {
            // Ложка должна быть прикреплена к руке
            if (transform.parent != StaticGrab)
                return;

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
                    hit.collider.GetComponent<ShwarmaCooking>().AddIngredient(this.gameObject);
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