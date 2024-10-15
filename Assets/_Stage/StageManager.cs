using Guidance.Data;
using Guidance.Gameplay.Game.Manager;
using Guidance.Gameplay.Obstacles;
using Guidance.Stage;
using Guidance.Stage.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Guidance.Gameplay.Stage {
  public class StageManager : MonoBehaviour, IStageTransition {

    public event Action OnTargetReached;

    [SerializeField] private GameObject m_TargetPrefab;
    [SerializeField] private List<Obstacle> m_ObstacleCompnentTypes;
    public List<Obstacle> ObstacleComponentTypes {
      get {
        return m_ObstacleCompnentTypes;
      }
    }
    private Target m_PreviousTarget;
    private Target m_CurrentTarget;
    private List<Target> m_InactiveTargets;
    private List<Obstacle> m_Obstacles;

    private GameObject m_CurrentStage;
    private StageData[] m_StageData;
    private int m_StageNumber;

    private void Awake() {
      m_InactiveTargets = new();
      m_Obstacles = new();
      m_StageData = Utilities.GetStageData();
    }

    public void DeactivatePreviousGoalTarget() {
      if (m_PreviousTarget == null) {
        return;
      }
      if (!m_PreviousTarget.GoalLocation.IsMeshColliderEnabled) {
        return;
      }
      m_PreviousTarget.GoalLocation.DisableMeshCollider();
    }

    public void ShiftForStageTransition() {
      if (m_PreviousTarget != null) {
        Vector3 position = m_PreviousTarget.transform.position;
        Position previousTargetLocation = new Position { X = position.x, Y = Constants.TARGET_LOCATION_Y_FINAL_LOCATION, Z = position.z };
        StartCoroutine(StageTransitionManager.ShiftToStartLocationForNextStage(m_PreviousTarget.transform, previousTargetLocation));
      }
      Position newTargetLocation = m_StageData[m_StageNumber].TargetLocation;
      StartCoroutine(StageTransitionManager.ShiftToStartLocationForNextStage(m_CurrentTarget.transform, newTargetLocation));
      foreach (Target target in m_InactiveTargets) {
        StartCoroutine(StageTransitionManager.ShiftForNextStage(target.transform));
      }
      foreach (Obstacle obstacle in m_Obstacles) {
        StartCoroutine(StageTransitionManager.ShiftForNextStage(obstacle.transform));
      }
    }

    public bool SpawnNextStage(int stageNumber) {
      m_StageNumber = stageNumber;
      m_CurrentStage = new GameObject($"Stage_{m_StageNumber}");
      m_CurrentStage.transform.SetParent(transform);
      if (m_StageNumber < 0 || m_StageNumber >= m_StageData?.Length) {
        Debug.LogError("Cannot access stage data for spawning a target because the index does not exist");
        return false;
      }
      StageData stageData = m_StageData[m_StageNumber];
      GameObject target = SpawnNewTarget(stageData.TargetLocation);
      RegisterNewTarget(target);
      SpawnObstacles(stageData.Obstacles);
      return true;
    }


    private void TargetGoalLocation_OnTargetReached() {
      OnTargetReached?.Invoke();
    }

    private void SpawnObstacles(ObstacleData[] obstacles) {
      if (obstacles.Length == 0) {
        return;
      }
      foreach (ObstacleData obstacle in obstacles) {
        Obstacle obstacleComponent = m_ObstacleCompnentTypes.Find(component => component.TypeId == obstacle.TypeId);
        GameObject newObstacle = obstacleComponent.Initialize(obstacle, m_CurrentStage.transform);
        RegisterObstacle(newObstacle);
      }
    }

    private GameObject SpawnNewTarget(Position spawnPosition) {
      float xPos, yPos, zPos;
      xPos = spawnPosition.X;
      zPos = spawnPosition.Z;
      if (m_StageNumber == 0) {
        yPos = spawnPosition.Y;
      } else {
        yPos = Constants.TARGET_LOCATION_Y_SPAWN_LOCATION;
      }
      Vector3 spawnLocation = new Vector3(xPos, yPos, zPos);
      GameObject newTarget = Instantiate(m_TargetPrefab, spawnLocation, Quaternion.identity, m_CurrentStage.transform);
      return newTarget;
    }

    private void RegisterNewTarget(GameObject target) {
      if (m_PreviousTarget != null) {
        m_PreviousTarget.GoalLocation.OnTargetReached -= TargetGoalLocation_OnTargetReached;
        m_InactiveTargets.Add(m_PreviousTarget);
        m_PreviousTarget = m_CurrentTarget;
      }
      Target targetComponent = target.GetComponent<Target>();
      m_PreviousTarget = m_CurrentTarget;
      m_CurrentTarget = targetComponent;
      targetComponent.GoalLocation.OnTargetReached += TargetGoalLocation_OnTargetReached;
    }

    private void RegisterObstacle(GameObject obstacle) {
      m_Obstacles.Add(obstacle.GetComponent<Obstacle>());
    }
  }
}
