using Guidance.Gameplay.BackgroundGrid;
using Guidance.Gameplay.Game.Manager;
using Guidance.Gameplay.Stage;
using Guidance.Stage;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Guidance.Gameplay.Game.Controller {
  public class GameController : MonoBehaviour {
    [Header("Dependencies")]
    [SerializeField] private WallBackgroundController m_WallBackgroundController;
    [SerializeField] private Ball m_CurrentActiveBall;
    public Ball CurrentActiveBall { get { return m_CurrentActiveBall; } }
    [SerializeField] private StageManager m_StageManager;
    [SerializeField] private PlatformCreator m_PlatformCreator;
    [SerializeField] private StageViewer m_StageViewer;

    [Header("StageData")]
    [SerializeField][ReadOnly] private int m_StageNumber;
    public int StageNumber {
      set {
        m_StageNumber = value;
      }
    }
    private int m_NextStageNumber { get { return m_StageNumber + 1; } }

    [Header("Stage Testing")]
    [ReadOnly] public bool IsStageDebug;
    [ReadOnly] public bool IsStageEdit;
    [ReadOnly] public Vector3 BallPosition;

    private void Awake() {
      m_StageViewer.gameObject.SetActive(false);
    }

    private void OnEnable() {
      if (IsStageDebug) {
        m_CurrentActiveBall.SetBallPosition(BallPosition);
      }
      m_StageManager.OnTargetReached += TargetManager_OnTargetReached;
      m_PlatformCreator.OnPlatformCreated += PlatformCreator_OnPlatformCreated;
      StageTransitionManager.OnStageTransition += StageTransitionManager_OnStageTransition;
    }

    private void OnDisable() {
      m_StageManager.OnTargetReached -= TargetManager_OnTargetReached;
      m_PlatformCreator.OnPlatformCreated -= PlatformCreator_OnPlatformCreated;
      StageTransitionManager.OnStageTransition -= StageTransitionManager_OnStageTransition;
    }

    private void Start() {
      if (IsStageDebug) {
        m_CurrentActiveBall.ShiftForStageTransition();
        m_StageManager.SpawnNextStage(m_StageNumber);
        m_StageManager.ShiftForStageTransition();
        m_PlatformCreator.ShiftForStageTransition();
        return;
      }
      m_StageManager.SpawnNextStage(m_StageNumber);
    }

    public void ResetBallPosition() {
      m_CurrentActiveBall.ResetBallPosition();
    }

    private void StageTransitionManager_OnStageTransition(bool isTransitioning) {
      m_PlatformCreator.IsEnabled = !isTransitioning;
    }

    private void PlatformCreator_OnPlatformCreated() {
      m_CurrentActiveBall.ActivateRigidbody();
      m_StageManager.DeactivatePreviousGoalTarget();
    }

    private void TargetManager_OnTargetReached() {
      bool hasNextStage = m_StageManager.SpawnNextStage(m_NextStageNumber);
      if (hasNextStage) {
        TransitionToNextStage();
      }
    }

    private void TransitionToNextStage() {
      m_StageNumber++;
      m_CurrentActiveBall.ShiftForStageTransition();
      m_StageManager.ShiftForStageTransition();
      m_PlatformCreator.ShiftForStageTransition();
    }
  }
}
