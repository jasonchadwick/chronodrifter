using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class MainMenu : MonoBehaviour {
    public Button newGame;
    public Button levels;
    public Button quit;

    void Start() {
        newGame.onClick.AddListener(StartNewGame);
        levels.onClick.AddListener(LoadLevelScreen);
        quit.onClick.AddListener(QuitGame);
    }

    void StartNewGame() {
        SceneManager.LoadScene("Level0");
    }

    void LoadLevelScreen() {
        SceneManager.LoadScene("LevelSelector");
    }

    void QuitGame() {
        Application.Quit();
    }
}