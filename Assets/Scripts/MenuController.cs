using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject settingsMenu;
    private bool isPaused = false;

    public AudioMixer mainMixer;

    public void OnToggleMenu(InputAction.CallbackContext context)
    {
        Debug.Log("Esc key was pressed!");
        // Only trigger when the button is first pressed (started)
        if (context.started)
        {
            Toggle();
        }
    }

    public void OnSaveAndQuit(InputAction.CallbackContext context)
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
    }

    public void OnReturnToMenu()
    {
            SceneManager.LoadScene("MainMenu");
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
    public void SetMusicVolume(float volume)
    {
        mainMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        mainMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
    }
}