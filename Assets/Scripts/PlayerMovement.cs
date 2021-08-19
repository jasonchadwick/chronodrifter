using System.Collections.Generic;
using UnityEngine;

class PlayerMovement : MonoBehaviour {
    public float jumpStrength = 1.0f;
    public float walkStrength = 1.0f;
    public float maxHorizontalVelocity = 5.0f;
    public float selfRightingLerp = 1.0f;
    Rigidbody2D rb2D;
    private bool isOnGround;

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        Physics2D.queriesHitTriggers = false;
    }

    void Update() {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, Vector3.Dot(transform.up, Vector3.up)), selfRightingLerp*Time.deltaTime);

        if (Input.GetKey(KeyCode.W) && isOnGround && rb2D.velocity.y < jumpStrength/5) {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpStrength);
        }
        if (Input.GetKey(KeyCode.A) && rb2D.velocity.x > -maxHorizontalVelocity) {
            rb2D.AddForce(new Vector2(-walkStrength, 0));
        }
        else if (Input.GetKey(KeyCode.D) && rb2D.velocity.x < maxHorizontalVelocity) {
            rb2D.AddForce(new Vector2(walkStrength, 0));
        }
        else {

        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        isOnGround = true;
    }

    void OnTriggerStay2D(Collider2D other) {
        isOnGround = true;
    }

    void OnTriggerExit2D(Collider2D other) {
        isOnGround = false;
    }
}