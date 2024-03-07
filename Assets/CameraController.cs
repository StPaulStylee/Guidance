using System.Collections;
using UnityEngine;

namespace Guidance.Gameplay.Game.Controller {
  public class CameraController : MonoBehaviour {
    private Camera m_Camera;

    private const float CAMERA_SHIFT_MOVE_SPEED = 2f;
    private const float CAMERA_SHIFT_DISTANCE = 11.25f;
    private const float CAMERA_SHIFT_DISTANCE_OFFSET = 0.01f;

    private void Awake() {
      m_Camera = Camera.main;
    }

    public IEnumerator ShiftCameraForNextStage() {
      float distanceToMove = CAMERA_SHIFT_DISTANCE;
      Vector3 currentPosition = m_Camera.transform.position;
      Vector3 targetPosition = new Vector3(currentPosition.x, currentPosition.y - distanceToMove, currentPosition.z);
      float moveSpeed = CAMERA_SHIFT_MOVE_SPEED;
      while (Vector3.Distance(m_Camera.transform.position, targetPosition) > CAMERA_SHIFT_DISTANCE_OFFSET) {
        Vector3 position = Vector3.Lerp(m_Camera.transform.position, targetPosition, Time.deltaTime * moveSpeed);
        m_Camera.transform.position = position;
        yield return null;
      }
      m_Camera.transform.position = targetPosition;
    }
  }
}
