using System.Collections.Generic;
using UnityEngine;

class TimeEventManager : MonoBehaviour {
    public delegate void PauseAction();
    public delegate void ReverseAction();
    public static event PauseAction OnPause;
    public static event ReverseAction OnReverse;
    public static bool isPaused;
    public static bool isReversed;

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            Debug.Log("R");
            isReversed = !isReversed;
            if (OnReverse != null) {
                OnReverse();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("P");
            isPaused = !isPaused;
            if (OnPause != null) {
                OnPause();
            }
        }
    }
}