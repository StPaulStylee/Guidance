using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  public class RedObstacle : Obstacle, ICollisionBehavior {
    [SerializeField] private Transform m_Teleport0;
    [SerializeField] private Transform m_Teleport1;
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
        }
      }
    }

    public void ApplyCollisionBehavior(Ball ball, Collider collider) {
      Vector3 teleportPosition = m_Teleport1.transform.position;
      teleportPosition.y = m_Teleport1.transform.position.y - ball.transform.localScale.y;
      // Working on getting the position of the ball right in the center of the m_Teleport1
      teleportPosition.x = m_Teleport1.transform.position.x + (m_Teleport1.transform.localScale.x / 2);
      Vector3 currentVelocity = ball.Rb.velocity;
      currentVelocity.x = 0f;
      ball.Rb.velocity = currentVelocity;
      ball.Rb.position = teleportPosition;

    }
  }
}
