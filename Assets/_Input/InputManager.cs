using UnityEngine;
using UnityEngine.SceneManagement;

namespace Guidance.Gameplay.Inputs {
  public class InputManager : MonoBehaviour {
    // Can use this class if/when I switch to new Input System
    void Update() {
      if (Input.GetKeyDown(KeyCode.R)) {
        SceneManager.LoadSceneAsync(0);
      }
    }
  }
}
