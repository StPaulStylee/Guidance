using Guidance.Gameplay.Game.Manager;
using UnityEngine;

namespace Guidance.Gameplay {
  [RequireComponent(typeof(Rigidbody))]
  public class Ball : MonoBehaviour, IStageTransition {
    private Rigidbody rb;

    private void Awake() {
      rb = GetComponent<Rigidbody>();
      rb.isKinematic = true;
    }

    public void ActivateRigidbody() {
      if (rb.isKinematic == false) {
        return;
      }
      rb.isKinematic = false;
    }

    public void DeactivateRigidbody() {
      if (rb.isKinematic) {
        return;
      }
      rb.isKinematic = true;
    }

    public void ToggleRigidbody() {
      rb.isKinematic = !rb.isKinematic;
    }

    public void ShiftForStageTransition() {
      DeactivateRigidbody();
      StartCoroutine(StageTransitionManager.ShiftForNextStage(transform));
    }
  }
}
