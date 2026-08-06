using UnityEngine;

public class Trash : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            return;                     // игрока не удаляем

        Destroy(other.gameObject);      // всё остальное уничтожаем
    }
}