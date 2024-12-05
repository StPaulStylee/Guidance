using System;
using System.Collections.Generic;
using _Data;
using _Game;
using _Obstacle;
using _Target;
using Guidance.Stage;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Stage {
  public class StageManager : MonoBehaviour, IStageTransition {
    [FormerlySerializedAs("mTargetPrefab")] [FormerlySerializedAs("m_TargetPrefab")] [SerializeField]
    private GameObject targetPrefab;

    [FormerlySerializedAs("m_ObstacleCompnentTypes")] [SerializeField]
    private List<Obstacle> obstacleCompnentTypes;

    private GameObject _currentStage;
    private Target _currentTarget;
    private List<Target> _inactiveTargets;
    private List<Obstacle> _obstacles;
    private Target _previousTarget;
    private StageData[] _stageData;
    private int _stageNumber;

    public List<Obstacle> ObstacleComponentTypes => obstacleCompnentTypes;

    private void Awake() {
      _inactiveTargets = new List<Target>();
      _obstacles = new List<Obstacle>();
      _stageData = Utilities.GetStageData();
    }

    public void ShiftForStageTransition() {
      if (_previousTarget) {
        Vector3 position = _previousTarget.transform.position;
        // Should this variable be named "newTargetLocation" or "shiftLocation" or something?
        Position previousTargetLocation = new()
          { X = position.x, Y = Constants.TARGET_LOCATION_Y_FINAL_LOCATION, Z = position.z };
        StartCoroutine(
          StageTransitionManager.ShiftToStartLocationForNextStage(_previousTarget.transform, previousTargetLocation));
      }

      Position newTargetLocation = _stageData[_stageNumber].TargetLocation;
      StartCoroutine(
        StageTransitionManager.ShiftToStartLocationForNextStage(_currentTarget.transform, newTargetLocation));
      foreach (Target target in _inactiveTargets)
        StartCoroutine(StageTransitionManager.ShiftForNextStage(target.transform));
      foreach (Obstacle obstacle in _obstacles)
        StartCoroutine(StageTransitionManager.ShiftForNextStage(obstacle.transform));
    }

    public event Action OnTargetReached;

    public void DeactivatePreviousGoalTarget() {
      if (!_previousTarget) {
        return;
      }

      if (!_previousTarget.GoalLocation.IsMeshColliderEnabled) {
        return;
      }

      _previousTarget.GoalLocation.DisableMeshCollider();
    }

    public bool SpawnNextStage(int stageNumber) {
      _stageNumber = stageNumber;
      _currentStage = new GameObject($"Stage_{_stageNumber}");
      _currentStage.transform.SetParent(transform);
      if (_stageNumber < 0 || _stageNumber >= _stageData?.Length) {
        Debug.LogError("Cannot access stage data for spawning a target because the index does not exist");
        return false;
      }

      StageData stageData = _stageData[_stageNumber];
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
        Obstacle obstacleComponent = obstacleCompnentTypes.Find(component => component.TypeId == obstacle.TypeId);
        GameObject newObstacle = obstacleComponent.Initialize(obstacle, _currentStage.transform);
        RegisterObstacle(newObstacle);
      }
    }

    private GameObject SpawnNewTarget(Position spawnPosition) {
      float xPos = spawnPosition.X;
      float zPos = spawnPosition.Z;
      float yPos = _stageNumber == 0 ? spawnPosition.Y : Constants.TARGET_LOCATION_Y_SPAWN_LOCATION;

      Vector3 spawnLocation = new(xPos, yPos, zPos);
      GameObject newTarget = Instantiate(targetPrefab, spawnLocation, Quaternion.identity, _currentStage.transform);
      return newTarget;
    }

    private void RegisterNewTarget(GameObject target) {
      if (_previousTarget != null) {
        _previousTarget.GoalLocation.OnTargetReached -= TargetGoalLocation_OnTargetReached;
        _inactiveTargets.Add(_previousTarget);
        _previousTarget = _currentTarget;
      }

      Target targetComponent = target.GetComponent<Target>();
      _previousTarget = _currentTarget;
      _currentTarget = targetComponent;
      targetComponent.GoalLocation.OnTargetReached += TargetGoalLocation_OnTargetReached;
    }

    private void RegisterObstacle(GameObject obstacle) {
      _obstacles.Add(obstacle.GetComponent<Obstacle>());
    }
  }
}
