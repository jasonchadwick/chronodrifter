using System.Collections.Generic;
using UnityEngine;

class TimeEventManager : MonoBehaviour {
    public delegate void PauseAction();
    public delegate void ReverseAction();
    public static event PauseAction OnPause;
    public static event ReverseAction OnReverse;
    public static bool isPaused;
    public static bool isReversed;
    public static float slowScale;

    void Start() {
        slowScale = 0.1f;
    }

    // should not be needed. Time reversible objects should remove
    // their functions from the event managers in OnDisable or OnDestroy.
    public static void Reset() {
        OnPause = null;
        OnReverse = null;
        isPaused = false;
        isReversed = false;
        Time.timeScale = 1.0f;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            isReversed = !isReversed;
            if (OnReverse != null) {
                OnReverse();
            }
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            isPaused = !isPaused;
            if (OnPause != null) {
                OnPause();
            }
        }
    }
}