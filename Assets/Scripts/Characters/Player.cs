using System.Collections.Generic;
using UnityEngine;

class Player : Character {
    private GameObject environment;

    public void Start() {
        environment = GameObject.FindWithTag("Environment");
    }

    public void Update() {
        if (environment == null) {
            environment = GameObject.FindWithTag("Environment");
        }
    }

    new public void Kill() {
        environment.GetComponent<SceneActions>().Restart();
        base.Kill();
    }
}