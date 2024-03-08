using System;
using UnityEngine;

namespace Guidance.Gameplay.Targets {
  public class TargetManager : MonoBehaviour {
    private const float MINIMUM_X_SPAWN_LOCATION = -4.5F;
    private const float MAXIMUM_X_SPAWN_LOCATION = 4.5f;
    private const float Y_SPAWN_DISTANCE = 11.25f;

    public static event Action OnTargetReached;

    [SerializeField] private GameObject m_TargetPrefab;
    private Target m_CurrentActiveTarget;


    private void Awake() {
      m_CurrentActiveTarget = GetComponentInChildren<Target>();
      m_CurrentActiveTarget.GoalLocation.OnTargetReached += TargetGoalLocation_OnTargetReached;
    }

    private void TargetGoalLocation_OnTargetReached() {
      GameObject newTarget = SpawnNewTarget();
      RegisterNewTarget(newTarget);
      OnTargetReached?.Invoke();
    }

    private GameObject SpawnNewTarget() {
      float spawnLocationX = UnityEngine.Random.Range(MINIMUM_X_SPAWN_LOCATION, MAXIMUM_X_SPAWN_LOCATION);
      float spawnLocationY = m_CurrentActiveTarget.transform.position.y - Y_SPAWN_DISTANCE;
      float spawnLocationZ = m_CurrentActiveTarget.transform.position.z;
      Vector3 spawnLocation = new Vector3(spawnLocationX, spawnLocationY, spawnLocationZ);
      GameObject newTarget = Instantiate(m_TargetPrefab, spawnLocation, Quaternion.identity, transform);
      return newTarget;
    }

    private void RegisterNewTarget(GameObject target) {
      m_CurrentActiveTarget.GoalLocation.OnTargetReached -= TargetGoalLocation_OnTargetReached;
      m_CurrentActiveTarget = target.GetComponent<Target>();
      m_CurrentActiveTarget.GoalLocation.OnTargetReached += TargetGoalLocation_OnTargetReached;
    }
  }
}
