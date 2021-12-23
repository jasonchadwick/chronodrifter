using System.Collections.Generic;
using UnityEngine;

// general class for player and enemies
class Character : MonoBehaviour {


    public void Kill() {
        Destroy(gameObject);
    }
}