using System.Collections.Generic;
using UnityEngine;

// TODO: make it just use a general object for the stack. How to restore values?
// Maybe use a dictionary? And at the end reset all values?
// Maybe the inheriting class provides its own "restore" function?
public class TimeReversibleObject : MonoBehaviour {
    private Stack<ObjectSnap2D> objectTimeHistory;
    private bool isRigidbody;
    private Rigidbody2D rb2D;
    private bool isKinematic;
    private Vector2 pausedVelocity;
    private float pausedAngularVelocity;
    private bool pausedReverse;

    // used for slowed-down time
    private ObjectSnap2D oldObjectSnap;
    private ObjectSnap2D goalObjectSnap;
    private float progressToNextSnap;

    public bool useHistoryStack = true;
    public bool restoringForce = true;
    public float restoringLerp = 10.0f;
    public float similarityThreshold = 5e-2f;
    public bool isFullyReversed = false;


    void Start() {
        if ((rb2D = GetComponent<Rigidbody2D>()) == null) {
            isRigidbody = false;
        }
        else {
            isRigidbody = true;
            isKinematic = rb2D.isKinematic;
        }
        objectTimeHistory = new Stack<ObjectSnap2D>();

        TimeEventManager.OnPause += UpdateOnPause;
        TimeEventManager.OnReverse += UpdateOnReverse;
    }

    // fixedupdate bc we want this to be framerate independent
    void FixedUpdate() {
        if (TimeEventManager.isPaused) {
            progressToNextSnap += Time.fixedDeltaTime * TimeEventManager.slowScale;
            if (progressToNextSnap > Time.fixedDeltaTime) {
                progressToNextSnap = 0;
                if (TimeEventManager.isReversed) {
                    
                }
            }
            if (TimeEventManager.isReversed) {

            }
        }
        else {
            if (TimeEventManager.isReversed) {
                ObjectSnap2D curSnap;
                // move backwards in time list
                if (objectTimeHistory.Count > 1) {
                    curSnap = objectTimeHistory.Peek();
                    if (curSnap.numIntervals <= 1) {
                        objectTimeHistory.Pop();
                    }
                    isFullyReversed = false;
                }
                else if (objectTimeHistory.Count == 1) {
                    curSnap = objectTimeHistory.Peek();
                    isFullyReversed = true;
                }
                else {return;}

                if (restoringForce && Vector2.Distance(transform.position, curSnap.position) < 1) {
                    transform.position = Vector3.Lerp(transform.position, curSnap.position, restoringLerp*Time.fixedDeltaTime);
                    transform.eulerAngles = new Vector3(0, 0, Utils.AngleLerp(transform.eulerAngles.z, curSnap.angle, restoringLerp*Time.fixedDeltaTime));
                    if (isRigidbody) {
                        rb2D.velocity = Vector3.Lerp(rb2D.velocity, -curSnap.velocity, restoringLerp*Time.fixedDeltaTime);
                        rb2D.angularVelocity = Mathf.Lerp(rb2D.angularVelocity, -curSnap.angularVelocity, restoringLerp*Time.fixedDeltaTime);
                    }
                }
                else {
                    transform.position = curSnap.position;
                    transform.eulerAngles = new Vector3(0, 0, curSnap.angle);
                    if (isRigidbody) {
                        rb2D.velocity = -curSnap.velocity;
                        rb2D.angularVelocity = -curSnap.angularVelocity;
                    }
                }

                curSnap.numIntervals--;
            }
            // if time is playing forward, add to state stack
            else {
                // if in the same place as last frame, just add one to numIntervals of last state.
                ObjectSnap2D lastState = null;
                if ((objectTimeHistory.Count > 0
                    && (lastState = objectTimeHistory.Peek()) != null
                    && (lastState.position - Utils.Vector3to2(transform.position)).magnitude < similarityThreshold
                    && (lastState.velocity - rb2D.velocity).magnitude < similarityThreshold
                    && (lastState.angle - transform.eulerAngles.z) < similarityThreshold
                    && (lastState.angularVelocity - rb2D.angularVelocity) < similarityThreshold)) {
                        lastState.numIntervals++;
                }
                else {
                    objectTimeHistory.Push(new ObjectSnap2D(transform, rb2D));
                }
            }
        }
    }

    void UpdateOnPause() {
        if (isRigidbody && !isKinematic) {
            rb2D.isKinematic = !rb2D.isKinematic;
        }
        if (TimeEventManager.isPaused) {
            rb2D.mass /= TimeEventManager.slowScale;
            rb2D.gravityScale *= TimeEventManager.slowScale;

            // store velocity values
            pausedReverse = TimeEventManager.isReversed;
            pausedVelocity = rb2D.velocity;
            pausedAngularVelocity = rb2D.angularVelocity;
            rb2D.velocity = new Vector2(0, 0);
            rb2D.angularVelocity = 0.0f;
        }
        else {
            rb2D.mass *= TimeEventManager.slowScale;
            rb2D.gravityScale /= TimeEventManager.slowScale;

            // clear snaps
            oldObjectSnap = null;
            goalObjectSnap = null;
            progressToNextSnap = 0;

            // restore velocity values
            rb2D.velocity = pausedVelocity;
            rb2D.angularVelocity = pausedAngularVelocity;
            if (pausedReverse ^ TimeEventManager.isReversed) {
                // if reverse mode has changed, invert velocity
                rb2D.velocity *= -1;
                rb2D.angularVelocity *= -1;
            }
        }
    }

    void UpdateOnReverse() {
        if (isRigidbody) {
            rb2D.velocity *= -1;
            rb2D.angularVelocity *= -1;
        }
        if (TimeEventManager.isPaused) {
            ObjectSnap2D tmp = goalObjectSnap;
            goalObjectSnap = oldObjectSnap;
            oldObjectSnap = tmp;
        }
    }

    void OnDisable() {
        TimeEventManager.OnPause -= UpdateOnPause;
        TimeEventManager.OnReverse -= UpdateOnReverse;
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
    public int numIntervals;

    public ObjectSnap2D (Transform transform, Rigidbody2D rb = null) {
        position = transform.position;
        angle = transform.eulerAngles.z;
        numIntervals = 1;

        if (rb) {
            velocity = rb.velocity;
            angularVelocity = rb.angularVelocity;
        }
    }
}