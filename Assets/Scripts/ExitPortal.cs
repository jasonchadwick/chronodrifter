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

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 3) {
            Destroy(other.gameObject);
            if (sceneidx == SceneManager.sceneCountInBuildSettings - 1) {
                Instantiate(gameEndPrefab, Vector3.zero, Quaternion.identity);
                // say "game complete, return to main menu"
            } else {
                Instantiate(nextLevelPrefab, Vector3.zero, Quaternion.identity);
                // say "return to main menu / next level"
            }
        }
    }
}