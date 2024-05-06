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
    private readonly float m_YSpawnDistance = Constants.Y_STAGE_LENGTH;

    public event Action OnTargetReached;

    [SerializeField] private GameObject m_TargetPrefab;
    [SerializeField] private List<Obstacle_SO> m_ObstacleScriptableObjs;
    public List<Obstacle_SO> ObstacleScriptableObjs {
      get {
        return m_ObstacleScriptableObjs;
      }
    }
    private List<Target> m_Targets;
    private List<Obstacle> m_Obstacles;

    private GameObject m_CurrentStage;
    private StageData[] m_StageData;
    private int m_StageNumber;

    private void Awake() {
      m_Targets = new();
      m_Obstacles = new();
      m_StageData = Utilities.GetStageData();
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
      foreach (Obstacle obstacle in m_Obstacles) {
        StartCoroutine(StageTransitionManager.ShiftForNextStage(obstacle.transform));
      }
    }

    public void SpawnNextStage(int stageNumber) {
      m_StageNumber = stageNumber;
      m_CurrentStage = new GameObject($"Stage_{m_StageNumber}");
      m_CurrentStage.transform.SetParent(transform);
      if (m_StageNumber < 0 || m_StageNumber >= m_StageData?.Length) {
        Debug.LogError("Cannot access stage data for spawning a target because the index does not exist");
        return;
      }
      StageData stageData = m_StageData[m_StageNumber];
      GameObject target = SpawnNewTarget(stageData.TargetLocation);
      RegisterNewTarget(target);
      SpawnObstacles(stageData.Obstacles);
    }


    private void TargetGoalLocation_OnTargetReached() {
      OnTargetReached?.Invoke();
    }

    private void SpawnObstacles(ObstacleData[] obstacles) {
      if (obstacles.Length == 0) {
        return;
      }
      foreach (ObstacleData obstacle in obstacles) {
        float xPos = obstacle.Position.X;
        float yPos = obstacle.Position.Y - m_YSpawnDistance;
        float zPos = obstacle.Position.Z;
        Vector3 spawnLocation = new Vector3(xPos, yPos, zPos);
        Quaternion spawnRoation = Quaternion.Euler(0, 0, obstacle.Rotation);
        GameObject obstaclePrefab = m_ObstacleScriptableObjs.Find(so => so.TypeId == obstacle.TypeId).Prefab;
        GameObject newObstacle = Instantiate(obstaclePrefab, spawnLocation, spawnRoation, m_CurrentStage.transform);
        Obstacle obstacleComponent = newObstacle.GetComponent<Obstacle>();
        newObstacle.transform.localScale = new Vector3(obstacle.Scale, 1f, 1f);
        obstacleComponent.LinkId = obstacle.LinkId;
        RegisterObstacle(newObstacle);
      }
    }

    private GameObject SpawnNewTarget(Position spawnPosition) {
      float xPos = spawnPosition.X;
      float yPos = spawnPosition.Y;
      float zPos = spawnPosition.Z;
      Vector3 spawnLocation = new Vector3(xPos, yPos, zPos);
      GameObject newTarget = Instantiate(m_TargetPrefab, spawnLocation, Quaternion.identity, m_CurrentStage.transform);
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

    private void RegisterObstacle(GameObject obstacle) {
      m_Obstacles.Add(obstacle.GetComponent<Obstacle>());
    }
  }
}
