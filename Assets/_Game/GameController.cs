using Guidance.Gameplay.BackgroundGrid;
using Guidance.Gameplay.Targets;
using System.Collections;
using UnityEngine;

namespace Guidance.Gameplay.Game.Controller {
  public class GameController : MonoBehaviour {
    [SerializeField] CameraController m_CameraController;
    //[SerializeField] Tar
    [SerializeField] WallBackgroundController m_WallBackgroundController;
    private void Awake() {
      TargetManager.OnTargetReached += OnTargetReached;
    }

    private void OnDisable() {
      TargetManager.OnTargetReached -= OnTargetReached;
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
