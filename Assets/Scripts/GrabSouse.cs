using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSouse : Interaction
{
    public GameObject GrabPrefab;
    public float spawnDistance = 2f;

    private GrabManager grabManager;

    private void Start()
    {
        grabManager = FindFirstObjectByType<GrabManager>();
        if (grabManager == null)
            Debug.LogError("GrabManager не найден в сцене!");
    }

    public override void Click()
    {
        Vector3 spawnPos = Camera.main.transform.position + Camera.main.transform.forward * spawnDistance;

        var inst = Instantiate(GrabPrefab, spawnPos, Quaternion.identity);

        // Убеждаемся, что мясо не привязано к иерархии вертела
        inst.transform.SetParent(null);

        // Передаём GrabManager'у, чтобы он сразу схватил этот объект
        grabManager.GrabSpecificObject(inst);

    }
}
