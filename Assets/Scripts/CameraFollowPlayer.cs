using System.Collections.Generic;
using UnityEngine;

class CameraFollowPlayer : MonoBehaviour {
    public Transform target;
    public float cameraLerp = 5.0f;
    public float bigSize = 18.0f;
    public float smallSize = 12.0f;
    private Camera cam;
    private float cameraZ;
    public bool zoomedOut;

    void Start() {
        cam = Camera.main;
        cameraZ = -30;
    }

    void Update() {
        if (target == null) {
            zoomedOut = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            zoomedOut = !zoomedOut;
        }

        if (zoomedOut) {
            ZoomOut();
        }
        else {
            ZoomFollowPlayer();
        }
    }

    void ZoomOut() {
        transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, cameraZ), cameraLerp * Time.deltaTime);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, bigSize, cameraLerp*Time.deltaTime);
    }

    void ZoomFollowPlayer() {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, cameraZ), cameraLerp * Time.deltaTime);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, smallSize, cameraLerp*Time.deltaTime);
    }
}