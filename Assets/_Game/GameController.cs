using Guidance.Gameplay.BackgroundGrid;
using Guidance.Gameplay.Targets;
using UnityEngine;

namespace Guidance.Gameplay.Game.Controller {
  public class GameController : MonoBehaviour {
    [SerializeField] CameraController m_CameraController;
    [SerializeField] WallBackgroundController m_WallBackgroundController;
    [SerializeField] Ball m_CurrentActiveBall;
    [SerializeField] TargetManager m_TargetManager;
    [SerializeField] PlatformCreator m_PlatformCreator;
    private void Awake() {
      m_TargetManager.OnTargetReached += TargetManager_OnTargetReached;
      m_PlatformCreator.OnPlatformCreated += PlatformCreator_OnPlatformCreated;
    }

    private void OnDisable() {
      m_TargetManager.OnTargetReached -= TargetManager_OnTargetReached;
      m_PlatformCreator.OnPlatformCreated -= PlatformCreator_OnPlatformCreated;
    }

    private void PlatformCreator_OnPlatformCreated() {
      m_CurrentActiveBall.ActivateRigidbody();
      m_TargetManager.DeactivatePreviousGoalTarget();
    }

    private void TargetManager_OnTargetReached() {
      m_WallBackgroundController.ExecuteNewWallAttachmentProcedure();
      TransitionToNextStage();
    }

    private void TransitionToNextStage() {
      StartCoroutine(m_CurrentActiveBall.ShiftBallForNextStage());
      m_TargetManager.ShiftTargetsForNextStage();
      //yield return StartCoroutine(m_WallBackgroundController.ManageWallSectionsAfterAddition());
    }
  }
}
