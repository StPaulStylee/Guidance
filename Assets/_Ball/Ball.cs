using Guidance.Data;
using System.Collections;
using UnityEngine;

namespace Guidance.Gameplay {
  [RequireComponent(typeof(Rigidbody))]
  public class Ball : MonoBehaviour {
    private Rigidbody rb;
    private readonly float m_YShiftDistance = Constants.Y_STAGE_LENGTH;
    private readonly float m_ShiftSpeed = Constants.STAGE_TRANSITION_EFFECT_SPEED;
    private readonly float m_ShiftDistanceOffset = Constants.STAGE_TRANSITION_DISTANCE_OFFSET;
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

    public IEnumerator ShiftBallForNextStage() {
      DeactivateRigidbody();
      Vector3 currentPosition = transform.position;
      Vector3 targetPosition = new Vector3(currentPosition.x, currentPosition.y + m_YShiftDistance, currentPosition.z);
      while (Vector3.Distance(transform.position, targetPosition) > m_ShiftDistanceOffset) {
        Vector3 position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * m_ShiftSpeed);
        transform.position = position;
        yield return null;
      }
      transform.position = targetPosition;
    }
  }
}
