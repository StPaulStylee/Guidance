using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  public class BlueObstacle : Obstacle, ICollisionBehavior {
    private void Awake() {
      CollisionBehavior = this;
      TypeId = ObstacleType.Blue;
    }

    public void ApplyCollisionBehaviorOnEnter(Ball ball, Collider collider) {
      throw new System.NotImplementedException();
    }

    public void ApplyCollisionBehaviorOnExit(Ball ball, Collider collider) {
      throw new System.NotImplementedException();
    }

    public void ApplyCollisionBehaviorOnStay(Ball ball, Collider collider) {
      throw new System.NotImplementedException();
    }
  }
}
