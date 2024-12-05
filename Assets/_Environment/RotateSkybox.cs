using UnityEngine;
using UnityEngine.Serialization;

namespace _Environment {
  public class RotateSkybox : MonoBehaviour {
    [FormerlySerializedAs("m_RotationSpeed")] [SerializeField]
    private float rotationSpeed;

    [FormerlySerializedAs("m_IsRotating")] [SerializeField]
    private bool isRotating;

    private void Update() {
      if (isRotating) RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }
  }
}
