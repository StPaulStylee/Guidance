using Guidance.Data;
using Guidance.Gameplay.Game.Manager;
using Guidance.Stage.Data;
using System.Collections;
using UnityEngine;

namespace Guidance.Gameplay {
  [RequireComponent(typeof(Rigidbody))]
  public class Ball : MonoBehaviour, IStageTransition {
    public Rigidbody Rb { get; private set; }
    public Collider Collider { get; private set; }
    public Material BallMaterial { get; private set; }
    [SerializeField] private Vector3 m_StartingPosition; // This is used for debugging. Maybe it can be reused but I can't remember how it works rn
    private Vector3 m_RestartPosition; // This is used to set the position on fail/reload
    private PathTraveledRenderer m_PathTraveledRendered;
    private void Awake() {
      Rb = GetComponent<Rigidbody>();
      Collider = GetComponent<Collider>();
      Rb.isKinematic = true;
      BallMaterial = GetComponent<Renderer>().material;
      m_PathTraveledRendered = GetComponent<PathTraveledRenderer>();
    }

    private void OnEnable() {
      StageTransitionManager.OnStageTransition += SetRestartPosition;
    }

    private void OnDisable() {
      StageTransitionManager.OnStageTransition -= SetRestartPosition;
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
      StartCoroutine(ManagePathTraveledRenderer(previousTargetLocation));
      // Capture the ball position after this shift has occurred then it can be used to reset the ball position
      // on a fail
    }

    public void SetBallPosition(Vector3 position) {
      transform.position = position;
    }

    public void ResetBallPositionToStartOfStage() => transform.position = m_RestartPosition;

    public void ResetBallToStartOfStageProcedure() {
      DeactivateRigidbody();
      ResetBallPositionToStartOfStage();
      Rb.useGravity = true;
    }

    public void ResetBallPosition() => transform.position = m_StartingPosition;
    private void SetRestartPosition(bool isBallTransitioning) {
      if (isBallTransitioning) {
        return;
      }
      m_RestartPosition = transform.position;
    }

    private IEnumerator ManagePathTraveledRenderer(Position targetPosition) {
      Debug.Log("Disabling PathTraveledRenderer");
      float enablePosition = targetPosition.Y - m_PathTraveledRendered.PositionTolerance;
      while (transform.position.y < enablePosition) {
        yield return null;
      }
      Debug.Log("Enabling PathTraveledRendered");
    }
  }

}
