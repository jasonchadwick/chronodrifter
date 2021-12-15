using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

class Elevator : ActivatedObject {
    public float velocity = 1.0f;
    public Color frontColor = Color.white;
    public float colorChangeLerp = 1.0f;
    private Color backColor;
    private bool paused;

    void Start() {
        backColor = new Color(frontColor.r, frontColor.g, frontColor.b, 0);
        paused = false;
        TimeEventManager.OnPause += Pause;
        TimeEventManager.OnReverse += Reverse;
    }

    void Update() {
        if (!paused && isActive) {
            foreach (Transform childTransform in transform) {
                GameObject child = childTransform.gameObject;
                float childVelocity = velocity;
                ElevatorPlatform platform = child.GetComponent<ElevatorPlatform>();
                SpriteRenderer srender = child.GetComponent<SpriteRenderer>();
                BoxCollider2D collider = child.GetComponent<BoxCollider2D>();
                ShadowCaster2D shadows = child.GetComponent<ShadowCaster2D>();

                if (platform.isInFront) {
                    srender.color = Color.Lerp(srender.color, frontColor, colorChangeLerp*Time.deltaTime);
                    collider.enabled = true;
                    shadows.enabled = true;
                    child.layer = 6;
                }
                else {
                    childVelocity = -velocity;
                    srender.color = Color.Lerp(srender.color, backColor, colorChangeLerp*Time.deltaTime);
                    if (srender.color.a < 0.1) {
                        collider.enabled = false;
                        shadows.enabled = false;
                    }
                    child.layer = 3;
                }
                childTransform.position += Vector3.up * childVelocity * Time.deltaTime;

                if (childTransform.position.y >= transform.position.y + 6) {
                    if (velocity > 0) {
                        // if elevator is going upwards
                        platform.isInFront = false;
                    }
                    else {
                        platform.isInFront = true;
                    }
                }
                else if (childTransform.position.y <= transform.position.y - 3) {
                    if (velocity > 0) {
                        // if elevator is going upwards
                        platform.isInFront = true;
                    }
                    else {
                        platform.isInFront = false;
                    }
                }
            }
        }
    }

    void FixedUpdate() {
        
    }

    void Pause() {
        paused = !paused;
    }

    void Reverse() {
        velocity = -velocity;
    }

    void OnDisable() {
        TimeEventManager.OnPause -= Pause;
        TimeEventManager.OnReverse -= Reverse;
    }
}

// for child:
// if infront: move up until at top
// when at top: move to background (turn off collider, lerp color with dark color, shrink size)
// then move down until at bottom