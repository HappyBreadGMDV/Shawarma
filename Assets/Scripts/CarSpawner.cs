using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{

    public float CarDelayMax = 5f;
    public float CarDelayMin = 2f;

    public float CarSpeedMax = 5f;
    public float CarSpeedMin = 2f;

    public Transform StartPosition;
    public Transform EndPosition;

    public List<GameObject> Cars = new List<GameObject>();

    private float nextSpawnTime;
    private float timer;

    private void Start()
    {
        // Первый интервал – случайный
        SetRandomDelay();
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer >= nextSpawnTime)
        {
            SpawnCar();
            SetRandomDelay();   // новый случайный интервал
            timer = 0f;
        }
    }

    void SpawnCar()
    {
        if (Cars.Count == 0) return;

        int index = UnityEngine.Random.Range(0, Cars.Count);
        GameObject carModel = Cars[index];

        // Спавним в мире без родителя
        GameObject newCar = Instantiate(carModel, transform);

        newCar.transform.position = StartPosition.position;

        // Запускаем движение через DOTween
        newCar.transform.DOMove(EndPosition.position, UnityEngine.Random.Range(CarSpeedMin, CarSpeedMax))
            .SetEase(Ease.Linear)         // плавное равномерное движение
            .OnComplete(() => Destroy(newCar.gameObject)); // удалить после прибытия (опционально)
    }

    void SetRandomDelay()
    {
        nextSpawnTime = UnityEngine.Random.Range(CarDelayMin, CarDelayMax);
    }
}