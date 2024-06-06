using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  public class GoldObstacle : Obstacle, ICollisionBehavior {
    [SerializeField]
    [Range(0f, 1f)]
    private float m_TimeScaleValue = 1f;
    private const float m_NormalTimeScale = 1f;
    private float m_NormalFixedDeltaTime;
    private void Awake() {
      CollisionBehavior = this;
      TypeId = ObstacleType.Gold;
      m_NormalFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void ApplyCollisionBehaviorOnEnter(Ball ball, Collider collider) {
      Time.timeScale = m_TimeScaleValue;
      Time.fixedDeltaTime = m_NormalFixedDeltaTime * Time.timeScale;
    }

    public void ApplyCollisionBehaviorOnExit(Ball ball, Collider collider) {
      Time.timeScale = m_NormalTimeScale;
      Time.fixedDeltaTime = m_NormalFixedDeltaTime;
    }

    public void ApplyCollisionBehaviorOnStay(Ball ball, Collider collider) {
      return;
    }
  }
}
