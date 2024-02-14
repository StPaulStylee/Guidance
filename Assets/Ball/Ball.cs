using UnityEngine;

namespace Guidance.Gameplay {
  [RequireComponent(typeof(Rigidbody))]
  public class Ball : MonoBehaviour {
    private Rigidbody rb;
    private void Awake() {
      rb = GetComponent<Rigidbody>();
      rb.isKinematic = true;
    }
  }
}
