using _Ball;
using _Ball.Obstacles;
using UnityEngine;

namespace _Obstacle.Obstacle_Green {
  public class GreenObstacle : Obstacle, ICollisionBehavior {
    //private void OnCollisionEnter(Collision collision) {
    //  if (collision.transform.TryGetComponent(out Ball ball)) {
    //    ball.Rb.velocity = Vector3.zero;
    //  }
    //}

    private void Awake() {
      CollisionBehavior = this;
      TypeId = ObstacleType.Green;
    }

    public void ApplyCollisionBehaviorOnEnter(Ball ball, Collider col) {
      ball.Rb.velocity = Vector3.zero;

      Debug.LogWarning($"ApplyCollisionBehaviorOnEnter not implemented on {name}");
    }

    public void ApplyCollisionBehaviorOnExit(Ball ball, Collider col) {
      Debug.LogWarning($"ApplyCollisionBehaviorOnExit not implemented on {name}");
    }

    public void ApplyCollisionBehaviorOnStay(Ball ball, Collider col) {
      Vector3 direction = transform.right;
      ball.Rb.AddForce(direction * 0.2f, ForceMode.Impulse);
    }
  }
}
