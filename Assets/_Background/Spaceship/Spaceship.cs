using UnityEngine;

namespace Guidance.Environment {
  public class Spaceship : MonoBehaviour {
    [SerializeField] private float m_Speed;
    private Vector3 m_MovementVector;

    private void Awake() {
      m_MovementVector = new Vector3(0f, m_Speed, 0f);
    }
    void Update() {
      transform.Translate(Vector3.forward * m_Speed * Time.deltaTime);
    }
  }
}
