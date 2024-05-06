using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  public interface ICollisionBehavior {
    void ApplyCollisionBehaviorOnEnter(Ball ball, Collider collider);
    void ApplyCollisionBehaviorOnExit(Ball ball, Collider collider);
    void ApplyCollisionBehaviorOnStay(Ball ball, Collider collider);
  }
}
