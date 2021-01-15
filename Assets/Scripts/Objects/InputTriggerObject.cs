using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InputTriggerObject : TriggerableObject {
    void Trigger(int input);
}
