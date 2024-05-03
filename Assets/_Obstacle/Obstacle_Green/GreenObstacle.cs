using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  public class GreenObstacle : Obstacle, ICollisionBehavior {
    public void ApplyCollisionBehavior(Ball ball, Collision collision) {
      Vector3 direction = transform.right;
      ball.Rb.AddForce(direction * 0.2f, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision) {
      if (collision.transform.TryGetComponent(out Ball ball)) {
        ball.Rb.velocity = Vector3.zero;
      }
    }

    private void Awake() {
      CollisionBehavior = this;
      TypeId = ObstacleType.Green;
    }
  }
}
