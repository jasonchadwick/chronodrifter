using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

class PressableButton : MonoBehaviour {
    public float pressTime = 2.0f;
    public GameObject target;
    private Transform plateTransform;
    private float timePressed;
    private float scaleY;
    private float initX;
    private float initY;
    private float initZ;
    private bool activated;
    private bool pressingDown;
    private Stack<float> yHistory;

    void Start() {
        timePressed = 0;
        activated = false;
        pressingDown = false;
        plateTransform = transform.GetChild(0);
        scaleY = plateTransform.localScale.y;
        initX = plateTransform.localPosition.x;
        initY = plateTransform.localPosition.y;
        initZ = plateTransform.localPosition.z;
        yHistory = new Stack<float>();
    }

    void OnTriggerEnter2D(Collider2D obj) {
        if (obj.gameObject.layer != 6) {
            pressingDown = true;
            activated = true;
        }
    }

    void OnTriggerStay2D(Collider2D obj) {
        if (obj.gameObject.layer != 6) {
            pressingDown = true;
            activated = true;
        }
    }

    void OnTriggerExit2D(Collider2D obj) {
        if (obj.gameObject.layer != 6) {
            pressingDown = false;
            activated = false;
        }
    }

    void FixedUpdate() {
        if (!TimeEventManager.isPaused) {
                if (!TimeEventManager.isReversed) {
                    plateTransform.localPosition = new Vector3(initX, Mathf.Lerp(initY, initY - scaleY, timePressed / pressTime), initZ);
                    yHistory.Push(plateTransform.localPosition.y);
                    if (pressingDown && timePressed < pressTime) {
                        timePressed += Time.fixedDeltaTime;
                    }
                    else if (!pressingDown && timePressed > 0) {
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

        if (activated) {
            target.GetComponent<ButtonActivatedObject>().isActive = true;
            foreach (Light2D light in plateTransform.GetComponentsInChildren<Light2D>()) {
                light.intensity = 1;
            }
        }
        else {
            target.GetComponent<ButtonActivatedObject>().isActive = false;
            foreach (Light2D light in plateTransform.GetComponentsInChildren<Light2D>()) {
                light.intensity = 0;
            }
        }
    }
}