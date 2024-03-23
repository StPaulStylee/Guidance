using Guidance.Data;
using System.Collections;
using UnityEngine;

namespace Guidance.Gameplay.Game.Manager {
  public static class StageTransitionManager {
    private static readonly float m_YShiftDistance = Constants.Y_STAGE_LENGTH;
    private static readonly float m_ShiftSpeed = Constants.STAGE_TRANSITION_EFFECT_SPEED;
    private static readonly float m_ShiftDistanceOffset = Constants.STAGE_TRANSITION_DISTANCE_OFFSET;

    public static IEnumerator ShiftForNextStage(Transform shiftable) {
      Vector3 currentPos = shiftable.transform.position;
      Vector3 targetPosition = new Vector3(currentPos.x, currentPos.y + m_YShiftDistance, currentPos.z);
      while (Vector3.Distance(shiftable.position, targetPosition) > m_ShiftDistanceOffset) {
        Vector3 position = Vector3.Lerp(shiftable.position, targetPosition, Time.deltaTime * m_ShiftSpeed);
        shiftable.position = position;
        yield return null;
      }
      shiftable.position = targetPosition;
    }


  }

}
