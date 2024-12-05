using UnityEngine;

namespace _Background.Spaceship {
  public class Spaceship : MonoBehaviour {
    [SerializeField] private float m_Speed;
    private Vector3 _movementVector;

    private void Awake() {
      _movementVector = new Vector3(0f, m_Speed, 0f);
    }

    private void Update() {
      transform.Translate(Vector3.forward * (m_Speed * Time.deltaTime));
    }
  }
}
