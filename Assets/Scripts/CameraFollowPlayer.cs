using System.Collections.Generic;
using UnityEngine;

class CameraFollowPlayer : MonoBehaviour {
    public Transform target;
    public float cameraLerp = 5.0f;
    private Camera cam;

    void Start() {
        cam = Camera.main;
    }

    void Update() {
        if (target == null) {
            return;
        }
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), cameraLerp * Time.deltaTime);
    }
}