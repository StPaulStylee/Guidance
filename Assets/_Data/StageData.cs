using Guidance.Gameplay.Obstacles;
using Sirenix.OdinInspector;

namespace Guidance.Stage.Data {
  [System.Serializable]
  public class StageData {
    [PropertyOrder(3)]
    public ObstacleData[] Obstacles;
    [PropertyOrder(1)]
    public int StageNumber;
    [PropertyOrder(2)]
    public Position TargetLocation;
#if UNITY_EDITOR
    [PropertyOrder(4)]
    [Button("Debug this Stage")]
    private void DebugStage() {
      StageDebugger.SetStageDebug(StageNumber);
    }
    [PropertyOrder(5)]
    [Button("Edit/View in Scene")]
    private void ViewInScene() {
      StageDebugger.ViewInScene(StageNumber);
    }
#endif
  }

  [System.Serializable]
  public class ObstacleData {
    public Position Position;
    public float Rotation;
    public float Scale;
    [PropertyOrder(1)]
    public ObstacleType TypeId;
    public int? LinkId = null;
    public SpinDirection RotationDirection;
  }

  [System.Serializable]
  public class Position {
    public float X;
    public float Y;
    public float Z;
  }
}
