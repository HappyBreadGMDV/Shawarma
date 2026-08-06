using UnityEngine;

public class noclip : MonoBehaviour
{
    public Rigidbody rb;
    public FirstPersonController firstPersonController;
    public Collider playerCollider;
    public float flySpeed = 10f;
    public float mouseSensitivity = 2f;

    private bool isClip = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (isClip)
        {
            // Отключаем стандартный контроллер
            if (firstPersonController != null)
                firstPersonController.enabled = false;

            // Отключаем физику и коллизии
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
            }
            if (playerCollider != null)
                playerCollider.isTrigger = true;

            // --- Поворот камеры ---
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Горизонтальный поворот тела
            transform.Rotate(Vector3.up * mouseX);

            // Вертикальный поворот камеры
            Camera cam = Camera.main;
            if (cam != null)
            {
                float pitch = cam.transform.localEulerAngles.x;
                pitch = (pitch > 180) ? pitch - 360 : pitch; // переводим в диапазон -180..180
                pitch -= mouseY;                             // двигаем мышь вверх = смотрим вверх
                pitch = Mathf.Clamp(pitch, -90f, 90f);      // ограничение
                cam.transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
            }

            // --- Движение ---
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            // Направление в горизонтальной плоскости
            Vector3 moveDir = (transform.right * h + transform.forward * v).normalized;

            // Рассчитываем перемещение
            Vector3 move = moveDir * flySpeed * Time.deltaTime;

            // Вертикальное движение отдельно
            if (Input.GetKey(KeyCode.Space))
                move.y += flySpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftShift))
                move.y -= flySpeed * Time.deltaTime;

            transform.position += move;
        }
    }

    [Command]
    public void Noclip()
    {
        isClip = !isClip;

        if (!isClip)
        {
            if (firstPersonController != null)
                firstPersonController.enabled = true;
            if (rb != null)
                rb.isKinematic = false;
            if (playerCollider != null)
                playerCollider.isTrigger = false;
        }
    }
}