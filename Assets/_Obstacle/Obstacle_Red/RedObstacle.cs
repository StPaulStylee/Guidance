using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  public class RedObstacle : Obstacle, ICollisionBehavior {
    private Transform m_Teleport0;
    private Transform m_Teleport1;
    private void Awake() {
      Transform[] obstacles = GetComponentsInChildren<Transform>();
      m_Teleport0 = obstacles[0];
      m_Teleport1 = obstacles[1];
      TypeId = ObstacleType.Red;
    }

    private void OnCollisionEnter(Collision collision) {
      if (collision.transform.TryGetComponent(out Ball ball)) {
        ApplyCollisionBehavior(ball, collision);
      }
    }

    public void ApplyCollisionBehavior(Ball ball, Collision collision) {
      Debug.Log(collision);
    }
  }
}
