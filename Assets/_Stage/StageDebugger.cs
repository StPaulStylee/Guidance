using Guidance.Data;
using Guidance.Gameplay.Game.Controller;
using Guidance.Gameplay.Stage;
using Guidance.Stage.Data;
using System.Linq;
using UnityEngine;
using static Guidance.Stage.Data.StageDebuggerData;

namespace Guidance.Stage {
  public class StageDebugger : MonoBehaviour {
    private static StageDebuggerData m_Data;
    private static void SetStageDebuggerData() {
      StageData[] stageData = Utilities.GetStageData();
      GameObject rootGO = GameObject.Find("Game");
      GameObject stageViewer = GameObject.Find("StageViewer");
      GameController controller = rootGO.GetComponent<GameController>();
      StageManager stageManager = rootGO.GetComponent<StageManager>();
      m_Data = new StageDebuggerData(stageData, controller, rootGO, stageViewer, stageManager);
      m_Data.Controller.IsStageDebug = false;
      m_Data.Controller.IsStageEdit = false;
    }

    public static EditingData EditingData {
      get {
        if (m_Data == null) {
          SetStageDebuggerData();
        }
        return m_Data.Editing;
      }
    }

    public static void SetStageDebug(int stageNumber) {
      if (stageNumber <= 0) {
        Debug.LogError("The Stage Number must be greater than zero. If you want to debug the first level... Just play the game.)");
        return;
      }
      SetStageDebuggerData();
      m_Data.Controller.IsStageDebug = true;
      m_Data.Controller.StageNumber = stageNumber;
      SetBallDebugPosition(stageNumber);
    }

    public static void ResetStageDebug() {
      SetStageDebuggerData();
      DeleteAllObjectsInStageViewer();
      //ResetAllIsEditingFlags();
      m_Data.Controller.IsStageDebug = false;
      m_Data.Controller.IsStageEdit = false;
      m_Data.Controller.StageNumber = 0;
      m_Data.Controller.ResetBallPosition();
    }

    // THe editing isn't working. I think I need to be sharing the same reference of the stage data between
    // this and the StageCreator. If I could find a way to remove this as a monobehavior could I just new up 
    // and instance of this class to be consumed by the StageCreator? Can I pass an "Instantiate" method as
    // a method parameter?
    public static void ViewInScene(int stageNumber) {
      SetStageDebuggerData();
      DeleteAllObjectsInStageViewer();
      foreach (StageData stage in m_Data.Data) {

      }
      //ResetAllIsEditingFlags();
      StageData stageData = m_Data.Data[stageNumber];
      m_Data.Editing.IsEditing = true;
      m_Data.Editing.StageBeingEdited = stageData.StageNumber;
      SetBallViewPosition(stageNumber);
      SpawnTarget(stageData);
      SpawnObstacles(stageData.Obstacles);
      m_Data.Controller.StageNumber = stageNumber;
      m_Data.Controller.IsStageEdit = true;
    }

    public static void RemoveStage(int StageNumber) {
      Debug.LogError("Not implemented yet.");
    }

    //private static void ResetAllIsEditingFlags() {
    //  foreach (StageData stage in m_Data.Data) {
    //    stage.IsEditing = false;
    //  }
    //}

    private static void SetBallViewPosition(int stageNumber) {
      StageData stageData = m_Data.Data.FirstOrDefault(stage => stage.StageNumber == stageNumber - 1) ?? m_Data.Data[0];
      Position ballPosition = stageData.TargetLocation;
      float xPos = stageData.StageNumber == 0 ? Constants.STARTING_BALL_POSITION_X : ballPosition.X;
      Vector3 ballPositionVector = new Vector3(xPos, Constants.STARTING_BALL_POSITION_Y, ballPosition.Z);
      m_Data.Controller.BallPosition = ballPositionVector;
      m_Data.Controller.CurrentActiveBall.transform.position = ballPositionVector;
    }

    private static void SetBallDebugPosition(int stageNumber) {
      StageData stageData = m_Data.Data.FirstOrDefault(stage => stage.StageNumber == stageNumber - 1) ?? m_Data.Data[0];
      Position ballPosition = stageData.TargetLocation;
      Vector3 ballPositionVector = new Vector3(ballPosition.X, -Constants.STARTING_BALL_POSITION_Y, ballPosition.Z);
      m_Data.Controller.BallPosition = ballPositionVector;
    }

    private static void SpawnTarget(StageData stageData) {
      GameObject targetPrefab = m_Data.Root.gameObject.GetComponentInChildren<Target>(true).gameObject;
      float xPos = stageData.TargetLocation.X;
      float yPos = stageData.StageNumber == 0 ? stageData.TargetLocation.Y : stageData.TargetLocation.Y + Constants.Y_STAGE_LENGTH;
      float zPos = stageData.TargetLocation.Z;
      Vector3 spawnLocation = new Vector3(xPos, yPos, zPos);
      GameObject newTarget = Instantiate(targetPrefab, spawnLocation, Quaternion.identity, m_Data.StageViewer.transform);
      newTarget.gameObject.SetActive(true);
    }

    private static void SpawnObstacles(ObstacleData[] obstacles) {
      if (obstacles.Length == 0) {
        return;
      }
      foreach (ObstacleData obstacle in obstacles) {
        float xPos = obstacle.Position.X;
        float yPos = obstacle.Position.Y;
        float zPos = obstacle.Position.Z;
        Vector3 spawnLocation = new Vector3(xPos, yPos, zPos);
        Quaternion spawnRoation = Quaternion.Euler(0, 0, obstacle.Rotation);
        GameObject obstaclePrefab = m_Data.StageManager.ObstacleScriptableObjs.Find(so => so.TypeId == obstacle.TypeId).Prefab;
        GameObject newObstacle = Instantiate(obstaclePrefab, spawnLocation, spawnRoation, m_Data.StageViewer.transform);
        newObstacle.transform.localScale = new Vector3(obstacle.Scale, 1f, 1f);
      }
    }

    private static void DeleteAllObjectsInStageViewer() {
      while (m_Data.StageViewer.transform.childCount > 0) {
        DestroyImmediate(m_Data.StageViewer.transform.GetChild(0).gameObject);
      }
    }
  }
}
