using Guidance.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guidance.Gameplay.Targets {
  public class TargetManager : MonoBehaviour {
    private const float MINIMUM_X_SPAWN_LOCATION = -4.5F;
    private const float MAXIMUM_X_SPAWN_LOCATION = 4.5f;

    private readonly float m_YSpawnDistance = Constants.Y_STAGE_LENGTH;
    private readonly float m_TargetShiftMoveSpeed = Constants.STAGE_TRANSITION_EFFECT_SPEED;
    private readonly float m_TargetShiftDistanceOffset = Constants.STAGE_TRANSITION_DISTANCE_OFFSET;

    public event Action OnTargetReached;

    [SerializeField] private GameObject m_TargetPrefab;
    [SerializeField] private Target m_CurrentGoalTarget;
    [SerializeField] private Target m_PreviousGoalTarget = null;

    private List<Target> m_Targets;

    private void Awake() {
      m_Targets = new();
      GameObject initialTarget = SpawnNewTarget(-2.97f, -5.5f, -0.35f);
      RegisterNewTarget(initialTarget);
    }

    public void DeactivatePreviousGoalTarget() {
      if (m_Targets.Count < 2) {
        return;
      }
      if (!m_Targets[^2].GoalLocation.IsMeshColliderEnabled) {
        return;
      }
      m_Targets[^2].GoalLocation.DisableMeshCollider();
    }

    public void ShiftTargetsForNextStage() {
      foreach (Target target in m_Targets) {
        StartCoroutine(ShiftTargetForNextStage(target));
      }
    }

    private IEnumerator ShiftTargetForNextStage(Target target) {
      Vector3 currentPos = target.transform.position;
      Vector3 targetPosition = new Vector3(currentPos.x, currentPos.y + m_YSpawnDistance, currentPos.z);
      while (Vector3.Distance(target.transform.position, targetPosition) > m_TargetShiftDistanceOffset) {
        Vector3 position = Vector3.Lerp(target.transform.position, targetPosition, Time.deltaTime * m_TargetShiftMoveSpeed);
        target.transform.position = position;
        yield return null;
      }
      target.transform.position = targetPosition;
    }

    private void TargetGoalLocation_OnTargetReached() {
      GameObject newTarget = SpawnNewTarget();
      RegisterNewTarget(newTarget);
      OnTargetReached?.Invoke();
    }

    private GameObject SpawnNewTarget() {
      Vector3 seedLocation = m_Targets[m_Targets.Count - 1].transform.position;
      float spawnLocationX = UnityEngine.Random.Range(MINIMUM_X_SPAWN_LOCATION, MAXIMUM_X_SPAWN_LOCATION);
      float spawnLocationY = seedLocation.y - m_YSpawnDistance;
      float spawnLocationZ = seedLocation.z;
      Vector3 spawnLocation = new Vector3(spawnLocationX, spawnLocationY, spawnLocationZ);
      GameObject newTarget = Instantiate(m_TargetPrefab, spawnLocation, Quaternion.identity, transform);
      return newTarget;
    }

    private GameObject SpawnNewTarget(float xPos, float yPos, float zPos) {
      float spawnLocationX = UnityEngine.Random.Range(MINIMUM_X_SPAWN_LOCATION, MAXIMUM_X_SPAWN_LOCATION);
      Vector3 spawnLocation = new Vector3(xPos, yPos, zPos);
      GameObject newTarget = Instantiate(m_TargetPrefab, spawnLocation, Quaternion.identity, transform);
      return newTarget;
    }

    private void RegisterNewTarget(GameObject target) {
      if (m_Targets.Count > 0) {
        Target previousTarget = m_Targets[m_Targets.Count - 1];
        previousTarget.GoalLocation.OnTargetReached -= TargetGoalLocation_OnTargetReached;
      }
      Target targetComponent = target.GetComponent<Target>();
      targetComponent.GoalLocation.OnTargetReached += TargetGoalLocation_OnTargetReached;
      m_Targets.Add(targetComponent); ;
    }
  }
}
