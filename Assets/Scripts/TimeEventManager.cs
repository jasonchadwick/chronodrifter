using System.Collections.Generic;
using UnityEngine;

class TimeEventManager : MonoBehaviour {
    public delegate void PauseAction();
    public delegate void ReverseAction();
    public static event PauseAction OnPause;
    public static event ReverseAction OnReverse;
    public static bool isPaused;
    public static bool isReversed;

    public static void Reset() {
        OnPause = null;
        OnReverse = null;
        isPaused = false;
        isReversed = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            isReversed = !isReversed;
            if (OnReverse != null) {
                OnReverse();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            isPaused = !isPaused;
            if (OnPause != null) {
                OnPause();
            }
        }
    }
}