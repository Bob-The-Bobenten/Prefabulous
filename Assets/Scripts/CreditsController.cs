using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject creditsUI; // Drag your UI Canvas or Raw Image here
    public string mainMenuSceneName = "MainMenu";

    void Start()
    {
        // Make sure credits are hidden when the game starts
        if (creditsUI != null) creditsUI.SetActive(false);

        videoPlayer.loopPointReached += OnVideoFinished;
    }

    public void StartCredits()
    {
        if (creditsUI != null) creditsUI.SetActive(true);
        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Explicitly turn off the UI visuals
        if (creditsUI != null) creditsUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoFinished;
    }
}