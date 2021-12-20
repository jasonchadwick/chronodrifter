using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

class MovingPillar : ActivatedObject {
    public float baseYScale;
    public float activeYScale;
    public float moveTime;
    private Vector3 scaleAnchor;
    private float activeTime;
    private Vector3 activePos;
    private Vector3 inactivePos;
    private Light2D activeLight;

    public override void ChildStart() {
        activeTime = 0;
        if (defaultActiveStatus) {
            // TODO: when it starts, lerps from inactive to active based on activeTime.
            // need to adjust activeTime to account for when it defaults to active.
            activePos = transform.position;
            inactivePos = activePos - Vector3.Project(transform.localScale, transform.up) * 15;
        }
        else {
            inactivePos = transform.position;
            activePos = inactivePos + Vector3.Project(transform.localScale, transform.up) * 15;
            Debug.Log(inactivePos);
            Debug.Log(activePos);
        }
        activeLight = GetComponentInChildren<Light2D>();
    }

    public override void ChildFixedUpdate() {
        if (activeTime > 0.01f) {
            activeLight.intensity = 0.5f;
        }
        else {
            activeLight.intensity = 0;
        }
        if (!TimeEventManager.isPaused) {
            if (!TimeEventManager.isReversed) {
                if (IsActive()) {
                    if (activeTime < moveTime) {
                        activeTime += Time.fixedDeltaTime;
                        transform.position = Vector3.Lerp(inactivePos, activePos, activeTime / moveTime);
                    }
                    else if (activeTime > moveTime) {
                        activeTime = moveTime;
                        transform.position = activePos;
                    }
                }
                else {
                    if (activeTime > 0) {
                        activeTime -= Time.fixedDeltaTime;
                        transform.position = Vector3.Lerp(inactivePos, activePos, activeTime / moveTime);
                    }
                    else if (activeTime < 0) {
                        activeTime = 0;
                        transform.position = inactivePos;
                    }
                }
            }
        }
    }

    public override State GetCurrentState() {
        return new PillarState(transform.position, activeTime);
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
        activeTime = state.activeTime;
    }
}

class PillarState : State {
    public Vector3 pos;
    public float activeTime;

    public PillarState(Vector3 p, float t) {
        pos = p;
        activeTime = t;
    }
}