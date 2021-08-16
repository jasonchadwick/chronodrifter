using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class MainMenu : MonoBehaviour {
    public Button newGame;
    public Button levels;

    void Start() {
        newGame.onClick.AddListener(StartNewGame);
        levels.onClick.AddListener(LoadLevelScreen);
    }

    void StartNewGame() {
        SceneManager.LoadScene("Level0");
    }

    void LoadLevelScreen() {
        SceneManager.LoadScene("Levels");
    }
}