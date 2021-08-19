using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class GamePausedPopup : MonoBehaviour {
    public Button resumeButton;
    public Button mainMenuButton;
    public GameObject pauseManager;
    private int sceneidx;

    void Start() {
        resumeButton.onClick.AddListener(Resume);
        mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    void Resume() {
        pauseManager.GetComponent<SceneActions>().Pause();
    }

    void LoadMainMenu() {
        TimeEventManager.Reset();
        SceneManager.LoadScene("MainMenu");
    }
}