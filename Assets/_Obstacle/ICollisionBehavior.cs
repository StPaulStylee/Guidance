using _Ball;
using UnityEngine;

namespace _Obstacle {
  public interface ICollisionBehavior {
    void ApplyCollisionBehaviorOnEnter(Ball ball, Collider col);
    void ApplyCollisionBehaviorOnExit(Ball ball, Collider col);
    void ApplyCollisionBehaviorOnStay(Ball ball, Collider col);
  }
}
