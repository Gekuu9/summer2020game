using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InputTriggerObject : InteractableObject {
    void Trigger(int input);
}
