using _Ball;
using _Ball.Obstacles;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Obstacle.Obstacle_Gold {
  public class GoldObstacle : Obstacle, ICollisionBehavior {
    private const float NORMAL_TIME_SCALE = 1f;

    [FormerlySerializedAs("m_TimeScaleValue")] [SerializeField] [Range(0f, 1f)]
    private float timeScaleValue = 1f;

    private float _normalFixedDeltaTime;

    private void Awake() {
      CollisionBehavior = this;
      TypeId = ObstacleType.Gold;
      _normalFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void ApplyCollisionBehaviorOnEnter(Ball ball, Collider col) {
      Time.timeScale = timeScaleValue;
      Time.fixedDeltaTime = _normalFixedDeltaTime * Time.timeScale;
    }

    public void ApplyCollisionBehaviorOnExit(Ball ball, Collider col) {
      Time.timeScale = NORMAL_TIME_SCALE;
      Time.fixedDeltaTime = _normalFixedDeltaTime;
    }

    public void ApplyCollisionBehaviorOnStay(Ball ball, Collider col) { }
  }
}
