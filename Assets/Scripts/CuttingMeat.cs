using Cinemachine;
using UnityEngine;
using UnityEngine.Localization;

public class CuttingMeat : Interaction
{
    public CinemachineVirtualCamera virtualCamera;
    public FirstPersonController firstPersonController;

    [Header("Cutting Settings")]
    public Transform MeetTransform;
    public float MouseSpeedMaxCutting;
    public float Meet;
    public float maxMeet = 1f;

    public Transform Look;
    public Transform MeetStartTransform;
    public Transform MeetEndTransform;

    private bool isCutting;
    private Vector3 lastMousePos;

    // —юда сохран€ем переведенный текст
    private LocalizedString oldName;
    private LocalizedString oldDescription;

    private void Start()
    {
        lastMousePos = Input.mousePosition;

        // Ѕерем готовый перевод из базовых свойств родител€ Interaction
        oldName = Name;
        oldDescription = Description;

        // »нициализируем текущие строки текста
        Name = oldName;
        Description = oldDescription;
    }

    public override void Click()
    {
        if (Camera.main)
            virtualCamera.transform.position = Camera.main.transform.position;

        firstPersonController.playerCanMove = false;
        firstPersonController.cameraCanMove = false;
        firstPersonController.lockCursor = false;
        Cursor.lockState = CursorLockMode.None;

        virtualCamera.Priority = 20;
        virtualCamera.Follow = Look;
        virtualCamera.LookAt = transform;

        isCutting = true;

        // “еперь это обычные строки, они спокойно очищаютс€
        Name.SetReference("", "");
        Description.SetReference("", "");
    }

    private void Update()
    {
        if (!isCutting) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitCuttingMode();
            return;
        }

        Vector2 mouseDelta = (Vector2)Input.mousePosition - (Vector2)lastMousePos;
        float mouseSpeed = mouseDelta.magnitude / Time.deltaTime;
        lastMousePos = Input.mousePosition;

        if (mouseSpeed >= MouseSpeedMaxCutting)
        {
            Meet = Mathf.Clamp(Meet + 0.1f, 0f, maxMeet);
        }

        float progress = Mathf.Clamp01(Meet / maxMeet);
        MeetTransform.position = Vector3.Lerp(MeetStartTransform.position, MeetEndTransform.position, progress);
    }

    private void ExitCuttingMode()
    {
        firstPersonController.playerCanMove = true;
        firstPersonController.cameraCanMove = true;
        firstPersonController.lockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;

        virtualCamera.Priority = 10;
        virtualCamera.Follow = null;
        virtualCamera.LookAt = null;

        isCutting = false;

        // ¬озвращаем исходный переведенный текст
        Name = oldName;
        Description = oldDescription;
    }
}
