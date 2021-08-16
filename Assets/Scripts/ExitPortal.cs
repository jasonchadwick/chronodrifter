using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class ExitPortal : MonoBehaviour {
    public GameObject nextLevelPrefab;
    public GameObject gameEndPrefab;
    int sceneidx;

    void Start() {
        sceneidx = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 3) {
            if (sceneidx == SceneManager.sceneCountInBuildSettings - 1) {
                Instantiate(gameEndPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                // say "game complete, return to main menu"
            } else {
                Instantiate(nextLevelPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                // say "return to main menu / next level"
            }
        }
    }
}