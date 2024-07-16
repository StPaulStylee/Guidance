using UnityEngine;

namespace Guidance.Background.Object {
  public class SpawnPoint : MonoBehaviour {
    [SerializeField] private float[] m_RotationRange;
    [Tooltip("2 indicies. First index represents variation on the positive Y. Second represents variation in the negative Y.")]
    [SerializeField] private float[] m_YVariation;

    private float GetRandomYRotation() {
      return Random.Range(m_RotationRange[0], m_RotationRange[1]);
    }

    private float GetRandomYPositionVariation() {
      // If first index value is "0" I know it can only vary on the "-Y"
      if (m_YVariation[0] == 0f) {
        return -Random.Range(0, m_YVariation[1]);
      }
      // If the second index value is "0" I know it can only vary on the "+Y"
      if (m_YVariation[1] == 0f) {
        return Random.Range(0, m_YVariation[0]);
      }
      // I think this logic only works because the values in both indicies are the same value
      return Random.Range(-m_YVariation[0], m_YVariation[1]);
    }

    public BackgroundObjectSpawnData GetSpawnData() {
      Quaternion rotation = Quaternion.Euler(0f, GetRandomYRotation(), 0f);
      Vector3 position = new Vector3(transform.position.x, transform.position.y + GetRandomYPositionVariation(), transform.position.z);
      return new(rotation, position);
    }
  }
}
