using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretLoad : MonoBehaviour
{
    [Header("LevelLoad")]
    public GameObject loadLevel;
    public GameObject unloadHidden;
    void Start()
    {
        if (loadLevel != null)
            loadLevel.gameObject.SetActive(false);
        if (unloadHidden != null)
            unloadHidden.gameObject.SetActive(true);
        if (loadLevel.gameObject.name != PlayerClassManager.SelectedClass)
        {
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
