using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  public abstract class Obstacle : MonoBehaviour {
    public ObstacleType TypeId;
    protected ICollisionBehavior CollisionBehavior;

    private void OnCollisionStay(Collision collision) {
      if (CollisionBehavior != null && collision.transform.TryGetComponent(out Ball ball)) {
        CollisionBehavior.ApplyCollisionBehavior(ball, collision);
      }
    }
  }
}
