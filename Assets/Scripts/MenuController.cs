using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    public GameObject settingsMenu;
    private bool isPaused = false;


    public void OnToggleMenu(InputAction.CallbackContext context)
    {
        Debug.Log("Esc key was pressed!");
        // Only trigger when the button is first pressed (started)
        if (context.started)
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            settingsMenu.SetActive(true);
            Time.timeScale = 0f; // Freezes game world
            Cursor.lockState = CursorLockMode.None; // Unlocks mouse
            Cursor.visible = true;
        }
        else
        {
            settingsMenu.SetActive(false);
            Time.timeScale = 1f; // Resumes game world
            Cursor.lockState = CursorLockMode.Locked; // Relocks mouse for gameplay
            Cursor.visible = false;
        }
    }
}