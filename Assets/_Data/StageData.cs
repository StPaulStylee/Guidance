using UnityEngine;

namespace Guidance.Data {
  [System.Serializable]
  public class StageData {
    public int StageNumber;
    public Vector3 TargetLocation;
    public ObstacleData[] Obstacles;
  }

  [System.Serializable]
  public class ObstacleData {
    public float Scale;
    public float Rotation;
    public Vector3 Position;
  }
}
