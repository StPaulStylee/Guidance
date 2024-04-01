using UnityEngine;

namespace Guidance.Data {
  [System.Serializable]
  public class StageData {
    public int StageNumber;
    public Vector3 TargetLocation;
    public Obstacle[] Obstacles;
  }

  [System.Serializable]
  public class Obstacle {
    public float Scale;
    public float Rotation;
    public Vector3 Position;
  }
}
