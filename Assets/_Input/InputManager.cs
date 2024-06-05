
using System;
using UnityEngine;

namespace Guidance.Gameplay.Inputs {
  public class InputManager : MonoBehaviour {
    public static event Action OnReloadStage;
    // Can use this class if/when I switch to new Input System
    void Update() {
      if (Input.GetKeyDown(KeyCode.R)) {
        OnReloadStage?.Invoke();
      }
    }
  }
}
