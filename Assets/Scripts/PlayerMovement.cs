using System.Collections.Generic;
using UnityEngine;

class PlayerMovement : Character {
    public float jumpStrength = 1.0f;
    public float walkStrength = 1.0f;
    public float maxHorizontalVelocity = 5.0f;
    public float selfRightingLerp = 1.0f;
    public float horizontalDampingLerp = 150.0f;
    Rigidbody2D rb2D;
    private bool isOnGround;

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        Physics2D.queriesHitTriggers = false;
    }

    void Update() {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, Vector3.Dot(transform.up, Vector3.up)), selfRightingLerp*Time.deltaTime);

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && isOnGround && rb2D.velocity.y < jumpStrength/5) {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpStrength);
        }
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && rb2D.velocity.x > -maxHorizontalVelocity) {
            rb2D.AddForce(new Vector2(-walkStrength, 0));
        }
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && rb2D.velocity.x < maxHorizontalVelocity) {
            rb2D.AddForce(new Vector2(walkStrength, 0));
        }
    }

    void FixedUpdate() {
        if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))) {
            rb2D.velocity = Vector2.Lerp(rb2D.velocity, new Vector2(0, rb2D.velocity.y), horizontalDampingLerp * Time.fixedDeltaTime);
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