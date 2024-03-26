using Guidance.Gameplay.BackgroundGrid;
using Guidance.Gameplay.Game.Manager;
using Guidance.Gameplay.Targets;
using UnityEngine;

namespace Guidance.Gameplay.Game.Controller {
  public class GameController : MonoBehaviour {
    [SerializeField] CameraController m_CameraController;
    [SerializeField] WallBackgroundController m_WallBackgroundController;
    [SerializeField] Ball m_CurrentActiveBall;
    [SerializeField] TargetManager m_TargetManager;
    [SerializeField] PlatformCreator m_PlatformCreator;
    private int m_StageNumber;
    private int m_NextStageNumber { get { return m_StageNumber + 1; } }

    private void Awake() {
      m_StageNumber = 0;
    }

    private void OnEnable() {
      m_TargetManager.OnTargetReached += TargetManager_OnTargetReached;
      m_PlatformCreator.OnPlatformCreated += PlatformCreator_OnPlatformCreated;
      StageTransitionManager.OnStageTransition += StageTransitionManager_OnStageTransition;
    }

    private void OnDisable() {
      m_TargetManager.OnTargetReached -= TargetManager_OnTargetReached;
      m_PlatformCreator.OnPlatformCreated -= PlatformCreator_OnPlatformCreated;
      StageTransitionManager.OnStageTransition -= StageTransitionManager_OnStageTransition;
    }

    private void Start() {
      m_TargetManager.SpawnTargetForStage(m_StageNumber);
    }

    private void StageTransitionManager_OnStageTransition(bool isTransitioning) {
      m_PlatformCreator.IsEnabled = !isTransitioning;
    }

    private void PlatformCreator_OnPlatformCreated() {
      m_CurrentActiveBall.ActivateRigidbody();
      m_TargetManager.DeactivatePreviousGoalTarget();
    }

    private void TargetManager_OnTargetReached() {
      //m_WallBackgroundController.ExecuteNewWallAttachmentProcedure();
      m_TargetManager.SpawnTargetForStage(m_NextStageNumber);
      TransitionToNextStage();
    }

    private void TransitionToNextStage() {
      m_StageNumber++;
      m_CurrentActiveBall.ShiftForStageTransition();
      m_TargetManager.ShiftForStageTransition();
      m_PlatformCreator.ShiftForStageTransition();
    }
  }
}
