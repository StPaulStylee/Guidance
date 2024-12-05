using System.Collections;
using _Ball;
using _Input;
using _Obstacle.Obstacle_White;
using _PlatformCreator;
using _Stage;
using _TitleScreen;
using Guidance.Stage;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game {
  public class GameController : MonoBehaviour {
    [FormerlySerializedAs("m_CurrentActiveBall")]
    [Header("Dependencies")]
    [SerializeField]
    private Ball currentActiveBall;

    [FormerlySerializedAs("m_StageManager")] [SerializeField]
    private StageManager stageManager;

    [FormerlySerializedAs("m_PlatformCreator")] [SerializeField]
    private PlatformCreator platformCreator;

    [FormerlySerializedAs("m_StageViewer")] [SerializeField]
    private StageViewer stageViewer;

    [FormerlySerializedAs("m_TitleSceneController")] [SerializeField]
    private TitleSceneController titleSceneController;

    [FormerlySerializedAs("m_InputManager")] [SerializeField]
    private InputManager inputManager;

    [FormerlySerializedAs("m_TitleSceneObjects")] [SerializeField]
    private GameObject titleSceneObjects;

    [FormerlySerializedAs("m_StageNumber")]
    [Header("StageData")]
    [SerializeField] [ReadOnly]
    private int stageNumber;

    [Header("Stage Testing")]
    [ReadOnly] public bool IsStageDebug;

    [ReadOnly] public bool IsStageEdit;
    [ReadOnly] public Vector3 BallPosition;
    public Ball CurrentActiveBall => currentActiveBall;

    public int StageNumber {
      set => stageNumber = value;
    }

    private int NextStageNumber => stageNumber + 1;

    private void Awake() {
      stageViewer.gameObject.SetActive(false);
    }

    private void Start() {
      if (IsStageDebug) {
        currentActiveBall.ShiftForStageTransition();
        stageManager.SpawnNextStage(stageNumber);
        stageManager.ShiftForStageTransition();
        platformCreator.ShiftForStageTransition();
        return;
      }

      stageManager.SpawnNextStage(stageNumber);
      currentActiveBall.PathTraveledRenderer.ClearDataCapture();
      currentActiveBall.PathTraveledRenderer.EnableDataCapture();
    }

    private void OnEnable() {
      if (IsStageDebug) {
        currentActiveBall.SetBallPosition(BallPosition);
      }

      InputManager.OnReloadStage += InputManager_OnReloadStage;
      stageManager.OnTargetReached += TargetManager_OnTargetReached;
      platformCreator.OnPlatformCreated += PlatformCreator_OnPlatformCreated;
      StageTransitionManager.OnIsStageTransitioning += StageTransitionManager_OnStageTransition;
      titleSceneController.OnTitleSceneEnd += ActivateInGameDependencies;
      WhiteObstacle.OnWhiteObstacleCollision += WhiteObstacle_OnCollision;
    }

    private void OnDisable() {
      InputManager.OnReloadStage -= InputManager_OnReloadStage;
      stageManager.OnTargetReached -= TargetManager_OnTargetReached;
      platformCreator.OnPlatformCreated -= PlatformCreator_OnPlatformCreated;
      StageTransitionManager.OnIsStageTransitioning -= StageTransitionManager_OnStageTransition;
      titleSceneController.OnTitleSceneEnd -= ActivateInGameDependencies;
      WhiteObstacle.OnWhiteObstacleCollision -= WhiteObstacle_OnCollision;
    }

    private void InputManager_OnReloadStage() {
      currentActiveBall.ResetBallToStartOfStageProcedure();
      //m_StageManager.SpawnNextStage(m_StageNumber);
      //m_CurrentActiveBall.ShiftForStageTransition();
      //m_StageManager.ShiftForStageTransition();
      //m_PlatformCreator.ShiftForStageTransition();
    }

    public void ResetBallPosition() {
      currentActiveBall.ResetBallPosition();
    }

    private void ActivateInGameDependencies() {
      platformCreator.enabled = true;
      inputManager.enabled = true;
      currentActiveBall.PathTraveledRenderer.enabled = true;
      titleSceneController.enabled = false;
      Destroy(titleSceneObjects);
    }

    private void StageTransitionManager_OnStageTransition(bool isTransitioning) {
      platformCreator.IsEnabled = !isTransitioning;
    }

    private void PlatformCreator_OnPlatformCreated() {
      currentActiveBall.ActivateRigidbody();
      stageManager.DeactivatePreviousGoalTarget();
    }

    private void TargetManager_OnTargetReached() {
      bool hasNextStage = stageManager.SpawnNextStage(NextStageNumber);
      if (hasNextStage) {
        StartCoroutine(TransitionToNextStage());
      }
    }

    private IEnumerator TransitionToNextStage() {
      yield return StartCoroutine(currentActiveBall.PathTraveledRenderer.DrawPathTraveledOverDuration());
      stageNumber++;
      currentActiveBall.ShiftForStageTransition();
      stageManager.ShiftForStageTransition();
      platformCreator.ShiftForStageTransition();
    }

    private void WhiteObstacle_OnCollision() {
      StartCoroutine(ResetBallToStartOfStage());
    }

    private IEnumerator ResetBallToStartOfStage() {
      currentActiveBall.PathTraveledRenderer.DisableDataCapture();
      yield return StartCoroutine(BallDissolveManager.PerformVerticalDissolveDown(currentActiveBall.BallMaterial));
      yield return StartCoroutine(platformCreator.ShiftForStageRestart());
      yield return new WaitForSeconds(1f);
      currentActiveBall.ResetBallToStartOfStageProcedure();
      yield return StartCoroutine(BallDissolveManager.PerformVerticalDissolveUp(currentActiveBall.BallMaterial));
      currentActiveBall.PathTraveledRenderer.ClearDataCapture();
      currentActiveBall.PathTraveledRenderer.EnableDataCapture();
    }
  }
}
