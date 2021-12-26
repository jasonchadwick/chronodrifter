using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

class PressurePlate : ControlObject {
    public float pressTime;
    public float pressDistance;
    private Transform plateTransform;
    private float timePressed;
    private Vector3 initPos;
    private int pressingCount;

    public override void ChildStart() {
        timePressed = 0;
        pressingCount = 0;

        // want to move the plate, not the (invisible) collider box
        plateTransform = transform.GetChild(0);
        initPos = plateTransform.localPosition;
    }

    void OnTriggerEnter2D(Collider2D obj) {
        if (obj.gameObject.layer != 6) {
            pressingCount++;
            Activate();
        }
    }

    void OnTriggerExit2D(Collider2D obj) {
        if (obj.gameObject.layer != 6) {
            pressingCount--;
            if (pressingCount == 0) {
                Deactivate();
            }
        }
    }

    public override void ChildFixedUpdate() {
        if (!TimeEventManager.isReversed) {
            plateTransform.localPosition = new Vector3(initPos.x, Mathf.Lerp(initPos.y, initPos.y - pressDistance, timePressed / pressTime), initPos.z);
            if (pressingCount > 0 && timePressed < pressTime) {
                timePressed += Time.fixedDeltaTime / TimeEventManager.curSlowFactor;
            }
            else if (pressingCount == 0 && timePressed > 0) {
                timePressed -= Time.fixedDeltaTime / TimeEventManager.curSlowFactor;
            }
        }
    }

    public override State GetCurrentState() {
        return new FloatState(plateTransform.localPosition.y);
    }

    public override void UpdateObjectState(State s) {
        FloatState state = s as FloatState;
        plateTransform.localPosition = new Vector3(initPos.x, state.f, initPos.z);
    }

    public override float GetStateDifference(State s1, State s2) {
        FloatState state1 = s1 as FloatState;
        FloatState state2 = s2 as FloatState;
        return Mathf.Abs(state1.f - state2.f);
    }
}

public class FloatState : State {
    public float f;

    public FloatState(float num) {
        f = num;
    }
}