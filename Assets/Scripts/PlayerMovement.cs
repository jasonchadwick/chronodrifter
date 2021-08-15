using System.Collections.Generic;
using UnityEngine;

class PlayerMovement : MonoBehaviour {
    public float jumpStrength = 1.0f;
    public float walkStrength = 1.0f;
    public float maxHorizontalVelocity = 5.0f;
    public float selfRightingLerp = 1.0f;
    Rigidbody2D rb2D;
    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update() {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, Vector3.Dot(transform.up, Vector3.up)), selfRightingLerp*Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.W)) {
            if (rb2D.velocity.y < 1e-5) {
                rb2D.velocity += Vector2.up * jumpStrength;
            }
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
}