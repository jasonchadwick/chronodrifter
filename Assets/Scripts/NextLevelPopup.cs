using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class NextLevelPopup : MonoBehaviour {
    public Button nextLevelButton;
    public Button mainMenuButton;
    private int sceneidx;

    void Start() {
        sceneidx = SceneManager.GetActiveScene().buildIndex;
        if (nextLevelButton != null) {
            nextLevelButton.onClick.AddListener(LoadNextLevel);
        }
        mainMenuButton.onClick.AddListener(LoadMainMenu);
        Debug.Log("start");
    }

    void LoadNextLevel() {
        Debug.Log("nextlevel");
        TimeEventManager.Reset();
        SceneManager.LoadScene(sceneidx + 1);
    }

    void LoadMainMenu() {
        Debug.Log("nextlevel");
        TimeEventManager.Reset();
        SceneManager.LoadScene("MainMenu");
    }
}