using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/* Pillar velocity is currently not saved, and it instead lerps to
   its goal position. Could change this in the future.
*/

class MovingPillar : ActivatedObject<PillarState> {
    public float velocity;
    public float moveDistance;
    public bool crushing;
    public bool crushOnActive;
    public float crushDistance;
    private Rigidbody2D rb2d;
    private BoxCollider2D crushCollider;
    private Vector3 activePos;
    private Vector3 inactivePos;
    private Light2D activeLight;

    public override void ChildStart() {
        rb2d = GetComponent<Rigidbody2D>();
        inactivePos = transform.position;
        activePos = inactivePos + transform.up * moveDistance;
        activeLight = GetComponentInChildren<Light2D>();
        if (crushing) {
            crushCollider = transform.Find("PlayerCrusher").GetComponent<BoxCollider2D>();
        }
    }

    public override void ChildFixedUpdate() {
        if (crushing) {
            Vector3 crushPos;
            if (crushOnActive) {
                crushPos = activePos;
            }
            else {
                crushPos = inactivePos;
            }

            if ((transform.position - crushPos).magnitude < crushDistance) {
                crushCollider.enabled = true;
            }
            else {
                crushCollider.enabled = false;
            }
        }

        if (!TimeEventManager.isReversed) {
            if (IsActive()) {
                activeLight.intensity = 0.5f;
                float normDistance = Vector3.Dot(activePos - transform.position, transform.up) / moveDistance;
                if (normDistance > 0.01f) {
                    rb2d.velocity = transform.up * velocity;
                }
                else {
                    rb2d.velocity = Vector2.zero;
                    transform.position = activePos;
                }
            }
            else {
                activeLight.intensity = 0;
                float normDistance = Vector3.Dot(transform.position - inactivePos, transform.up) / moveDistance;
                if (normDistance > 0.01f) {
                    rb2d.velocity = -transform.up * velocity;
                }
                else {
                    rb2d.velocity = Vector2.zero;
                    transform.position = inactivePos;
                }
            }
            if (moveDistance < 0) {
                rb2d.velocity *= -1;
            }
        }
    }

    public override PillarState GetCurrentState() {
        return new PillarState(transform.position, rb2d.velocity, activeLight.intensity);
    }

    public override float GetStateDifference(PillarState state1, PillarState state2)
    {
        return (state1.position - state2.position).magnitude 
             + (state1.velocity - state2.velocity).magnitude
             + Mathf.Abs(state1.intensity - state2.intensity);
    }

    public override void UpdateObjectState(PillarState state)
    {
        transform.position = state.position;
        rb2d.velocity = -state.velocity;
        activeLight.intensity = state.intensity;
    }

    public override PillarState StateLerp(PillarState state1, PillarState state2, float f) {
        return new PillarState(Vector2.Lerp(state1.position, state2.position, f),
                               Vector2.Lerp(state1.velocity, state2.velocity, f),
                               Mathf.Lerp(state1.intensity, state2.intensity, f));
    }
}