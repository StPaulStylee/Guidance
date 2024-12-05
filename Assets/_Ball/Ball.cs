using System.Collections;
using _Data;
using _Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Ball {
  [RequireComponent(typeof(Rigidbody))]
  public class Ball : MonoBehaviour, IStageTransition {
    // This is used for debugging. Maybe it can be reused, but I can't remember how it works rn
    [FormerlySerializedAs("m_StartingPosition")] [SerializeField]
    private Vector3 startingPosition;

    private Vector3 _restartPosition; // This is used to set the position on fail/reload
    public Rigidbody Rb { get; private set; }
    public Collider Collider { get; private set; }
    public Material BallMaterial { get; private set; }
    public PathTraveledRenderer PathTraveledRenderer { get; private set; }

    private void Awake() {
      Rb = GetComponent<Rigidbody>();
      Collider = GetComponent<Collider>();
      Rb.isKinematic = true;
      BallMaterial = GetComponent<Renderer>().material;
      PathTraveledRenderer = GetComponent<PathTraveledRenderer>();
    }

    private void OnEnable() {
      StageTransitionManager.OnIsStageTransitioning += SetRestartPosition;
    }

    private void OnDisable() {
      StageTransitionManager.OnIsStageTransitioning -= SetRestartPosition;
    }

    public void ShiftForStageTransition() {
      DeactivateRigidbody();
      Vector3 position = transform.position;
      Position previousTargetLocation = new()
        { X = position.x, Y = Constants.TARGET_LOCATION_Y_FINAL_LOCATION - Collider.bounds.size.y / 2, Z = position.z };
      StartCoroutine(StageTransitionManager.ShiftToStartLocationForNextStage(transform, previousTargetLocation));
      StartCoroutine(ManagePathTraveledRendererOnStageTransition(previousTargetLocation));
      // Capture the ball position after this shift has occurred then it can be used to reset the ball position
      // on a fail
    }

    public void ActivateRigidbody() {
      if (Rb.isKinematic == false) {
        return;
      }

      Rb.isKinematic = false;
    }

    private void DeactivateRigidbody() {
      if (Rb.isKinematic) {
        return;
      }

      Rb.isKinematic = true;
    }

    public void ToggleRigidbody() {
      Rb.isKinematic = !Rb.isKinematic;
    }

    public void SetBallPosition(Vector3 position) {
      transform.position = position;
    }

    private void ResetBallPositionToStartOfStage() {
      transform.position = _restartPosition;
    }

    public void ResetBallToStartOfStageProcedure() {
      DeactivateRigidbody();
      ResetBallPositionToStartOfStage();

      Rb.useGravity = true;
    }

    public void ResetBallPosition() {
      transform.position = startingPosition;
    }

    private void SetRestartPosition(bool isBallTransitioning) {
      if (isBallTransitioning) {
        return;
      }

      _restartPosition = transform.position;
    }

    private IEnumerator ManagePathTraveledRendererOnStageTransition(Position targetPosition) {
      Debug.Log("Disabling PathTraveledRenderer");
      PathTraveledRenderer.DisableDataCapture();
      float enablePosition = targetPosition.Y - PathTraveledRenderer.PositionTolerance;
      while (transform.position.y < enablePosition) yield return null;
      Debug.Log("Enabling PathTraveledRendered");
      PathTraveledRenderer.ClearDataCapture();
      PathTraveledRenderer.EnableDataCapture();
    }
  }
}
