using Guidance.Gameplay.Game.Manager;
using UnityEngine;

namespace Guidance.Gameplay {
  [RequireComponent(typeof(Rigidbody))]
  public class Ball : MonoBehaviour, IStageTransition {
    public Rigidbody Rb { get; private set; }
    public Collider Collider { get; private set; }
    [SerializeField] private Vector3 m_StartingPosition;

    private void Awake() {
      Rb = GetComponent<Rigidbody>();
      Collider = GetComponent<Collider>();
      Rb.isKinematic = true;
    }

    public void ActivateRigidbody() {
      if (Rb.isKinematic == false) {
        return;
      }
      Rb.isKinematic = false;
    }

    public void DeactivateRigidbody() {
      if (Rb.isKinematic) {
        return;
      }
      Rb.isKinematic = true;
    }

    public void ToggleRigidbody() {
      Rb.isKinematic = !Rb.isKinematic;
    }

    public void ShiftForStageTransition() {
      DeactivateRigidbody();
      StartCoroutine(StageTransitionManager.ShiftForNextStage(transform));
    }

    public void SetBallPosition(Vector3 position) {
      transform.position = position;
    }

    public void ResetBallPosition() => transform.position = m_StartingPosition;
  }
}
