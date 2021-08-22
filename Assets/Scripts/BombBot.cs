using System.Collections.Generic;
using UnityEngine;

// rolls towards player once spotted
// when spotted: light narrows, points towards player
// animation: when dormant, grey. when active, red
// target = null, when locked on target = player.
  // if player.pos too far away, target = null again

class BombBot : Character {
    private GameObject target;

    void Explode() {
        if (target != null) {
            target.GetComponent<Character>().Kill();
        }
        GetComponent<Character>().Kill();
    }
}