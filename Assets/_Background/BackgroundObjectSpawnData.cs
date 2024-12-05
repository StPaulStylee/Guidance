using UnityEngine;

namespace _Background {
  public class BackgroundObjectSpawnData {
    public Vector3 Position;
    public Quaternion Rotation;

    public BackgroundObjectSpawnData(Quaternion rotation, Vector3 position) {
      Rotation = rotation;
      Position = position;
    }
  }
}