using System.Collections.Generic;
using UnityEngine;

/* The time ghost that is visible when time is reversed, showing previous
   actions of the player. 
*/

class TimeGhost : TimeReversibleObject<PositionState> {
    public GameObject ghostPrefab;
    private GameObject ghost;

    public override void ChildStart() {
        TimeEventManager.OnPause += OnPause;
        TimeEventManager.OnReverse += OnReverse;
    }

    void OnPause() {
        if (TimeEventManager.isReversed && ghost == null) {
            ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        }
        else if (!TimeEventManager.isReversed) {
            Destroy(ghost);
        }
    }

    void OnReverse() {
        if (TimeEventManager.isReversed && ghost == null) {
            ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        }
        else if (!TimeEventManager.isReversed) {
            Destroy(ghost);
        }
    }

    public override PositionState GetCurrentState()
    {
        return new PositionState(Utils.Vector3to2(transform.position));
    }

    public override float GetStateDifference(PositionState state1, PositionState state2)
    {
        return (state1.position - state2.position).magnitude;
    }

    public override void UpdateObjectState(PositionState state)
    {   
        if (ghost != null) {
            ghost.transform.position = state.position;
        }
    }

    public override void OnStackSize1(PositionState state)
    {
        Destroy(ghost);
    }

    public override PositionState StateLerp(PositionState state1, PositionState state2, float f) {
        return new PositionState(Vector2.Lerp(state1.position, state2.position, f));
    }
}