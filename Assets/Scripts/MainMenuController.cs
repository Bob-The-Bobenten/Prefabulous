using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject classSelectionPanel;
    public GameObject continueButton;
    public GameObject mainMenu;
    public GameObject mainMenuNoTitle;
    public Button confirm;
    public TMP_Dropdown classDropdown;


    public void Start()
    {
        string path = Application.persistentDataPath + "/playerSave.json";
        continueButton.SetActive(System.IO.File.Exists(path));
        mainMenuPanel.SetActive(true);
        classSelectionPanel.SetActive(false);
        mainMenu.SetActive(true);
        mainMenuNoTitle.SetActive(false);
    }

    public void OnContinuePressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("PF3");
    }

    public void OnNewGameConfirmed()
    {
        // Clear the old data before starting fresh!
        if (SaveManager.instance != null)
        {
            SaveManager.instance.ClearSaveData();
        }

        // Now proceed to class selection
        mainMenuPanel.SetActive(false);
        classSelectionPanel.SetActive(true);
        mainMenu.SetActive(false);
        mainMenuNoTitle.SetActive(true);

    }

    public void OnStartPressed()
    {
        // Hide main menu, show class selection
        mainMenuPanel.SetActive(false);
        classSelectionPanel.SetActive(true);
    }

    public void ChooseClass(string className)
    {
        string chosenClass = classDropdown.options[classDropdown.value].text;

        if (chosenClass == "Select")
        {
            confirm.interactable = false;
            return;
        }
        PlayerClassManager.SelectedClass = chosenClass;

        Debug.Log("Chosen class: " + chosenClass);

        confirm.interactable = true;
    }

    public void OnConfirmClass()
    {
        string chosenClass = classDropdown.options[classDropdown.value].text;

        SaveManager.instance.currentSave.playerClass = chosenClass;

        SaveManager.instance.SaveGame(Vector2.zero); // Position will be updated on level load

        SceneManager.LoadScene("PF3");
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
