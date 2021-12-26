using System.Collections.Generic;
using UnityEngine;

class TimeGhost : TimeReversibleObject {
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

    public override State GetCurrentState()
    {
        return new PositionState(Utils.Vector3to2(transform.position));
    }

    public override float GetStateDifference(State s1, State s2)
    {
        PositionState state1 = s1 as PositionState;
        PositionState state2 = s2 as PositionState;
        return (state1.position - state2.position).magnitude;
    }

    public override void UpdateObjectState(State s)
    {   
        PositionState state = s as PositionState;
        if (ghost != null) {
            ghost.transform.position = state.position;
        }
    }

    public override void OnStackSize1(State s)
    {
        Destroy(ghost);
    }

    public override State StateLerp(State s1, State s2, float f) {
        PositionState state1 = s1 as PositionState;
        PositionState state2 = s2 as PositionState;
        return new PositionState(Vector2.Lerp(state1.position, state2.position, f));
    }
}

public class PositionState : State {
    public Vector2 position;

    public PositionState(Vector2 p) {
        position = p;
    }
}