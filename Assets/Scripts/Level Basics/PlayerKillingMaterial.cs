using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class PlayerKillingMaterial : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.layer == 3) {
            coll.gameObject.GetComponent<Player>().Kill();
        }
    }

}