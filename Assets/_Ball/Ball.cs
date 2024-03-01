using UnityEngine;

namespace Guidance.Gameplay {
  [RequireComponent(typeof(Rigidbody))]
  public class Ball : MonoBehaviour {
    private Rigidbody rb;
    private void Awake() {
      rb = GetComponent<Rigidbody>();
      rb.isKinematic = true;
      PlatformCreator.OnPlatformCreated += EnableRigidBody;
    }

    private void OnDisable() {
      PlatformCreator.OnPlatformCreated -= EnableRigidBody;
    }

    private void EnableRigidBody() {
      if (rb.isKinematic == true) {
        rb.isKinematic = false;
      }
    }

    public void ToggleRigidBody() {
      rb.isKinematic = !rb.isKinematic;
    }
  }
}
