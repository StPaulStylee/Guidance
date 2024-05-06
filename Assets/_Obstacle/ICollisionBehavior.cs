using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  public interface ICollisionBehavior {
    void ApplyCollisionBehavior(Ball ball, Collider collider);
  }
}
