using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  public class GreenObstacle : Obstacle, ICollisionBehavior {
    public void ApplyCollisionBehavior(Ball ball) {
      Debug.Log("Applying Force");
      Vector3 direction = transform.right;
      ball.Rb.AddForce(direction * 0.2f, ForceMode.Impulse);
    }

    private void Awake() {
      CollisionBehavior = this;
      TypeId = 1;
    }
  }
}
