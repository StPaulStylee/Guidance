using _Ball;
using _Ball.Obstacles;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Obstacle.Obstacle_Red {
  public class RedObstacle : Obstacle, ICollisionBehavior {
    [FormerlySerializedAs("m_Teleport0")] [SerializeField]
    private Transform teleport0;

    [FormerlySerializedAs("m_Teleport1")] [SerializeField]
    private Transform teleport1;

    private Collider _teleport1Collider;

    private void Awake() {
      teleport0 = transform;
      CollisionBehavior = this;
      TypeId = ObstacleType.Red;
    }

    private void Start() {
      Obstacle[] obstaclesInStage = transform.parent.GetComponentsInChildren<Obstacle>();
      foreach (Obstacle obstacle in obstaclesInStage)
        if (obstacle is RedObstacle && obstacle != this && obstacle.LinkId == LinkId) {
          teleport1 = obstacle.transform;
          _teleport1Collider = teleport1.GetComponent<Collider>();
        }
    }

    public void ApplyCollisionBehaviorOnEnter(Ball ball, Collider col) {
      Debug.LogWarning($"ApplyCollisionBehaviorOnEnter not implemented on {name}");
    }

    public void ApplyCollisionBehaviorOnExit(Ball ball, Collider col) {
      Debug.LogWarning($"ApplyCollisionBehaviorOnExit not implemented on {name}");
    }

    public void ApplyCollisionBehaviorOnStay(Ball ball, Collider col) {
      Vector3 teleportPosition = teleport1.transform.position;
      teleportPosition.y = teleport1.transform.position.y - ball.Collider.bounds.size.y;
      Debug.Log(_teleport1Collider.bounds.center);
      teleportPosition.x = _teleport1Collider.bounds.center.x;
      Vector3 currentVelocity = ball.Rb.velocity;
      currentVelocity.x = 0f;
      currentVelocity.y = 0f;
      ball.Rb.velocity = currentVelocity;
      ball.Rb.position = teleportPosition;
    }
  }
}
