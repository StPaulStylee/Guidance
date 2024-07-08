using UnityEngine;

namespace Guidance.Background.Object {
  public class BackgroundObjectSpawnData {
    public Quaternion Rotation;
    public Vector3 Position;
    public BackgroundObjectSpawnData(Quaternion rotation, Vector3 position) {
      Rotation = rotation;
      Position = position;
    }
  }
}
