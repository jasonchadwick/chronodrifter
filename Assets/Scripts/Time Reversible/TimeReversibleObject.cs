using System.Collections.Generic;
using UnityEngine;

// General class for a time reversible object. Maintains a stack of previous
// states. Forward time adds new states to the stack, and reverse pops them.

public abstract class TimeReversibleObject : MonoBehaviour {
    // stack of previous states.
    private Stack<State> objectHistory;
    private State state;
    private State lastState;
    private float maxStackSize;
    public float similarityThreshold = 5e-2f;
    private float maxHistorySeconds = 5*60; // five minutes

    void Start() {
        objectHistory = new Stack<State>();
        ChildStart();
        maxStackSize = maxHistorySeconds/Time.fixedDeltaTime;
    }

    void FixedUpdate() {
        if (!TimeEventManager.isPaused) {
            if (TimeEventManager.isReversed) {
                if (objectHistory.Count > 1) {
                    state = objectHistory.Peek();
                    if (state.numIntervals <= 1) {
                        objectHistory.Pop();
                    }
                }
                else if (objectHistory.Count == 1) {
                    state = objectHistory.Peek();
                    OnStackSize1(state);
                }
                else {
                    OnStackEmpty();
                }

                UpdateObjectState(state);
                state.numIntervals--;
            }
            else if (objectHistory.Count < maxStackSize) {
                state = GetCurrentState();
                if (objectHistory.Count > 0) {
                    lastState = objectHistory.Peek();
                    if (GetStateDifference(state, lastState) < similarityThreshold) {
                        lastState.numIntervals++;
                    }
                    else {
                        objectHistory.Push(state);
                    }
                }
                else {
                    objectHistory.Push(state);
                }
            }
        }
        ChildFixedUpdate();
    }

    // The following functions can be implemented by each child class.
    public virtual State GetCurrentState() {return new State();}
    public virtual float GetStateDifference(State state1, State state2) {return 0.0f;}
    public virtual void UpdateObjectState(State s) {}
    public virtual void OnStackSize1(State s) {}
    public virtual void OnStackEmpty() {}
    public virtual void ChildStart() {}
    public virtual void ChildFixedUpdate() {}
}

// child classes of TimeReversibleObject will implement a specific State class.
public class State : Object {
    // Number of FixedUpdate iterations for which the object has been in this State.
    public int numIntervals = 1;
}