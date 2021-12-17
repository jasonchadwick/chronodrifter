using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

class Elevator : ActivatedObject {
    public GameObject platformPrefab;
    public float velocity = 1.0f;
    public float fadeDistance = 0.5f;
    public float topOffset;
    public float botOffset;
    private Color defaultColor = Color.white;
    private Color fadeColor;

    public override void ChildStart() {
        TimeEventManager.OnReverse += Reverse;
        fadeColor = new Color(1, 1, 1, 0);
    }

    public override void ChildFixedUpdate() {
        if (!TimeEventManager.isPaused && isActive) {
            foreach (Transform childTransform in transform) {
                GameObject child = childTransform.gameObject;
                SpriteRenderer srender = child.GetComponent<SpriteRenderer>();
                ShadowCaster2D shadows = child.GetComponent<ShadowCaster2D>();

                float deltaTop = transform.position.y + topOffset - childTransform.position.y;
                float deltaBot = childTransform.position.y - (transform.position.y + botOffset);

                if (deltaTop >= 0 && deltaTop < fadeDistance) {
                    float lerp = deltaTop / fadeDistance;
                    srender.color = Color.Lerp(fadeColor, defaultColor, lerp);
                }
                else if (deltaTop < 0) {
                    childTransform.position = new Vector3(childTransform.position.x, transform.position.y + botOffset, childTransform.position.z);
                }
                else if (deltaBot >= 0 && deltaBot < fadeDistance) {
                    float lerp = deltaBot / fadeDistance;
                    srender.color = Color.Lerp(fadeColor, defaultColor, lerp);
                }
                else if (deltaBot < 0) {
                    childTransform.position = new Vector3(childTransform.position.x, transform.position.y + topOffset, childTransform.position.z);
                }
                else {
                    srender.color = defaultColor;
                }
                childTransform.position += Vector3.up * velocity * Time.fixedDeltaTime;
            }
        }
    }

    void Reverse() {
        velocity = -velocity;
    }

    void OnDisable() {
        TimeEventManager.OnReverse -= Reverse;
    }
}