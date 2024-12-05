using _Ball;
using _Ball.Obstacles;
using _Target;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Obstacle.Obstacle_Blue {
  public class BlueObstacle : Obstacle, ICollisionBehavior {
    [FormerlySerializedAs("m_LaunchForce")] [SerializeField]
    private float launchForce;

    private Target _target;

    private void Awake() {
      CollisionBehavior = this;
      TypeId = ObstacleType.Blue;
    }

    public void ApplyCollisionBehaviorOnEnter(Ball ball, Collider col) { }

    public void ApplyCollisionBehaviorOnExit(Ball ball, Collider col) { }

    public void ApplyCollisionBehaviorOnStay(Ball ball, Collider col) {
      ball.Rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
    }
  }
}
