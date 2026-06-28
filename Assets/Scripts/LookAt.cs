using UnityEngine;

[ExecuteInEditMode]
public class LookAt : MonoBehaviour
{
    public Transform Look;
    public Quaternion rotation;

    void Update()
    {
        if (Look == null) return;

        // Смотрим на цель
        transform.LookAt(Look);
        // Доворачиваем на заданный Quaternion (умножение)
        transform.rotation = transform.rotation * rotation;
    }
}