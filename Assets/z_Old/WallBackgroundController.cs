using UnityEngine;

namespace Guidance.Gameplay.BackgroundGrid {
  public class WallBackgroundController : MonoBehaviour {
    private static WallBackgroundController instance;
    private void Awake() {
      if (instance != null) {
        Destroy(gameObject);
        return;
      }
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
  }
}
