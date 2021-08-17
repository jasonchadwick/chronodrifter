using System.Collections.Generic;
using UnityEngine;

class PauseManager : MonoBehaviour {
    public GameObject pauseScreen;
    private GameObject pauseScreenObject;
    private bool paused;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
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
}