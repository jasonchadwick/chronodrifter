using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

class PressurePlate : ControlObject<FloatState> {
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

    public override FloatState GetCurrentState() {
        return new FloatState(plateTransform.localPosition.y);
    }

    public override void UpdateObjectState(FloatState state) {
        plateTransform.localPosition = new Vector3(initPos.x, state.f, initPos.z);
    }

    public override float GetStateDifference(FloatState state1, FloatState state2) {
        return Mathf.Abs(state1.f - state2.f);
    }

    public override FloatState StateLerp(FloatState state1, FloatState state2, float f)
    {
        return new FloatState(Mathf.Lerp(state1.f, state2.f, f));
    }
}