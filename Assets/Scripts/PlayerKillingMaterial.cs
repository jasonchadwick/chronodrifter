using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class PlayerKillingMaterial : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D coll) {
        Debug.Log(coll.gameObject.name);
        if (coll.gameObject.layer == 3) {
            Destroy(coll.gameObject);
            TimeEventManager.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}