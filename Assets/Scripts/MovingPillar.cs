using System.Collections.Generic;
using UnityEngine;

class MovingPillar : ButtonActivatedObject {
    public float baseYScale;
    public float activeYScale;
    public float moveTime;
    private Vector3 scaleAnchor;
    private float xScale;
    private float zScale;
    private Stack<float> ys;
    private float activeTime;

    void Start() {
        ys = new Stack<float>();
        activeTime = 0;
        xScale = transform.localScale.x;
        zScale = transform.localScale.z;
        scaleAnchor = new Vector3(transform.localPosition.x, transform.localPosition.y - baseYScale/2, transform.localPosition.z);
        Debug.Log(scaleAnchor);
    }

    void FixedUpdate() {
        if (!TimeEventManager.isPaused) {
            float yScale = 0;
            if (TimeEventManager.isReversed) {
                float newY;
                if (ys.Count > 1) {
                    newY = ys.Pop();
                }
                else {
                    newY = ys.Peek();
                }
                yScale = newY;
                if (newY < transform.localScale.y) {
                    activeTime -= Time.fixedDeltaTime;
                }
                else {
                    activeTime += Time.fixedDeltaTime;
                }
            }
            else if (isActive) {
                if (activeTime < moveTime) {
                    activeTime += Time.fixedDeltaTime;
                }
                yScale = Mathf.Lerp(baseYScale, activeYScale, activeTime / moveTime);
                ys.Push(yScale);
            }
            else if (!isActive) {
                if (activeTime > 0) {
                    activeTime -= Time.fixedDeltaTime;
                }
                yScale = Mathf.Lerp(baseYScale, activeYScale, activeTime / moveTime);
                ys.Push(yScale);
            }

            if (yScale != 0) {
                Utils.ScaleFromPoint(transform, scaleAnchor, new Vector3(xScale, yScale, zScale));
            }
        }
    }

}