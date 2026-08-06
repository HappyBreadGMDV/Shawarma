using UnityEngine;

public class GrabSouse : Interaction
{
    public GameObject GrabPrefab;
    public float GrabSubstract = 0.05f;
    public float MaxSouse;
    public float Souse;
    public Transform SouseTransform;
    public Transform SouseStart;
    public Transform SouseEnd;
    public float spawnDistance = 2f;

    private GrabManager grabManager;

    private void Start()
    {
        grabManager = FindFirstObjectByType<GrabManager>();
        if (grabManager == null)
            Debug.LogError("GrabManager не найден в сцене!");

        // Устанавливаем начальную позицию соуса
        UpdateSousePosition();
    }

    void Update()
    {
        // Постоянно обновляем позицию соуса (плавное движение при изменении Souse)
        UpdateSousePosition();
    }

    public override void Click()
    {
        if (Souse < GrabSubstract)
        {
            Debug.Log("Недостаточно соуса");
            return;
        }

        if (Camera.main == null)
        {
            Debug.LogError("Main Camera не найдена!");
            return;
        }

        Vector3 spawnPos = Camera.main.transform.position + Camera.main.transform.forward * spawnDistance;
        var inst = Instantiate(GrabPrefab, spawnPos, Quaternion.identity);

        grabManager.GrabSpecificObject(inst);

        Souse -= GrabSubstract;
        // Позиция обновится автоматически в Update()
    }

    private void UpdateSousePosition()
    {
        if (SouseTransform == null || SouseStart == null || SouseEnd == null)
            return;

        float progress = Mathf.Clamp01(Souse / MaxSouse);
        //SouseTransform.position = Vector3.Lerp(SouseStart.position, SouseEnd.position, progress);
        SouseTransform.localPosition = Vector3.Lerp(SouseStart.localPosition, SouseEnd.localPosition, progress);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.name.Contains(GrabPrefab.name))
    //    {
    //        Destroy(other.gameObject);
    //        Souse += GrabSubstract;
    //        UpdateSousePosition();
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains(GrabPrefab.name))
        {
            Destroy(collision.gameObject);
            Souse += GrabSubstract;
            UpdateSousePosition();
        }
    }
}
