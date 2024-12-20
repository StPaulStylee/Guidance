using System;
using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  public class WhiteObstacle : Obstacle, ICollisionBehavior {
    public static event Action OnWhiteObstacleCollision;
    public void ApplyCollisionBehaviorOnEnter(Ball ball, Collider collider) {
      ball.Rb.velocity = Vector3.zero;
      ball.Rb.useGravity = false;
      ball.Rb.angularVelocity = Vector3.zero;
      ball.transform.rotation = Quaternion.identity;
      OnWhiteObstacleCollision?.Invoke();
      return;
    }

    public void ApplyCollisionBehaviorOnExit(Ball ball, Collider collider) {
      return;
    }

    public void ApplyCollisionBehaviorOnStay(Ball ball, Collider collider) {
      return;
    }

    private void Awake() {
      TypeId = ObstacleType.White;
      CollisionBehavior = this;
    }
  }
}
