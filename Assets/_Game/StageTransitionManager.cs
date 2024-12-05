using System;
using System.Collections;
using _Data;
using UnityEngine;

namespace _Game {
  public static class StageTransitionManager {
    private const float Y_SHIFT_DISTANCE = Constants.Y_STAGE_LENGTH;
    private const float SHIFT_SPEED = Constants.STAGE_TRANSITION_EFFECT_SPEED;
    private const float SHIFT_DISTANCE_OFFSET = Constants.STAGE_TRANSITION_DISTANCE_OFFSET;

    public static event Action<bool> OnIsStageTransitioning;

    public static IEnumerator ShiftForNextStage(Transform shiftable) {
      Vector3 currentPos = shiftable.transform.position;
      Vector3 targetPosition = new(currentPos.x, currentPos.y + Y_SHIFT_DISTANCE, currentPos.z);
      OnIsStageTransitioning?.Invoke(true);
      while (Vector3.Distance(shiftable.position, targetPosition) > SHIFT_DISTANCE_OFFSET) {
        Vector3 position = Vector3.Lerp(shiftable.position, targetPosition, Time.deltaTime * SHIFT_SPEED);
        shiftable.position = position;
        yield return null;
      }

      OnIsStageTransitioning?.Invoke(false);
      shiftable.position = targetPosition;
    }

    public static IEnumerator ShiftToStartLocationForNextStage(Transform shiftable, Position newTargetPosition) {
      Vector3 currentPos = shiftable.transform.position;
      Vector3 targetPosition = new(newTargetPosition.X, newTargetPosition.Y, newTargetPosition.Z);
      OnIsStageTransitioning?.Invoke(true);
      while (Vector3.Distance(shiftable.position, targetPosition) > SHIFT_DISTANCE_OFFSET) {
        Vector3 position = Vector3.Lerp(shiftable.position, targetPosition, Time.deltaTime * SHIFT_SPEED);
        shiftable.position = position;
        yield return null;
      }

      OnIsStageTransitioning?.Invoke(false);
      shiftable.position = targetPosition;
    }
  }
}
