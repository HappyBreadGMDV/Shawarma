//using UnityEngine;

//public class PauseMenu : MonoBehaviour
//{
//    public GameObject pauseMenuUI;
//    public FirstPersonController PlayerController;
//    private bool isPaused = false;

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Escape))
//        {
//            if (isPaused) Resume();
//            else Pause();
//        }
//    }

//    public void Resume()
//    {
//        pauseMenuUI.SetActive(false);
//        PlayerController.cameraCanMove = true;
//        PlayerController.lockCursor = true;
//        Time.timeScale = 1f;
//        isPaused = false;
//        Cursor.lockState = CursorLockMode.Locked;
//    }

//    void Pause()
//    {
//        pauseMenuUI.SetActive(true);
//        PlayerController.cameraCanMove = false;
//        PlayerController.lockCursor = false;
//        Time.timeScale = 0f;
//        isPaused = true;
//        Cursor.lockState = CursorLockMode.None;
//    }
//}

using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public FirstPersonController PlayerController;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        if (PlayerController != null)
        {
            PlayerController.cameraCanMove = true;
            PlayerController.lockCursor = true;
        }

        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // доп. гарантия скрытия курсора
    }

    void Pause()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        if (PlayerController != null)
        {
            PlayerController.cameraCanMove = false;
            PlayerController.lockCursor = false;
        }

        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}