using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

// TODO: make it work with general TimeReversibleObject framework instead of using its own stack
class PressurePlate : ControlObject {
    public float pressTime = 2.0f;
    private Transform plateTransform;
    private float timePressed;
    private float scaleY;
    private float initX;
    private float initY;
    private float initZ;
    private int pressingCount;
    private Stack<float> yHistory;
    private ActivatedObject targetComponent;

    void Start() {
        timePressed = 0;
        pressingCount = 0;
        plateTransform = transform.GetChild(0);
        scaleY = plateTransform.localScale.y;
        initX = plateTransform.localPosition.x;
        initY = plateTransform.localPosition.y;
        initZ = plateTransform.localPosition.z;
        yHistory = new Stack<float>();
    }

    void OnTriggerEnter2D(Collider2D obj) {
        if (obj.gameObject.layer != 6) {
            pressingCount++;
            Activate();
        }
    }

    /*void OnTriggerStay2D(Collider2D obj) {
        if (obj.gameObject.layer != 6) {
            pressingCount = true;
            foreach (Light2D light in plateTransform.GetComponentsInChildren<Light2D>()) {
                light.intensity = 1;
            }
        }
    }*/

    void OnTriggerExit2D(Collider2D obj) {
        if (obj.gameObject.layer != 6) {
            pressingCount--;
            if (pressingCount == 0) {
                Deactivate();
            }
            foreach (Light2D light in plateTransform.GetComponentsInChildren<Light2D>()) {
                light.intensity = 0;
            }
        }
    }

    void FixedUpdate() {
        if (!TimeEventManager.isPaused) {
            if (!TimeEventManager.isReversed) {
                plateTransform.localPosition = new Vector3(initX, Mathf.Lerp(initY, initY - scaleY, timePressed / pressTime), initZ);
                yHistory.Push(plateTransform.localPosition.y);
                if (pressingCount > 0 && timePressed < pressTime) {
                    timePressed += Time.fixedDeltaTime;
                }
                else if (pressingCount == 0 && timePressed > 0) {
                    timePressed -= Time.fixedDeltaTime;
                }
            }
            else {
                float curY;
                if (yHistory.Count > 1) {
                    curY = yHistory.Pop();
                }
                else {
                    curY = yHistory.Peek();
                }
                plateTransform.localPosition = new Vector3(initX, curY, initZ);
            }
        }
    }
}