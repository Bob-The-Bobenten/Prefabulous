using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [System.Serializable]
    public class SaveData
    {
        public Vector3 lastSavePos;
        public List<string> collectedFeatherIDs = new List<string>();
        public string playerClass; // NEW: Stores "Warrior", "Mage", etc.
        public int score;
    }

    public SaveData currentSave = new SaveData();
    private string savePath;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keeps this alive during scene changes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        savePath = Application.persistentDataPath + "/playerSave.json";
        LoadGame();
    }

    public void SaveGame(Vector3 checkpointPos)
    {
        currentSave.lastSavePos = checkpointPos;
        string json = JsonUtility.ToJson(currentSave, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved to: " + savePath);
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            currentSave = JsonUtility.FromJson<SaveData>(json);
        }
    }

    public void ClearSaveData()
    {
        // 1. Wipe the data in the code
        currentSave = new SaveData();

        // 2. Delete the physical file
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Save file deleted for New Game.");
        }
    }

    // Call this from your Main Menu buttons!
    public void SetClassAndStart(string chosenClass)
    {
        currentSave.playerClass = chosenClass;
    }
    public void SetScore(int Score)
    {
        currentSave.score = Score;
    }
}