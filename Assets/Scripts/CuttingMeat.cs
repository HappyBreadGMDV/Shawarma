using Cinemachine;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

public class CuttingMeat : Interaction
{
    public CinemachineVirtualCamera virtualCamera;
    public FirstPersonController firstPersonController;
    public PauseMenu pauseMenu;
    public GameObject UiPodskazka;

    [Header("Cutting Settings")]
    public Transform MeetTransform;
    public float MouseSpeedMaxCutting = 100f;
    public float Meet;
    public float maxMeet = 1f;

    public Transform Look;
    public Transform MeetStartTransform;
    public Transform MeetEndTransform;

    private bool isCutting;
    private Vector3 lastMousePos;

    // ’раним точные ссылки на таблицы и ключи, а не просто копии ссылок
    private TableReference oldNameTable;
    private TableEntryReference oldNameEntry;
    private TableReference oldDescTable;
    private TableEntryReference oldDescEntry;

    private void Start()
    {
        lastMousePos = Input.mousePosition;

        // —охран€ем текущие значени€ локализации
        if (Name != null)
        {
            oldNameTable = Name.TableReference;
            oldNameEntry = Name.TableEntryReference;
        }
        if (Description != null)
        {
            oldDescTable = Description.TableReference;
            oldDescEntry = Description.TableEntryReference;
        }
    }

    public override void Click()
    {
        if (Camera.main)
            virtualCamera.transform.position = Camera.main.transform.position;

        UiPodskazka.SetActive(true);

        firstPersonController.playerCanMove = false;
        firstPersonController.cameraCanMove = false;
        firstPersonController.lockCursor = false;
        Cursor.lockState = CursorLockMode.None;

        virtualCamera.Priority = 20;
        virtualCamera.Follow = Look;
        virtualCamera.LookAt = transform;

        pauseMenu.enabled = false;

        isCutting = true;

        // ќчищаем им€ и описание (показываем пустые строки)
        Name = new LocalizedString();
        Description = new LocalizedString();
    }

    private void Update()
    {
        if (!isCutting) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitCuttingMode();
            UiPodskazka.SetActive(false);
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

        // ¬осстанавливаем оригинальные локализованные строки
        Name = new LocalizedString(oldNameTable, oldNameEntry);
        Description = new LocalizedString(oldDescTable, oldDescEntry);

        pauseMenu.enabled = true;       // включаем обратно меню паузы
        pauseMenu.Resume();             // снимаем паузу (если она была)
    }
}