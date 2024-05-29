using Guidance.Stage.Data;
using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  public class YellowObstacle : Obstacle, ICollisionBehavior {
    [SerializeField] private float m_RotationSpeed;
    [SerializeField] private float m_VelocityDamp = 0.25f;
    private bool m_IsDirty = false;
    [field: SerializeField] public SpinDirection SpinDirection { get; private set; }
    private Vector3 m_RotationDirection { get; set; }

    private void Awake() {
      TypeId = ObstacleType.Yellow;
      CollisionBehavior = this;
      // RIght now I am trying to save this SpinDirectoin value in the StageCreator. I tried to overwrite 
      // the base classes value here with Yellow's own value but for some reason the GetSpinDirection is 
      // returning SpinDirection.None in the StageCreator. 
    }

    private void Start() {
      m_RotationDirection = SpinDirection == SpinDirection.Clockwise ? Vector3.back : Vector3.forward;
    }

    void Update() {
      transform.Rotate(m_RotationDirection, m_RotationSpeed * Time.deltaTime);
    }

    public override GameObject Initialize(ObstacleData obstacleData, Transform parent) {
      GameObject obstacle = base.Initialize(obstacleData, parent);
      SpinDirection = obstacleData.RotationDirection;
      return obstacle;
    }

    public void ApplyCollisionBehaviorOnEnter(Ball ball, Collider collider) {
      if (m_IsDirty) {
        return;
      }
      Debug.Log("Enter");
      m_IsDirty = true;
      ball.Rb.velocity = ball.Rb.velocity * m_VelocityDamp;
    }

    public void ApplyCollisionBehaviorOnExit(Ball ball, Collider collider) {
      return;
    }

    public void ApplyCollisionBehaviorOnStay(Ball ball, Collider collider) {
      return;
    }
  }
}
