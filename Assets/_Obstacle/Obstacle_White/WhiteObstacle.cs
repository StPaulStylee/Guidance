using System;
using _Ball;
using _Ball.Obstacles;
using UnityEngine;

namespace _Obstacle.Obstacle_White {
  public class WhiteObstacle : Obstacle, ICollisionBehavior {
    private void Awake() {
      TypeId = ObstacleType.White;
      CollisionBehavior = this;
    }

    public void ApplyCollisionBehaviorOnEnter(Ball ball, Collider col) {
      ball.Rb.velocity = Vector3.zero;
      ball.Rb.useGravity = false;
      ball.Rb.angularVelocity = Vector3.zero;
      ball.transform.rotation = Quaternion.identity;
      OnWhiteObstacleCollision?.Invoke();
    }

    public void ApplyCollisionBehaviorOnExit(Ball ball, Collider col) { }

    public void ApplyCollisionBehaviorOnStay(Ball ball, Collider col) { }

    public static event Action OnWhiteObstacleCollision;
  }
}
