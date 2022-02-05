using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class SceneActions : MonoBehaviour {
    public GameObject loadScreenPrefab;
    private GameObject loadScreen;
    public Canvas textCanvasPrefab;
    private Canvas textCanvas;
    public float totalLoadTime = 4.0f;
    public float fadeInTime = 1.0f;
    public float fadeOutTime = 1.0f;
    public GameObject pauseScreen;
    public GameObject entrancePortal;
    private GameObject pauseScreenObject;
    private bool paused;
    private bool loading;
    private bool restarting;
    private float timeSinceRestart = 0;

    void Start() {
        loading = true;
        Time.timeScale = 0.00001f;
        loadScreen = Instantiate(loadScreenPrefab, Vector3.zero, Quaternion.identity);
        textCanvas = Instantiate(textCanvasPrefab, Vector3.zero, Quaternion.identity);
        textCanvas.GetComponentInChildren<Text>().text = Utils.formLevelName(SceneManager.GetActiveScene().buildIndex-3, 1, true);
    }

    void Update() {
        if (loading) {
            float curTime = Time.timeSinceLevelLoad / Time.timeScale;
            if (curTime < totalLoadTime) {
                if (curTime < fadeInTime) {
                    textCanvas.GetComponentInChildren<Text>().color = Color.Lerp(new Color(255, 255, 255, 0), Color.white, curTime/fadeInTime);
                }
                if (curTime > totalLoadTime - fadeOutTime) {
                    loadScreen.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.black, new Color(0, 0, 0, 0), 1-(totalLoadTime - curTime)/fadeOutTime);
                    textCanvas.GetComponentInChildren<Text>().color = Color.Lerp(Color.white, new Color(255, 255, 255, 0), 1-(totalLoadTime - curTime)/fadeOutTime);
                }
            }
            else {
                loading = false;
                loadScreen.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                textCanvas.GetComponentInChildren<Text>().color = new Color(0, 0, 0, 0);
                Time.timeScale = 1;
                entrancePortal.GetComponent<EntrancePortal>().Spawn();
            }
        }
        else if (restarting) {
            timeSinceRestart += Time.deltaTime;
            if (timeSinceRestart > 1) {
                TimeEventManager.Reset();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) {
            Pause();
        }
    }

    public void Pause() {
        if (paused) {
            Time.timeScale = 1;
            paused = false;
            Destroy(pauseScreenObject);
        }
        else {
            Time.timeScale = 0;
            paused = true;
            pauseScreenObject = Instantiate(pauseScreen, Vector3.zero, Quaternion.identity);
            pauseScreenObject.GetComponent<GamePausedPopup>().pauseManager = gameObject;
        }
    }

    public void Restart() {
        restarting = true;
    }
}