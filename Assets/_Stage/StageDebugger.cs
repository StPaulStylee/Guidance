# if UNITY_EDITOR
using System.Linq;
using _Data;
using _Game;
using _Obstacle;
using _Target;
using Guidance.Stage;
using UnityEngine;
using static _Stage.StageDebuggerData;

namespace _Stage {
  public class StageDebugger : MonoBehaviour {
    private static StageDebuggerData _data;

    public static EditingData EditingData {
      get {
        if (_data == null) {
          SetStageDebuggerData();
        }

        return _data.Editing;
      }
    }

    private static void SetStageDebuggerData() {
      StageData[] stageData = Utilities.GetStageData();
      GameObject rootGo = GameObject.Find("Game");
      GameObject stageViewer = GameObject.Find("StageViewer");
      GameController controller = rootGo.GetComponent<GameController>();
      StageManager stageManager = rootGo.GetComponent<StageManager>();
      _data = new StageDebuggerData(stageData, controller, rootGo, stageViewer, stageManager) {
        Controller = {
          IsStageDebug = false,
          IsStageEdit = false
        }
      };
    }

    public static void SetStageDebug(int stageNumber) {
      //if (stageNumber <= 0) {
      //  Debug.LogError("The Stage Number must be greater than zero. If you want to debug the first level... Just play the game.)");
      //  return;
      //}
      SetStageDebuggerData();
      _data.Controller.IsStageDebug = true;
      _data.Controller.StageNumber = stageNumber;
      SetBallDebugPosition(stageNumber);
    }

    public static void ResetStageDebug() {
      SetStageDebuggerData();
      DeleteAllObjectsInStageViewer();
      _data.Controller.IsStageDebug = false;
      _data.Controller.IsStageEdit = false;
      _data.Controller.StageNumber = 0;
      _data.Controller.ResetBallPosition();
    }

    public static void ViewInScene(int stageNumber) {
      SetStageDebuggerData();
      DeleteAllObjectsInStageViewer();
      StageData stageData = _data.Data[stageNumber];
      _data.Editing.IsEditing = true;
      _data.Editing.StageBeingEdited = stageData.StageNumber;
      SetBallViewPosition(stageNumber);
      SpawnTarget(stageData);
      SpawnObstacles(stageData.Obstacles);
      _data.Controller.StageNumber = stageNumber;
      _data.Controller.IsStageEdit = true;
    }

    private static void SetBallViewPosition(int stageNumber) {
      StageData stageData = _data.Data.FirstOrDefault(stage => stage.StageNumber == stageNumber - 1) ?? _data.Data[0];
      Position ballPosition = stageData.TargetLocation;
      float xPos = stageData.StageNumber == 0 ? Constants.STARTING_BALL_POSITION_X : ballPosition.X;
      Vector3 ballPositionVector = new(xPos, Constants.STARTING_BALL_POSITION_Y, ballPosition.Z);
      _data.Controller.BallPosition = ballPositionVector;
      _data.Controller.CurrentActiveBall.transform.position = ballPositionVector;
    }

    private static void SetBallDebugPosition(int stageNumber) {
      StageData stageData = _data.Data.FirstOrDefault(stage => stage.StageNumber == stageNumber - 1) ?? _data.Data[0];
      Position ballPosition = stageData.TargetLocation;
      Vector3 ballPositionVector = new(ballPosition.X, -Constants.STARTING_BALL_POSITION_Y, ballPosition.Z);
      _data.Controller.BallPosition = ballPositionVector;
    }

    private static void SpawnTarget(StageData stageData) {
      GameObject targetPrefab = _data.Root.GetComponentInChildren<Target>(true).gameObject;
      float xPos = stageData.TargetLocation.X;
      float yPos = stageData.TargetLocation.Y;
      float zPos = stageData.TargetLocation.Z;
      Vector3 spawnLocation = new(xPos, yPos, zPos);
      GameObject newTarget =
        Instantiate(targetPrefab, spawnLocation, Quaternion.identity, _data.StageViewer.transform);
      newTarget.SetActive(true);
    }

    private static void SpawnObstacles(ObstacleData[] obstacles) {
      if (obstacles.Length == 0) {
        return;
      }

      foreach (ObstacleData obstacle in obstacles) {
        //float xPos = obstacle.Position.X;
        //float yPos = obstacle.Position.Y;
        //float zPos = obstacle.Position.Z;
        //Vector3 spawnLocation = new Vector3(xPos, yPos, zPos);
        //Quaternion spawnRotation = Quaternion.Euler(0, 0, obstacle.Rotation);
        //GameObject obstaclePrefab = m_Data.StageManager.ObstacleScriptableObjs.Find(so => so.TypeId == obstacle.TypeId).Prefab;
        //GameObject newObstacle = Instantiate(obstaclePrefab, spawnLocation, spawnRoation, m_Data.StageViewer.transform);
        //Obstacle obstacleComponent = newObstacle.GetComponent<Obstacle>();
        //newObstacle.transform.localScale = new Vector3(obstacle.Scale, 1f, 1f);
        //obstacleComponent.LinkId = obstacle.LinkId;
        Obstacle obstacleComponent =
          _data.StageManager.ObstacleComponentTypes.Find(component => component.TypeId == obstacle.TypeId);
        //Obstacle obstacleComponent = obstaclePrefab.GetComponent<Obstacle>();
        GameObject newObstacle = obstacleComponent.InitializeDebug(obstacle, _data.StageViewer.transform);
      }
    }

    private static void DeleteAllObjectsInStageViewer() {
      while (_data.StageViewer.transform.childCount > 0)
        DestroyImmediate(_data.StageViewer.transform.GetChild(0).gameObject);
    }
  }
}
#endif
