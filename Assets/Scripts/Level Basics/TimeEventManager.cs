using System.Collections.Generic;
using UnityEngine;

class TimeEventManager : MonoBehaviour {
    public delegate void PauseAction();
    public delegate void ReverseAction();
    public static event PauseAction OnPause;
    public static event ReverseAction OnReverse;
    public static bool isPaused;
    public static bool isReversed;
    public static float slowFactor;
    public static float curSlowFactor = 1;

    void Start() {
        slowFactor = 10;//float.PositiveInfinity;
        curSlowFactor = 1;
    }

    public static void Reset() {
        OnPause = null;
        OnReverse = null;

        isPaused = false;
        isReversed = false;
        Time.timeScale = 1.0f;
        curSlowFactor = 1;
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
            if (isPaused) {
                curSlowFactor = slowFactor;
            }
            else {
                curSlowFactor = 1;
            }
            if (OnPause != null) {
                OnPause();
            }
        }
    }
}