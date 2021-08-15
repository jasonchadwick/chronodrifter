using System.Collections.Generic;
using UnityEngine;

public class TimeReversibleObject : MonoBehaviour {
    private Stack<ObjectSnap2D> objectTimeHistory;
    private bool isReversed;
    private bool isPaused;
    private bool isRigidbody;
    private Rigidbody2D rb2D;
    private bool isKinematic;
    private Vector2 pausedVelocity;
    private float pausedAngularVelocity;


    void Start() {
        if ((rb2D = GetComponent<Rigidbody2D>()) == null) {
            isRigidbody = false;
        }
        else {
            isRigidbody = true;
            isKinematic = rb2D.isKinematic;
        }
        objectTimeHistory = new Stack<ObjectSnap2D>();
        isReversed = false;
        isPaused = false;
    }

    // fixedupdate bc we want this to be framerate independent
    void FixedUpdate() {
        if (isPaused) return;
        if (isReversed) {
            ObjectSnap2D curSnap;
            // move backwards in time list
            if (objectTimeHistory.Count > 1) {
                curSnap = objectTimeHistory.Pop();
            }
            else if (objectTimeHistory.Count == 1) {
                curSnap = objectTimeHistory.Peek();
                isReversed = false;
                Pause();
            }
            else {return;}

            transform.position = curSnap.position;
            transform.eulerAngles = new Vector3(0, 0, curSnap.angle);
            if (isRigidbody) {
                rb2D.velocity = -curSnap.velocity;
                rb2D.angularVelocity = -curSnap.angularVelocity;
            }
        }
        // if time is playing forward, delete existing "future" and make a new one
        else {
            objectTimeHistory.Push(new ObjectSnap2D(transform, rb2D));
        }
    }

     void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            Debug.Log("R");
            isReversed = !isReversed;
            if (isRigidbody) {
                rb2D.velocity *= -1;
                rb2D.angularVelocity *= -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            Debug.Log("P");
            Pause();
        }
    }

    void Pause() {
        if (isRigidbody && !isKinematic) {
            rb2D.isKinematic = !rb2D.isKinematic;
        }
        if (isPaused) {
            pausedVelocity = rb2D.velocity;
            pausedAngularVelocity = rb2D.angularVelocity;
            rb2D.velocity = new Vector2(0, 0);
            rb2D.angularVelocity = 0.0f;
        }
        else {
            rb2D.velocity = pausedVelocity;
            rb2D.angularVelocity = pausedAngularVelocity;
        }
        isPaused = !isPaused;
    }
}

// general object that can be time reversed
// possible optimization: for non-rigidbodies, don't save linear/angular velocity
public class ObjectSnap2D {
    public Vector2 position;

    // third Euler angle (the only one we need for 2D)
    public float angle;
    public Vector2 velocity;
    public float angularVelocity;

    public ObjectSnap2D (Transform transform, Rigidbody2D rb = null) {
        position = transform.position;
        angle = transform.eulerAngles.z;

        if (rb) {
            velocity = rb.velocity;
            angularVelocity = rb.angularVelocity;
        }
    }
}