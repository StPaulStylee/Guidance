using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  public class RedObstacle : Obstacle, ICollisionBehavior {
    [SerializeField] private Transform m_Teleport0;
    [SerializeField] private Transform m_Teleport1;
    private Collider m_Teleport1_Collider;
    private void Awake() {
      m_Teleport0 = this.transform;
      CollisionBehavior = this;
      TypeId = ObstacleType.Red;
    }

    private void Start() {
      Obstacle[] obstaclesInStage = transform.parent.GetComponentsInChildren<Obstacle>();
      foreach (Obstacle obstacle in obstaclesInStage) {
        if (obstacle is RedObstacle && obstacle != this && obstacle.LinkId == this.LinkId) {
          m_Teleport1 = obstacle.transform;
          m_Teleport1_Collider = m_Teleport1.GetComponent<Collider>();
        }
      }
    }

    public void ApplyCollisionBehaviorOnEnter(Ball ball, Collider collider) {
      Debug.LogWarning($"ApplyCollisionBehaviorOnEnter not implemented on {name}");
    }

    public void ApplyCollisionBehaviorOnExit(Ball ball, Collider collider) {
      Debug.LogWarning($"ApplyCollisionBehaviorOnExit not implemented on {name}");
    }

    public void ApplyCollisionBehaviorOnStay(Ball ball, Collider collider) {
      Vector3 teleportPosition = m_Teleport1.transform.position;
      teleportPosition.y = m_Teleport1.transform.position.y - ball.Collider.bounds.size.y;
      Debug.Log(m_Teleport1_Collider.bounds.center);
      teleportPosition.x = m_Teleport1_Collider.bounds.center.x;
      Vector3 currentVelocity = ball.Rb.velocity;
      currentVelocity.x = 0f;
      currentVelocity.y = 0f;
      ball.Rb.velocity = currentVelocity;
      ball.Rb.position = teleportPosition;
    }
  }
}
