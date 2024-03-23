using Guidance.Data;
using Guidance.Gameplay.Game.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Guidance.Gameplay.Targets {
  public class TargetManager : MonoBehaviour, IStageTransition {
    private const float MINIMUM_X_SPAWN_LOCATION = -4.5F;
    private const float MAXIMUM_X_SPAWN_LOCATION = 4.5f;

    private readonly float m_YSpawnDistance = Constants.Y_STAGE_LENGTH;

    public event Action OnTargetReached;

    [SerializeField] private GameObject m_TargetPrefab;
    [SerializeField] private Target m_CurrentGoalTarget;
    [SerializeField] private Target m_PreviousGoalTarget = null;

    private List<Target> m_Targets;

    private void Awake() {
      m_Targets = new();
      GameObject initialTarget = SpawnNewTarget(-2.97f, -5.5f, 0f);
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

    public void ShiftForStageTransition() {
      foreach (Target target in m_Targets) {
        StartCoroutine(StageTransitionManager.ShiftForNextStage(target.transform));
      }
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
      Vector3 spawnLocation = new Vector3(spawnLocationX, yPos, zPos);
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
