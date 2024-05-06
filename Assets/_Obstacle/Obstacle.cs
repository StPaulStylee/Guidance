using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  public abstract class Obstacle : MonoBehaviour {
    public ObstacleType TypeId;
    [OdinSerialize][ShowInInspector] public int? LinkId;
    protected ICollisionBehavior CollisionBehavior;

    private void OnCollisionStay(Collision collision) {
      if (CollisionBehavior != null && collision.transform.TryGetComponent(out Ball ball)) {
        CollisionBehavior.ApplyCollisionBehavior(ball, collision.collider);
      }
    }

    private void OnTriggerStay(Collider collider) {
      if (CollisionBehavior != null && collider.transform.TryGetComponent(out Ball ball)) {
        CollisionBehavior.ApplyCollisionBehavior(ball, collider);
      }
    }
  }
}
