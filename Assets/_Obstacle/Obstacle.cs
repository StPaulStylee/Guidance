using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  public abstract class Obstacle : MonoBehaviour {
    public int TypeId;
    protected ICollisionBehavior CollisionBehavior;
    private void OnCollisionEnter(Collision collision) {
      if (CollisionBehavior != null && collision.transform.TryGetComponent(out Ball ball)) {
        ball.Rb.velocity = Vector3.zero;
      }
    }

    private void OnCollisionStay(Collision collision) {
      if (CollisionBehavior != null && collision.transform.TryGetComponent(out Ball ball)) {
        CollisionBehavior.ApplyCollisionBehavior(ball);
      }
    }
  }
}
