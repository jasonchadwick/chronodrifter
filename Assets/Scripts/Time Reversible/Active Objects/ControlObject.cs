using System.Collections.Generic;
using UnityEngine;

/* General class for a control object such as a button.
   Control objects send signals to ActivatedObjects.
   Signal strength can be adjusted.
*/

class ControlObject<T> : TimeReversibleObject<T> {
    public List<Object> targets;
    private List<ActivatedObject<DefaultState>> defaultTargets;
    private List<ActivatedObject<FloatState>> floatTargets;
    private List<ActivatedObject<PositionState>> positionTargets;
    public int signalStrength = 1;
    private bool isSendingSignal;

    public void Activate() {
        if (!isSendingSignal) {
            foreach (Object tgt in targets) {
                if (tgt is ActivatedObject<DefaultState>) {
                    (tgt as ActivatedObject<DefaultState>).ApplySignal(signalStrength);
                }
                else if (tgt is ActivatedObject<FloatState>) {
                    (tgt as ActivatedObject<FloatState>).ApplySignal(signalStrength);
                }
                else if (tgt is ActivatedObject<PositionState>) {
                    (tgt as ActivatedObject<PositionState>).ApplySignal(signalStrength);
                }
                else {
                    Debug.Log("bad target!");
                }
            }
            OnActivate();
            isSendingSignal = true;
        }
    }

    public void Deactivate() {
        if (isSendingSignal) {
            foreach (Object tgt in targets) {
                if (tgt is ActivatedObject<DefaultState>) {
                    (tgt as ActivatedObject<DefaultState>).ApplySignal(-signalStrength);
                }
                else if (tgt is ActivatedObject<FloatState>) {
                    (tgt as ActivatedObject<FloatState>).ApplySignal(-signalStrength);
                }
                else if (tgt is ActivatedObject<PositionState>) {
                    (tgt as ActivatedObject<PositionState>).ApplySignal(-signalStrength);
                }
                else {
                    Debug.Log("bad target!");
                }
            }
            OnDeactivate();
            isSendingSignal = false;
        }
    }
}