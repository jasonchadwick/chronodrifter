using System.Collections.Generic;
using UnityEngine;

//TODO: make abstract, then change params for all children
class ActivatedObject : TimeReversibleObject {
    public bool isActive = false;
    public bool defaultActiveStatus = false;
}