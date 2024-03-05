using Guidance.Gameplay.BackgroundGrid;
using UnityEngine;

namespace Guidance.Gameplay.Game.Controller {
  public class GameController : MonoBehaviour {
    [SerializeField] WallBackgroundController m_WallBackgroundController;
    private void Awake() {
      TargetGoalLocation.OnTargetReached += OnTargetReached;
    }

    private void OnDisable() {
      TargetGoalLocation.OnTargetReached -= OnTargetReached;
    }

    private void OnTargetReached() {
      m_WallBackgroundController.ExecuteNewWallProcedure();
      m_WallBackgroundController.IsCreatingNewWallSection = true;
    }
  }
}
