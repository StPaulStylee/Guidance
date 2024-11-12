using Guidance.Data;
using Guidance.Stage.Data;
using System;
using System.Collections;
using UnityEngine;

namespace Guidance.Gameplay.Game.Manager {
  public static class StageTransitionManager {
    private static readonly float m_YShiftDistance = Constants.Y_STAGE_LENGTH;
    private static readonly float m_ShiftSpeed = Constants.STAGE_TRANSITION_EFFECT_SPEED;
    private static readonly float m_ShiftDistanceOffset = Constants.STAGE_TRANSITION_DISTANCE_OFFSET;

    public static event Action<bool> OnIsStageTransitioning;

    public static IEnumerator ShiftForNextStage(Transform shiftable) {
      Vector3 currentPos = shiftable.transform.position;
      Vector3 targetPosition = new Vector3(currentPos.x, currentPos.y + m_YShiftDistance, currentPos.z);
      OnIsStageTransitioning?.Invoke(true);
      while (Vector3.Distance(shiftable.position, targetPosition) > m_ShiftDistanceOffset) {
        Vector3 position = Vector3.Lerp(shiftable.position, targetPosition, Time.deltaTime * m_ShiftSpeed);
        shiftable.position = position;
        yield return null;
      }
      OnIsStageTransitioning?.Invoke(false);
      shiftable.position = targetPosition;
    }

    public static IEnumerator ShiftToStartLocationForNextStage(Transform shiftable, Position newTargetPosition) {
      Vector3 currentPos = shiftable.transform.position;
      Vector3 targetPosition = new Vector3(newTargetPosition.X, newTargetPosition.Y, newTargetPosition.Z);
      OnIsStageTransitioning?.Invoke(true);
      while (Vector3.Distance(shiftable.position, targetPosition) > m_ShiftDistanceOffset) {
        Vector3 position = Vector3.Lerp(shiftable.position, targetPosition, Time.deltaTime * m_ShiftSpeed);
        shiftable.position = position;
        yield return null;
      }
      OnIsStageTransitioning?.Invoke(false);
      shiftable.position = targetPosition;
    }
  }
}
