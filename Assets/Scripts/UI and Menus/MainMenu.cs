using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class MainMenu : MonoBehaviour {
    public Button newGame;
    public Button levels;
    public Button quit;
    public GameObject title;
    public float titleLerp;
    public float titleAcc;
    public float accInterval;
    public Image startCover;
    private Transform titleTransform;
    private Vector2 titleAcceleration;
    private Vector2 titleAccelGoal;
    private Vector3 titleStart;
    private float timeSincePush;

    void Start() {
        newGame.onClick.AddListener(StartNewGame);
        levels.onClick.AddListener(LoadLevelScreen);
        quit.onClick.AddListener(QuitGame);
        titleTransform = title.transform;
        titleStart = titleTransform.position;
        timeSincePush = accInterval;
    }

    void FixedUpdate() {
        if (startCover != null) {
            Destroy(startCover);
        }

        if (timeSincePush > accInterval) {
            timeSincePush = 0;
            titleAccelGoal = titleAcceleration + titleAcc*Random.insideUnitCircle;
        }
        else {
            titleAccelGoal = titleAcceleration / 5;
        }
        titleAcceleration = Vector2.Lerp(titleAcceleration, titleAccelGoal, titleLerp * Time.fixedDeltaTime);
        titleTransform.position += Time.fixedDeltaTime * new Vector3(titleAcceleration.x, titleAcceleration.y, 0);
        timeSincePush += Time.fixedDeltaTime;
        titleTransform.position = Vector3.Lerp(titleTransform.position, titleStart, titleLerp * Time.fixedDeltaTime);
    }

    void StartNewGame() {
        Destroy(FindObjectOfType<MainMenuMusic>().gameObject);
        SceneManager.LoadScene("Level0");
    }

    void LoadLevelScreen() {
        SceneManager.LoadScene("LevelSelector");
    }

    void QuitGame() {
        Application.Quit();
    }
}