using Guidance.Gameplay.Stage;
using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  public class BlueObstacle : Obstacle, ICollisionBehavior {
    private Target m_Target;
    [SerializeField] private float m_LaunchForce;
    private void Awake() {
      CollisionBehavior = this;
      TypeId = ObstacleType.Blue;
    }

    public void ApplyCollisionBehaviorOnEnter(Ball ball, Collider collider) {
      return;
    }

    public void ApplyCollisionBehaviorOnExit(Ball ball, Collider collider) {
      return;
    }

    public void ApplyCollisionBehaviorOnStay(Ball ball, Collider collider) {
      ball.Rb.AddForce(Vector3.up * m_LaunchForce, ForceMode.Impulse);
    }
  }
}
