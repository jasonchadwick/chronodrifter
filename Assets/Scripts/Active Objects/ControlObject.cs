using System.Collections.Generic;
using UnityEngine;

// Control objects send signals to ActivatedObjects
class ControlObject : TimeReversibleObject {
    public List<ActivatedObject> targets;
    public int signalStrength = 1;
    private bool isSendingSignal;

    public void Activate() {
        if (!isSendingSignal) {
            foreach (ActivatedObject tgt in targets) {
                tgt.ApplySignal(signalStrength);
            }
            OnActivate();
            isSendingSignal = true;
        }
    }

    public void Deactivate() {
        if (isSendingSignal) {
            foreach (ActivatedObject tgt in targets) {
                tgt.ApplySignal(-signalStrength);
            }
            OnDeactivate();
            isSendingSignal = false;
        }
    }

    public virtual void OnActivate() {}
    public virtual void OnDeactivate() {}
}