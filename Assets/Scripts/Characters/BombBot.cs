using System.Collections.Generic;
using UnityEngine;

// rolls towards player once spotted
// when spotted: light narrows, points towards player
// animation: when dormant, grey. when active, red
// target = null, when locked on target = player.
  // if player.pos too far away, target = null again

class BombBot : Character {
    private GameObject target;
    private Rigidbody2D rb2D;

    public float rollForce;
    public float explodeDistance;

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        Vector3 direction = target.transform.position - transform.position;
        // TODO: if player is visible, bright red light. else dim red light.
        if (direction.magnitude < explodeDistance) {
            Explode();
        }
        //rb2D.AddForce(direction, rollForce);
        
    }

    // need to make animation happen as well
    void Explode() {
        if (target != null) {
            target.GetComponent<Character>().Kill();
        }
        GetComponent<Character>().Kill();
    }
}