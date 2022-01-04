using System.Collections.Generic;
using UnityEngine;

// General class for a time reversible object. Maintains a stack of previous
// states. Forward time adds new states to the stack, and reverse pops them.

public abstract class TimeReversibleObject<StateType> : MonoBehaviour {
    // stack of previous states.
    private ObjectHistoryArray<StateType> objectHistory;
    private StateType state;
    private StateType prevState;

    // for use with slowed time
    private StateType nextState;
    private int timeStepProgress;
    private int timeStepsPerState = 1;
    private int tmpStepProgress;
    private int tmpSteps;
    
    private int lastStateIdx = -1;
    private int maxStackSize;
    public float similarityThreshold = 5e-2f;
    private float maxHistorySeconds = 60*60; // five minutes

    // does not allocate enough space. Need to define the array in the inheriting class
    // need InitArray() function
    // should have no explicit references to the array in this class

    void Start() {
        maxStackSize = (int) Mathf.Round(maxHistorySeconds/Time.fixedDeltaTime);
        objectHistory = new ObjectHistoryArray<StateType>(maxStackSize);
        ChildStart();

        TimeEventManager.OnPause += UpdateOnPause;
        TimeEventManager.OnReverse += UpdateOnReverse;

        prevState = GetCurrentState();
        nextState = GetCurrentState();
        state = prevState;
    }

    void FixedUpdate() {
        // case where paused with infinite slowdown is handled separately in inheriting classes.
        if (!(TimeEventManager.isPaused && float.IsPositiveInfinity(TimeEventManager.slowFactor))) {
            if (TimeEventManager.isReversed) {
                if (tmpStepProgress >= 0) {
                    if (tmpStepProgress == 0) {
                        state = prevState;
                        timeStepProgress = timeStepsPerState-1;
                        if (lastStateIdx > 0) {
                            prevState = objectHistory.Get(lastStateIdx);
                            if (((State) prevState).numIntervals <= 1) {
                                lastStateIdx--;
                            }
                            objectHistory.Dec(lastStateIdx);
                        }
                        else if (lastStateIdx == 0) {
                            prevState = objectHistory.Get(lastStateIdx);
                            OnStackSize1(prevState);
                            objectHistory.Dec(lastStateIdx);
                        }
                        else {
                            OnStackEmpty();
                        }
                    }
                    else {
                        if (GetStateDifference(prevState, nextState) < 20) {
                            state = StateLerp(prevState, nextState, (float) (tmpStepProgress) / tmpSteps);
                        }
                        else {
                            if (2*tmpStepProgress < tmpSteps) {
                                state = prevState;
                            }
                            else {
                                state = nextState;
                            }
                        }
                    }
                    tmpStepProgress--;
                }
                
                else {
                    if (timeStepProgress == timeStepsPerState-1) {
                        nextState = prevState;

                        if (lastStateIdx > 0) {
                            prevState = objectHistory.Get(lastStateIdx);
                            if (((State) prevState).numIntervals <= 1) {
                                lastStateIdx--;
                            }
                            objectHistory.Dec(lastStateIdx);
                        }
                        else if (lastStateIdx == 0) {
                            prevState = objectHistory.Get(lastStateIdx);
                            OnStackSize1(prevState);
                            objectHistory.Dec(lastStateIdx);
                        }
                        else {
                            OnStackEmpty();
                        }
                    }
                    
                    if (GetStateDifference(prevState, nextState) < 20) {
                        state = StateLerp(prevState, nextState, (float) (timeStepProgress) / timeStepsPerState);
                    }
                    else {
                        if (2*timeStepProgress < timeStepsPerState) {
                            state = prevState;
                        }
                        else {
                            state = nextState;
                        }
                    }

                    timeStepProgress = (timeStepProgress - 1 + timeStepsPerState) % timeStepsPerState;
                }
                if (TimeEventManager.isPaused) {
                    state = SlowDownState(state);
                }
                UpdateObjectState(state);
            }
            else {
                if (timeStepProgress == timeStepsPerState-1) {
                    state = GetCurrentState();

                    if (TimeEventManager.isPaused) {
                        state = SpeedUpState(state);
                    }
                    if (lastStateIdx >= 0) {
                        StateType s = objectHistory.Get(lastStateIdx);
                        if (GetStateDifference(state, s) < similarityThreshold) {
                            objectHistory.Inc(lastStateIdx);
                            state = s;
                        }
                        else if (lastStateIdx < maxStackSize - 1) {
                            lastStateIdx++;
                            objectHistory.Set(lastStateIdx, state);
                        }
                    }
                    else if (lastStateIdx < maxStackSize - 1) {
                        lastStateIdx++;
                        objectHistory.Set(lastStateIdx, state);
                    }
                    nextState = prevState = state;
                }
                timeStepProgress = (timeStepProgress + 1) % timeStepsPerState;
            }
        }
        ChildFixedUpdate();
    }

    void UpdateOnPause() {
        tmpStepProgress = -1;

        timeStepsPerState = (int) TimeEventManager.curSlowFactor;
        if (TimeEventManager.isPaused) {
            timeStepProgress = timeStepsPerState - 1;
        }
        else {
            timeStepProgress = 0;
        }
    }

    void UpdateOnReverse() {
        if (TimeEventManager.isReversed && TimeEventManager.isPaused) {
            nextState = GetCurrentState();
            tmpSteps = timeStepProgress;
            tmpStepProgress = tmpSteps - 1;
        }
        else {
            tmpStepProgress = -1;
        }
    }

    // The following functions can be implemented by each child class.
    public virtual StateType GetCurrentState() {return default(StateType);}
    public virtual float GetStateDifference(StateType state1, StateType state2) {return 0.0f;}
    public virtual void UpdateObjectState(StateType s) {}
    public virtual void OnStackSize1(StateType s) {}
    public virtual void OnStackEmpty() {}
    public virtual void ChildStart() {}
    public virtual void ChildFixedUpdate() {}
    public virtual StateType StateLerp(StateType state1, StateType state2, float f) {return state2;}
    public virtual StateType SlowDownState(StateType state) {return state;}
    public virtual StateType SpeedUpState(StateType state) {return state;}
}

public class ObjectHistoryArray<T> {
    private T[] array;


    public ObjectHistoryArray(int size) {
        array = new T[size];
    }

    public T Get(int idx) {
        return array[idx];
    }

    public void Set(int idx, T val) {
        array[idx] = val;
    }

    public void Inc(int idx) {
        State state = (State) (array[idx]);
        if (state != null) {
            state.Increment();
            array[idx] = (T) state;
        }
        
    }

    public void Dec(int idx) {
        State s = (State) array[idx];
        s.Decrement();
        array[idx] = (T) s;
    }
}