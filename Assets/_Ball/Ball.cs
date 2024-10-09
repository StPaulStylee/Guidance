using Guidance.Data;
using Guidance.Gameplay.Game.Manager;
using Guidance.Stage.Data;
using UnityEngine;

namespace Guidance.Gameplay {
  [RequireComponent(typeof(Rigidbody))]
  public class Ball : MonoBehaviour, IStageTransition {
    public Rigidbody Rb { get; private set; }
    public Collider Collider { get; private set; }
    public Material BallMaterial { get; private set; }
    [SerializeField] private Vector3 m_StartingPosition;

    private void Awake() {
      Rb = GetComponent<Rigidbody>();
      Collider = GetComponent<Collider>();
      Rb.isKinematic = true;
      BallMaterial = GetComponent<Renderer>().material;
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
      Vector3 position = transform.position;
      Position previousTargetLocation = new Position { X = position.x, Y = Constants.TARGET_LOCATION_Y_FINAL_LOCATION, Z = position.z };
      StartCoroutine(StageTransitionManager.ShiftToStartLocationForNextStage(transform, previousTargetLocation));
    }

    public void SetBallPosition(Vector3 position) {
      transform.position = position;
    }

    public void ResetBallPosition() => transform.position = m_StartingPosition;
  }
}
