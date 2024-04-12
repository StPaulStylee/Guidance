using Sirenix.OdinInspector;
using UnityEngine;

namespace Guidance.Data {
  [System.Serializable]
  public class StageData {
    [PropertyOrder(3)]
    public ObstacleData[] Obstacles;
    [PropertyOrder(1)]
    public int StageNumber;
    [PropertyOrder(2)]
    public Vector3 TargetLocation;
  }

  [System.Serializable]
  public class ObstacleData {
    public Vector3 Position;
    public float Rotation;
    public float Scale;
    [PropertyOrder(1)]
    public int TypeId;
  }
}
