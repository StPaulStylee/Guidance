using System.Collections;
using UnityEngine;

namespace _Camera {
  public class CameraController : MonoBehaviour {
    private const float CAMERA_SHIFT_MOVE_SPEED = 2f;
    private const float CAMERA_SHIFT_DISTANCE = 11.25f;
    private const float CAMERA_SHIFT_DISTANCE_OFFSET = 0.01f;
    private Camera _camera;

    private void Awake() {
      _camera = Camera.main;
    }

    public IEnumerator ShiftCameraForNextStage() {
      float distanceToMove = CAMERA_SHIFT_DISTANCE;
      Vector3 currentPosition = _camera.transform.position;
      Vector3 targetPosition = new(currentPosition.x, currentPosition.y - distanceToMove, currentPosition.z);
      float moveSpeed = CAMERA_SHIFT_MOVE_SPEED;
      while (Vector3.Distance(_camera.transform.position, targetPosition) > CAMERA_SHIFT_DISTANCE_OFFSET) {
        Vector3 position = Vector3.Lerp(_camera.transform.position, targetPosition, Time.deltaTime * moveSpeed);
        _camera.transform.position = position;
        yield return null;
      }

      _camera.transform.position = targetPosition;
    }
  }
}
