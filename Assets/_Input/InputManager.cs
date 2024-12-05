using System;
using UnityEngine;

namespace _Input {
  public class InputManager : MonoBehaviour {
    // Can use this class if/when I switch to new Input System
    private void Update() {
      if (Input.GetKeyDown(KeyCode.R)) OnReloadStage?.Invoke();
    }

    public static event Action OnReloadStage;
  }
}
