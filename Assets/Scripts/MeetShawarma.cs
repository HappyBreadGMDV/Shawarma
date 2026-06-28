using UnityEngine;

public class MeetShawarma : Interaction
{
    public GameObject meetPrefab;
    public float spawnDistance = 2f;
    public float cuttingMeatSize;
    public CuttingMeat cuttingMeatScript;

    private GrabManager grabManager;
    private Camera mainCamera;

    private void Start()
    {
        grabManager = FindFirstObjectByType<GrabManager>();
        if (grabManager == null)
            Debug.LogError("GrabManager не найден в сцене!");

        mainCamera = Camera.main;
        if (mainCamera == null)
            Debug.LogError("Main Camera не найдена!");
    }

    public override void Click()
    {
        // Быстрая проверка на null, чтобы избежать лишних вычислений
        if (cuttingMeatScript == null || mainCamera == null)
            return;

        if (cuttingMeatScript.Meet < cuttingMeatSize)
            return;

        // Точка спавна перед камерой (используем закешированную камеру)
        Vector3 spawnPos = mainCamera.transform.position + mainCamera.transform.forward * spawnDistance;
        GameObject newMeat = Instantiate(meetPrefab, spawnPos, Quaternion.identity);

        // Передаём GrabManager'у для захвата
        grabManager.GrabSpecificObject(newMeat);

        // Уменьшаем прогресс нарезки, позиция обновится в CuttingMeat.Update()
        cuttingMeatScript.Meet -= cuttingMeatSize;

        float progress = Mathf.Clamp01(cuttingMeatScript.Meet / cuttingMeatScript.maxMeet);
        cuttingMeatScript.MeetTransform.position = Vector3.Lerp(cuttingMeatScript.MeetStartTransform.position, cuttingMeatScript.MeetEndTransform.position, progress);
    }
}