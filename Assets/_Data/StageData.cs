using Guidance.Stage;
using Sirenix.OdinInspector;

namespace Guidance.Data {
  [System.Serializable]
  public class StageData {
    [PropertyOrder(3)]
    public ObstacleData[] Obstacles;
    [PropertyOrder(1)]
    public int StageNumber;
    [PropertyOrder(2)]
    public Position TargetLocation;

    [PropertyOrder(4)]
    [Button("Debug this Stage")]
    private void DebugStage() {
      StageDebugger.SetStageDebug(StageNumber);
    }
  }

  [System.Serializable]
  public class ObstacleData {
    public Position Position;
    public float Rotation;
    public float Scale;
    [PropertyOrder(1)]
    public int TypeId;
  }

  [System.Serializable]
  public class Position {
    public float X;
    public float Y;
    public float Z;
  }
}
