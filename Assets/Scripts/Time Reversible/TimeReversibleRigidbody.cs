using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/* A physics object such as a square.
*/

public class TimeReversibleRigidbody : TimeReversibleObject<Rigidbody2DState> {
    protected Rigidbody2D rb2D;
    private bool isKinematic;
    private Vector2 pausedVelocity;
    private float pausedAngularVelocity;
    private bool pausedReverse;

    public bool restoringForce = true;
    public float restoringLerp = 10.0f;

    public override void ChildStart() {
        rb2D = GetComponent<Rigidbody2D>();
        
        isKinematic = rb2D.isKinematic;

        TimeEventManager.OnPause += UpdateOnPause;
        TimeEventManager.OnReverse += UpdateOnReverse;        
    }
    
    public override Rigidbody2DState GetCurrentState() {
        return new Rigidbody2DState(transform.position, transform.eulerAngles.z, rb2D.velocity, rb2D.angularVelocity);
    }
     
    public override float GetStateDifference(Rigidbody2DState state1, Rigidbody2DState state2) {
        float maxDiff = 
            (state1.position - state2.position).magnitude
            + (state1.velocity - state2.velocity).magnitude
            + Mathf.Abs(state1.angle - state2.angle) * Mathf.Deg2Rad;
                //include? Mathf.Abs(state1.angularVelocity - state2.angularVelocity));
        return maxDiff;
    }

    public override void UpdateObjectState(Rigidbody2DState state) {
        if (restoringForce && (Utils.Vector3to2(transform.position) - state.position).magnitude < 2) {
            transform.position = Vector3.Lerp(transform.position, state.position, restoringLerp*Time.fixedDeltaTime);
            transform.eulerAngles = new Vector3(0, 0, Utils.AngleLerp(transform.eulerAngles.z, state.angle, restoringLerp*Time.fixedDeltaTime));
            rb2D.velocity = Vector3.Lerp(rb2D.velocity, -state.velocity, restoringLerp*Time.fixedDeltaTime);
            rb2D.angularVelocity = Mathf.Lerp(rb2D.angularVelocity, -state.angularVelocity, restoringLerp*Time.fixedDeltaTime);
        }
        else {
            transform.position = state.position;
            transform.eulerAngles = new Vector3(0, 0, state.angle);
            rb2D.velocity = -state.velocity;
            rb2D.angularVelocity = -state.angularVelocity;
        }
    }

    public override Rigidbody2DState StateLerp(Rigidbody2DState state1, Rigidbody2DState state2, float f)
    {
        Vector2 newPos = Vector2.Lerp(state1.position, state2.position, f);
        float newA = Mathf.LerpAngle(state1.angle, state2.angle, f);
        Vector2 newV = Vector2.Lerp(state1.velocity, state2.velocity, f);
        float newW = Mathf.Lerp(state1.angularVelocity, state2.angularVelocity, f);
        
        return new Rigidbody2DState(newPos, newA, newV, newW);
    }

    void UpdateOnPause() {
        if (TimeEventManager.isPaused) {
            if (float.IsPositiveInfinity(TimeEventManager.slowFactor)) {
                rb2D.isKinematic = true;

                pausedVelocity = rb2D.velocity;
                pausedAngularVelocity = rb2D.angularVelocity;

                rb2D.velocity = new Vector2(0, 0);
                rb2D.angularVelocity = 0.0f;
            }
            else {
                rb2D.mass *= TimeEventManager.slowFactor;
                rb2D.velocity /= TimeEventManager.slowFactor;
                rb2D.angularVelocity /= TimeEventManager.slowFactor;
                rb2D.gravityScale /= TimeEventManager.slowFactor * TimeEventManager.slowFactor;
            }            
        }
        else {
            if (float.IsPositiveInfinity(TimeEventManager.slowFactor)) {

                rb2D.isKinematic = false;
                // restore velocity values
                rb2D.velocity = pausedVelocity;
                rb2D.angularVelocity = pausedAngularVelocity;
            }
            else {
                rb2D.mass /= TimeEventManager.slowFactor;
                rb2D.velocity *= TimeEventManager.slowFactor;
                rb2D.angularVelocity *= TimeEventManager.slowFactor;
                rb2D.gravityScale *= TimeEventManager.slowFactor * TimeEventManager.slowFactor;
            }
        }
    }

    void UpdateOnReverse() {
        rb2D.velocity *= -1;
        rb2D.angularVelocity *= -1;
        pausedVelocity *= -1;
        pausedAngularVelocity *= -1;
    }

    void OnDisable() {
        TimeEventManager.OnPause -= UpdateOnPause;
        TimeEventManager.OnReverse -= UpdateOnReverse;
    }

    public override Rigidbody2DState SlowDownState(Rigidbody2DState state) {
        return new Rigidbody2DState(state.position, state.angle, state.velocity / TimeEventManager.slowFactor, state.angularVelocity / TimeEventManager.slowFactor);
    }

    public override Rigidbody2DState SpeedUpState(Rigidbody2DState state) {
        return new Rigidbody2DState(state.position, state.angle, state.velocity * TimeEventManager.slowFactor, state.angularVelocity * TimeEventManager.slowFactor);
    }
}