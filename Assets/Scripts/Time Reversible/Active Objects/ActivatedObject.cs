using System.Collections.Generic;
using UnityEngine;

// An object that is either active or inactive based on how many
// control objects are currently activating it.
class ActivatedObject<T> : TimeReversibleObject<T> {
    public int incomingSignalSum = 0;
    public int activationThreshold = 1;
    public bool defaultActiveStatus = false;

    public void ApplySignal(int signalStrength) {
        incomingSignalSum += signalStrength;
        if (incomingSignalSum - signalStrength < activationThreshold
            && incomingSignalSum >= activationThreshold) {
            Activate();
        }
        else if (incomingSignalSum - signalStrength >= activationThreshold 
                 && incomingSignalSum < activationThreshold) {
            Deactivate();
        }
    }

    public bool IsActive() {
        if (incomingSignalSum >= activationThreshold) {
            return !defaultActiveStatus;
        }
        else {
            return defaultActiveStatus;
        }
    }

    public virtual void Activate() {}
    public virtual void Deactivate() {}
}

interface ActivatedObject1 {
    public void ApplySignal(int signalStrength);
    public bool IsActive();
}