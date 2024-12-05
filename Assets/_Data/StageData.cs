using System;
using _Ball.Obstacles;
using _Obstacle;
using _Stage;
using Sirenix.OdinInspector;

namespace _Data {
  [Serializable]
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

  [Serializable]
  public class ObstacleData {
    public Position Position;
    public float Rotation;
    public float Scale;

    [PropertyOrder(1)]
    public ObstacleType TypeId;

    public SpinDirection RotationDirection;
    public int? LinkId = null;
  }

  [Serializable]
  public class Position {
    public float X;
    public float Y;
    public float Z;
  }
}
