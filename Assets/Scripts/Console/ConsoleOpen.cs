using UnityEngine;

public class ConsoleOpen : MonoBehaviour
{
    public KeyCode keyCode = KeyCode.BackQuote;
    public FirstPersonController PersonController;
    public GameObject Console;

    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            bool consoleActive = !Console.activeSelf;
            Console.SetActive(consoleActive);

            if (PersonController != null)
            {
                PersonController.cameraCanMove = !consoleActive;
                PersonController.playerCanMove = !consoleActive;
                PersonController.enableJump = !consoleActive;
            }

            Cursor.lockState = consoleActive ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = consoleActive;
        }
    }
}