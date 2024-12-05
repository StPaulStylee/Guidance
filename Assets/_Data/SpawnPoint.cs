using _Background;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Data {
  public class SpawnPoint : MonoBehaviour {
    [FormerlySerializedAs("m_RotationRange")] [SerializeField]
    private float[] rotationRange;

    [FormerlySerializedAs("m_YVariation")]
    [Tooltip("2 indices. First index represents variation on the positive Y. Second represents variation in the negative Y.")]
    [SerializeField]
    private float[] yVariation;

    private float GetRandomYRotation() {
      return Random.Range(rotationRange[0], rotationRange[1]);
    }

    private float GetRandomYPositionVariation() {
      // If first index value is "0" I know it can only vary on the "-Y"
      if (yVariation[0] == 0f) return -Random.Range(0, yVariation[1]);
      // If the second index value is "0" I know it can only vary on the "+Y"
      if (yVariation[1] == 0f) return Random.Range(0, yVariation[0]);
      // I think this logic only works because the values in both indicies are the same value
      return Random.Range(-yVariation[0], yVariation[1]);
    }

    public BackgroundObjectSpawnData GetSpawnData() {
      Quaternion rotation = Quaternion.Euler(0f, GetRandomYRotation(), 0f);
      Vector3 position = new(transform.position.x, transform.position.y + GetRandomYPositionVariation(),
        transform.position.z);
      return new BackgroundObjectSpawnData(rotation, position);
    }
  }
}
