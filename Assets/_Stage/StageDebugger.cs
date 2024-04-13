using Guidance.Data;
using Guidance.Gameplay.Game.Controller;
using System.Linq;
using UnityEngine;

namespace Guidance.Stage {
  public class StageDebugger : MonoBehaviour {
    public static void SetStageDebug(int stageNumber) {
      if (stageNumber <= 0) {
        Debug.LogError("The Stage Number must be greater than zero. If you want to debug the first level... Just play the game.)");
        return;
      }
      StageData[] stageData = Utilities.GetStageData();
      GameController controller = GameObject.Find("Game").GetComponent<GameController>();
      controller.IsStageDebug = true;
      controller.StageNumberToDebug = stageNumber;
      Position ballPosition = stageData.FirstOrDefault(stage => stage.StageNumber == stageNumber - 1).TargetLocation;
      Vector3 ballPositionVector = new Vector3(ballPosition.X, -5.5f, ballPosition.Z);
      controller.BallPosition = ballPositionVector;
    }

    public static void ResetStageDebug() {
      GameController controller = GameObject.Find("Game").GetComponent<GameController>();
      controller.IsStageDebug = false;
      controller.StageNumberToDebug = 0;
      controller.ResetBallPosition();
    }
  }
}
