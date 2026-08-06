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
    public GameObject MeetParticle;
    public Transform MeetTransform;
    public Transform MeetModelTransform;
    public float MouseSpeedMaxCutting = 100f;
    public float Meet;
    public float maxMeet = 1f;
    public float rayDistance = 10f;                // дальность луча увеличена

    public Transform Look;
    public Transform MeetStartTransform;
    public Transform MeetEndTransform;

    private bool isCutting;
    private Vector3 lastMousePos;

    // Локализация
    private TableReference oldNameTable;
    private TableEntryReference oldNameEntry;
    private TableReference oldDescTable;
    private TableEntryReference oldDescEntry;

    private void Start()
    {
        lastMousePos = Input.mousePosition;

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

        if (pauseMenu != null)
            pauseMenu.enabled = false;

        isCutting = true;

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

            if (MeetParticle != null && MeetModelTransform != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
                {
                    // Проверяем, что корневой объект попавшего коллайдера совпадает с корнем модели мяса
                    if (hit.collider.transform.root == MeetModelTransform.root)
                    {
                        GameObject pr = Instantiate(MeetParticle, hit.point, MeetParticle.transform.rotation);
                        if (pr.TryGetComponent<ParticleSystem>(out var ps))
                        {
                            Destroy(pr, ps.main.duration);
                            Debug.Log("Particle Spawned");
                        }
                        else
                        {
                            Destroy(pr, 2f);
                        }
                    }
                    else
                    {
                        Debug.Log($"Попадание в {hit.collider.name} (root: {hit.collider.transform.root.name}), ожидался root: {MeetModelTransform.root.name}");
                    }
                }
                else
                {
                    Debug.Log("Луч не попал в объект");
                }
            }
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

        Name = new LocalizedString(oldNameTable, oldNameEntry);
        Description = new LocalizedString(oldDescTable, oldDescEntry);

        if (pauseMenu != null)
        {
            pauseMenu.enabled = true;
            pauseMenu.Resume();
        }
    }
}