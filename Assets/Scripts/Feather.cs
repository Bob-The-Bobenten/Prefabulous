using UnityEngine;

public class Feather : MonoBehaviour
{
    public string featherID;

    void Start()
    {
        // Use the instance we already have in SaveManager
        if (SaveManager.instance.currentSave.collectedFeatherIDs.Contains(featherID))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    { // Changed to 2D
        if (other.CompareTag("Player"))
        {
            if (!SaveManager.instance.currentSave.collectedFeatherIDs.Contains(featherID))
            {
                SaveManager.instance.currentSave.collectedFeatherIDs.Add(featherID);
            }
            SaveManager.instance.SaveGame(other.transform.position);
            gameObject.SetActive(false);
        }
    }
}