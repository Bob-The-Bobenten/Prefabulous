using Cinemachine;
using UnityEngine;

public class CameraSwitchTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera newCam;
    public CinemachineVirtualCamera mainCam;

    [Header("LevelLoad")]
    public GameObject loadLevel1;
    public GameObject loadLevel2;
    public GameObject loadLevel3;
    public GameObject unloadLevel1;
    public GameObject unloadLevel2;
    public GameObject unloadLevel3;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            newCam.Priority = 20;
            mainCam.Priority = 5;
            /*if (loadLevel1!=null)
                loadLevel1.gameObject.SetActive(true);
            if (loadLevel2 != null)
                loadLevel2.gameObject.SetActive(true);
            if (loadLevel3 != null)
                loadLevel3.gameObject.SetActive(true);
            if (unloadLevel1 != null)
                unloadLevel1.gameObject.SetActive(false);
            if (unloadLevel2 != null)
                unloadLevel2.gameObject.SetActive(false);
            if (unloadLevel3 != null)
                unloadLevel3.gameObject.SetActive(false);*/
        }

    }
}
