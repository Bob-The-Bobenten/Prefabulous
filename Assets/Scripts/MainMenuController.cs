using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject classSelectionPanel;
    public TMP_Dropdown classDropdown;

    public void Start()
    {
        mainMenuPanel.SetActive(true);
        classSelectionPanel.SetActive(false);
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

        PlayerClassManager.SelectedClass = chosenClass;

        Debug.Log("Chosen class: " + chosenClass);
    }

    public void OnConfirmClass()
    {
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
