using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class ControlsPopup : MonoBehaviour {
    public Button backButton;

    void Start() {
        backButton.onClick.AddListener(Back);
    }

    void Back() {
        Destroy(gameObject);
    }
}