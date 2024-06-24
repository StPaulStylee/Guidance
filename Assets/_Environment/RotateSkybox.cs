using UnityEngine;

namespace Guidance.Environment {
  public class RotateSkybox : MonoBehaviour {
    [SerializeField] private float m_RotationSpeed = 0f;
    [SerializeField] private bool m_IsRotating = false;
    void Update() {
      if (m_IsRotating) {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * m_RotationSpeed);
      }
    }
  }
}
