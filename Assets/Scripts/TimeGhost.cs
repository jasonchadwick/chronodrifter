using System.Collections.Generic;
using UnityEngine;

// same idea as TimeReversibleObject but only need positions instead of everything else.
class TimeGhost : MonoBehaviour {
    public GameObject ghostPrefab;
    private Stack<Vector2> positionHistory;
    private GameObject ghost;

    void Start() {
        TimeEventManager.OnPause += OnPause;
        TimeEventManager.OnReverse += OnReverse;
        positionHistory = new Stack<Vector2>();
        positionHistory.Push(Utils.Vector3to2(transform.position));
    }

    void FixedUpdate() {
        if (!TimeEventManager.isPaused) {
            if (TimeEventManager.isReversed && positionHistory.Count > 0) {
                if (positionHistory.Count > 1) {
                    ghost.transform.position = positionHistory.Pop();
                }
                else {
                    ghost.transform.position = positionHistory.Peek();
                }
            }
            else {
                positionHistory.Push(Utils.Vector3to2(transform.position));
            }
        }
    }

    void OnPause() {
        if (TimeEventManager.isPaused) {
            ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        }
        else {
            Destroy(ghost);
        }
    }

    void OnReverse() {
        if (TimeEventManager.isReversed) {
            ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        }
        else {
            positionHistory.Clear();
            Destroy(ghost);
        }
    }
}
// alpha 50