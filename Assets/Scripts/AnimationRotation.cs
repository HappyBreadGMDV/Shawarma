using UnityEngine;

public class AnimationRotation : MonoBehaviour
{
    public float speedRotation = 90f;   // градусов в секунду

    public bool xRotation;
    public bool yRotation;
    public bool zRotation;

    void Update()
    {
        // Собираем вектор угловой скорости (градусов в секунду)
        Vector3 rotationVector = Vector3.zero;
        if (xRotation) rotationVector.x = 1f;
        if (yRotation) rotationVector.y = 1f;
        if (zRotation) rotationVector.z = 1f;

        // Вращаем объект в локальных осях
        transform.Rotate(rotationVector * speedRotation * Time.deltaTime, Space.Self);
    }
}