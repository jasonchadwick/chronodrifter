using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class GamePausedPopup : MonoBehaviour {
    public Button resumeButton;
    public Button mainMenuButton;
    public Button restartButton;
    public Button controlsButton;
    public GameObject controlScreenPrefab;
    public GameObject pauseManager;
    private int sceneidx;

    void Start() {
        resumeButton.onClick.AddListener(Resume);
        mainMenuButton.onClick.AddListener(LoadMainMenu);
        restartButton.onClick.AddListener(Restart);
        controlsButton.onClick.AddListener(Controls);
    }

    void Resume() {
        pauseManager.GetComponent<SceneActions>().Pause();
    }

    void Restart() {
        TimeEventManager.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Controls() {
        Instantiate(controlScreenPrefab);
    }

    void LoadMainMenu() {
        TimeEventManager.Reset();
        SceneManager.LoadScene("MainMenu");
    }
}