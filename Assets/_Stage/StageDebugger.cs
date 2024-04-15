using Guidance.Data;
using Guidance.Gameplay.Game.Controller;
using Guidance.Stage.Data;
using System.Linq;
using UnityEngine;

namespace Guidance.Stage {
  public class StageDebugger : MonoBehaviour {
    private static StageDebuggerData m_Data;

    private static void SetStageDebuggerData() {
      if (m_Data != null) {
        return;
      }
      StageData[] stageData = Utilities.GetStageData();
      GameController controller = GameObject.Find("Game").GetComponent<GameController>();
      m_Data = new StageDebuggerData(stageData, controller);
    }

    // I need to implement enabling the stage prefabs in the heirarchy when debugging so that I 
    // can easily view the layout. Then when play is pressed the prefab needs to be disabled.
    public static void SetStageDebug(int stageNumber) {
      if (stageNumber <= 0) {
        Debug.LogError("The Stage Number must be greater than zero. If you want to debug the first level... Just play the game.)");
        return;
      }
      SetStageDebuggerData();
      m_Data.Controller.IsStageDebug = true;
      m_Data.Controller.StageNumber = stageNumber;
      SetBallPosition(stageNumber);
    }

    public static void ResetStageDebug() {
      SetStageDebuggerData();
      m_Data.Controller.IsStageDebug = false;
      m_Data.Controller.StageNumber = 0;
      m_Data.Controller.ResetBallPosition();
    }

    private static void SetBallPosition(int stageNumber) {
      Position ballPosition = m_Data.Data.FirstOrDefault(stage => stage.StageNumber == stageNumber - 1).TargetLocation;
      Vector3 ballPositionVector = new Vector3(ballPosition.X, -5.5f, ballPosition.Z);
      m_Data.Controller.BallPosition = ballPositionVector;
    }
  }
}
