using Guidance.Data;
using Guidance.Gameplay.Game.Manager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Guidance.Gameplay.Targets {
  public class StageManager : MonoBehaviour, IStageTransition {
    private const float MINIMUM_X_SPAWN_LOCATION = -4.5F;
    private const float MAXIMUM_X_SPAWN_LOCATION = 4.5f;

    private readonly float m_YSpawnDistance = Constants.Y_STAGE_LENGTH;

    public event Action OnTargetReached;

    [SerializeField] private GameObject m_TargetPrefab;
    [SerializeField] private Target m_CurrentGoalTarget;
    [SerializeField] private Target m_PreviousGoalTarget = null;
    [SerializeField] private GameObject m_ObstaclePrefab;
    private StageData[] m_StageData;
    private int m_StageNumber;

    private List<Target> m_Targets;
    private List<Obstacle> m_Obstacles;

    private void Awake() {
      m_Targets = new();
      m_Obstacles = new();
      LoadStageData();
      //GameObject initialTarget = SpawnNewTarget(-2.97f, -5.5f, 0f);
      //RegisterNewTarget(initialTarget);
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
      //GameObject newTarget = SpawnNewTarget();
      //RegisterNewTarget(newTarget);
      OnTargetReached?.Invoke();
    }

    private void SpawnObstacles(ObstacleData[] obstacles) {
      if (obstacles.Length == 0) {
        return;
      }
      foreach (ObstacleData obstacle in obstacles) {
        float xPos = obstacle.Position.x;
        float yPos = obstacle.Position.y - m_YSpawnDistance;
        float zPos = obstacle.Position.z;
        Vector3 spawnLocation = new Vector3(xPos, yPos, zPos);
        Quaternion spawnRoation = Quaternion.Euler(0, 0, obstacle.Rotation);
        GameObject newObstacle = Instantiate(m_ObstaclePrefab, spawnLocation, spawnRoation);
        newObstacle.transform.localScale = new Vector3(obstacle.Scale, 1f, 1f);
        m_Obstacles.Add(newObstacle.GetComponent<Obstacle>());
      }
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
      //float spawnLocationX = UnityEngine.Random.Range(MINIMUM_X_SPAWN_LOCATION, MAXIMUM_X_SPAWN_LOCATION);
      Vector3 spawnLocation = new Vector3(xPos, yPos, zPos);
      GameObject newTarget = Instantiate(m_TargetPrefab, spawnLocation, Quaternion.identity, transform);
      return newTarget;
    }

    private GameObject SpawnNewTarget(Vector3 spawnPositon) {
      float xPos = spawnPositon.x;
      float yPos = spawnPositon.y;
      float zPos = spawnPositon.z;
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

    private void LoadStageData() {
      string json = File.ReadAllText(Application.dataPath + "/_Data/StageData.json");
      m_StageData = JsonConvert.DeserializeObject<StageData[]>(json);
    }
  }
}
