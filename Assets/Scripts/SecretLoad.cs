using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class SecretLoad : MonoBehaviour
{
    [Header("LevelLoad")]
    public GameObject loadLevel;
    public GameObject unloadHidden;
    string[] Family = { "Ms.Fire", "Master T", "B.R.O", "Space Explorer", "Life Guard", "Nymph Of Gift" };
    string[] Friends = { "Dildo Boy", "BBL Warrior", "Beast Tamer", "26th Commander", "Coach XL", "Show Conductor" };
    bool isFam;
    bool isFriend;
    void Start()
    {
        if (loadLevel != null)
            loadLevel.gameObject.SetActive(false);
        if (unloadHidden != null)
            unloadHidden.gameObject.SetActive(true);
        string savedClass = SaveManager.instance.currentSave.playerClass;
        for(int i = 0; i < Family.Length; i++)
        {
            if(Family[i] == savedClass)
            {
                isFam = true;
            }
        }
        for (int i = 0; i < Friends.Length; i++)
        {
            if (Friends[i] == savedClass)
            {
                isFriend = true;
            }
        }
        if (loadLevel.gameObject.name != savedClass)
        {
            if(isFam&& loadLevel.gameObject.name== "Family")
            {
                return;
            }
            if (isFriend && loadLevel.gameObject.name == "Friends")
            {
                return;
            }
            Destroy(gameObject);
            Debug.Log(gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (loadLevel != null)
                loadLevel.gameObject.SetActive(true);
            if (unloadHidden != null)
                unloadHidden.gameObject.SetActive(false);
        }
    }
}
