using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

class MovingPillar : ActivatedObject {
    public float moveTime;
    public float moveLerp;
    public float moveDistance;
    private Vector3 activePos;
    private Vector3 inactivePos;
    private Light2D activeLight;

    public override void ChildStart() {
        inactivePos = transform.position;
        activePos = inactivePos + transform.up * moveDistance;
        activeLight = GetComponentInChildren<Light2D>();
    }

    public override void ChildFixedUpdate() {
        if ((transform.position - inactivePos).magnitude > 1.0f) {
            activeLight.intensity = 0.5f;
        }
        else {
            activeLight.intensity = 0;
        }
        if (!TimeEventManager.isPaused) {
            if (!TimeEventManager.isReversed) {
                if (IsActive()) {
                    if ((transform.position - activePos).magnitude > 0.05f) {
                        transform.position = Vector3.Lerp(transform.position, activePos, moveLerp * Time.fixedDeltaTime);
                    }
                    else {
                        transform.position = activePos;
                    }
                }
                else {
                    if ((transform.position - inactivePos).magnitude > 0.05f) {
                        transform.position = Vector3.Lerp(transform.position, inactivePos, moveLerp * Time.fixedDeltaTime);
                    }
                    else {
                        transform.position = inactivePos;
                    }
                }
            }
        }
    }

    public override State GetCurrentState() {
        return new PillarState(transform.position);
    }

    public override float GetStateDifference(State s1, State s2)
    {
        PillarState state1 = s1 as PillarState;
        PillarState state2 = s2 as PillarState;
        return (state1.pos - state2.pos).magnitude;
    }

    public override void UpdateObjectState(State s)
    {
        PillarState state = s as PillarState;
        transform.position = state.pos;
    }
}

class PillarState : State {
    public Vector3 pos;

    public PillarState(Vector3 p) {
        pos = p;
    }
}