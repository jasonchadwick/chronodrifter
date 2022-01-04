using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/* Pillar velocity is currently not saved, and it instead lerps to
   its goal position. Could change this in the future.
*/

class MovingPillar : ActivatedObject<PositionState> {
    public float moveTime;
    public float moveLerp;
    public float moveDistance;
    public bool crushing;
    public bool crushOnActive;
    public float crushDistance;
    private BoxCollider2D crushCollider;
    private Vector3 activePos;
    private Vector3 inactivePos;
    private Light2D activeLight;

    public override void ChildStart() {
        inactivePos = transform.position;
        activePos = inactivePos + transform.up * moveDistance;
        activeLight = GetComponentInChildren<Light2D>();
        if (crushing) {
            crushCollider = transform.Find("PlayerCrusher").GetComponent<BoxCollider2D>();
        }
    }

    public override void ChildFixedUpdate() {
        if ((transform.position - inactivePos).magnitude > 1.0f) {
            activeLight.intensity = 0.5f;
        }
        else {
            activeLight.intensity = 0;
        }

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
                if ((transform.position - activePos).magnitude > 0.05f) {
                    transform.position = Vector3.Lerp(transform.position, activePos, moveLerp * Time.fixedDeltaTime / TimeEventManager.curSlowFactor);
                }
                else {
                    transform.position = activePos;
                }
            }
            else {
                if ((transform.position - inactivePos).magnitude > 0.05f) {
                    transform.position = Vector3.Lerp(transform.position, inactivePos, moveLerp * Time.fixedDeltaTime / TimeEventManager.curSlowFactor);
                }
                else {
                    transform.position = inactivePos;
                }
            }
        }
    }

    public override PositionState GetCurrentState() {
        return new PositionState(transform.position);
    }

    public override float GetStateDifference(PositionState state1, PositionState state2)
    {
        return (state1.position - state2.position).magnitude;
    }

    public override void UpdateObjectState(PositionState state)
    {
        transform.position = state.position;
    }

    public override PositionState StateLerp(PositionState state1, PositionState state2, float f) {
        return new PositionState(Vector3.Lerp(state1.position, state2.position, f));
    }
}