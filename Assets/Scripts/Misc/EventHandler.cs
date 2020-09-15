using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour {

    public static EventHandler instance;

    private void Awake() {
        instance = this;
    }

    public event Action suppressInput;
    public void SuppressInput() {
        suppressInput?.Invoke();
    }

    public event Action handleInput;
    public void HandleInput() {
        handleInput?.Invoke();
    }

}
