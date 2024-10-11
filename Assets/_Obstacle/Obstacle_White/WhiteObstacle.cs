using Guidance.Gameplay.Game.Manager;
using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  public class WhiteObstacle : Obstacle, ICollisionBehavior {
    public void ApplyCollisionBehaviorOnEnter(Ball ball, Collider collider) {
      ball.Rb.velocity = Vector3.zero;
      ball.Rb.useGravity = false;
      //ball.transform.rotation = Quaternion.identity;
      StartCoroutine(BallDissolveManager.PerformVerticalDissolveDown(ball.BallMaterial));
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
