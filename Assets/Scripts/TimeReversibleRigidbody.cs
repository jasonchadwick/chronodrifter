using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class TimeReversibleRigidbody : TimeReversibleObject {
    private Stack<Rigidbody2DState> objectTimeHistory;
    private bool isRigidbody;
    private Rigidbody2D rb2D;
    private bool isKinematic;
    private Vector2 pausedVelocity;
    private float pausedAngularVelocity;
    private bool pausedReverse;

    private Rigidbody2DState curSnap;

    public bool useHistoryStack = true;
    public bool restoringForce = true;
    public float restoringLerp = 10.0f;

    public override void ChildStart() {
        if ((rb2D = GetComponent<Rigidbody2D>()) == null) {
            isRigidbody = false;
        }
        else {
            isRigidbody = true;
            isKinematic = rb2D.isKinematic;
        }

        TimeEventManager.OnPause += UpdateOnPause;
        TimeEventManager.OnReverse += UpdateOnReverse;
    }
    
    public override State GetCurrentState() {
        Light2D light = GetComponentInChildren<Light2D>();
        float intensity = 0.0f;
        if (light != null) {
            intensity = light.intensity;
        }
        return new Rigidbody2DState(transform, rb2D, GetComponent<SpriteRenderer>().color, intensity);
    }
     
    public override float GetStateDifference(State s1, State s2) {
        Rigidbody2DState state1 = s1 as Rigidbody2DState;
        Rigidbody2DState state2 = s2 as Rigidbody2DState;
        float maxDiff = 
            Mathf.Max(
                Mathf.Max(
                    Mathf.Max(
                        (state1.position - state2.position).magnitude,
                        (state1.velocity - state2.velocity).magnitude),
                        (state1.angle - state2.angle)),
                        (state1.angularVelocity - state2.angularVelocity));
        return maxDiff;
    }

    public override void UpdateObjectState(State s) {
        Rigidbody2DState state = s as Rigidbody2DState;
        if (restoringForce && (Utils.Vector3to2(transform.position) - state.position).magnitude < 2) {
            transform.position = Vector3.Lerp(transform.position, state.position, restoringLerp*Time.fixedDeltaTime);
            transform.eulerAngles = new Vector3(0, 0, Utils.AngleLerp(transform.eulerAngles.z, state.angle, restoringLerp*Time.fixedDeltaTime));
            if (isRigidbody) {
                rb2D.velocity = Vector3.Lerp(rb2D.velocity, -state.velocity, restoringLerp*Time.fixedDeltaTime);
                rb2D.angularVelocity = Mathf.Lerp(rb2D.angularVelocity, -state.angularVelocity, restoringLerp*Time.fixedDeltaTime);
            }
        }
        else {
            transform.position = state.position;
            transform.eulerAngles = new Vector3(0, 0, state.angle);
            if (isRigidbody) {
                rb2D.velocity = -state.velocity;
                rb2D.angularVelocity = -state.angularVelocity;
            }
        }
        GetComponent<SpriteRenderer>().color = state.color;
        Light2D light = GetComponentInChildren<Light2D>();
        if (light != null) {
            light.intensity = state.intensity;
        }
    }

    public override void ChildFixedUpdate() {}

    void UpdateOnPause() {
        if (TimeEventManager.isPaused) {
            // store velocity values
            pausedReverse = TimeEventManager.isReversed;
            pausedVelocity = rb2D.velocity;
            pausedAngularVelocity = rb2D.angularVelocity;
            rb2D.velocity = new Vector2(0, 0);
            rb2D.angularVelocity = 0.0f;
        }
        else {
            if (isKinematic) {
                rb2D.isKinematic = true;
            }
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
    }

    void OnDisable() {
        TimeEventManager.OnPause -= UpdateOnPause;
        TimeEventManager.OnReverse -= UpdateOnReverse;
    }
}

// general object that can be time reversed
// possible optimization: for non-rigidbodies, don't save linear/angular velocity
public class Rigidbody2DState : State {
    public Vector2 position;

    // third Euler angle (the only one we need for 2D)
    public float angle;
    public Vector2 velocity;
    public float angularVelocity;
    public Color color;
    public float intensity;

    public Rigidbody2DState (Transform transform, Rigidbody2D rb, Color c, float i) {
        position = transform.position;
        angle = transform.eulerAngles.z;

        // TODO: make subclass for lit square, including these properties
        color = c;
        intensity = i;

        if (rb) {
            velocity = rb.velocity;
            angularVelocity = rb.angularVelocity;
        }
    }
}