using Guidance.Gameplay.BackgroundGrid;
using System.Collections;
using UnityEngine;

namespace Guidance.Gameplay.Game.Controller {
  public class GameController : MonoBehaviour {
    [SerializeField] WallBackgroundController m_WallBackgroundController;
    [SerializeField] CameraController m_CameraController;
    private void Awake() {
      TargetGoalLocation.OnTargetReached += OnTargetReached;
    }

    private void OnDisable() {
      TargetGoalLocation.OnTargetReached -= OnTargetReached;
    }

    private void OnTargetReached() {
      m_WallBackgroundController.ExecuteNewWallAttachmentProcedure();
      StartCoroutine(TransitionToNextStage());
    }

    private IEnumerator TransitionToNextStage() {
      yield return StartCoroutine(m_CameraController.ShiftCameraForNextStage());
      yield return StartCoroutine(m_WallBackgroundController.ManageWallSectionsAfterAddition());
    }
  }
}
