using _Ball;
using _Ball.Obstacles;
using _Data;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Obstacle {
  public abstract class Obstacle : MonoBehaviour {
    private const float Y_OBSTACLE_SPAWN_DISTANCE = Constants.Y_STAGE_LENGTH;
    public ObstacleType TypeId;

    [FormerlySerializedAs("obstalePrefab")] [FormerlySerializedAs("m_ObstalePrefab")] [SerializeField]
    protected GameObject obstaclePrefab;

    protected ICollisionBehavior CollisionBehavior;

    [OdinSerialize] [ShowInInspector]
    public int? LinkId;

    private void OnCollisionEnter(Collision collision) {
      if (CollisionBehavior != null && collision.transform.TryGetComponent(out Ball ball))
        CollisionBehavior.ApplyCollisionBehaviorOnEnter(ball, collision.collider);
    }

    private void OnCollisionExit(Collision collision) {
      if (CollisionBehavior != null && collision.transform.TryGetComponent(out Ball ball))
        CollisionBehavior.ApplyCollisionBehaviorOnExit(ball, collision.collider);
    }

    private void OnCollisionStay(Collision collision) {
      if (CollisionBehavior != null && collision.transform.TryGetComponent(out Ball ball))
        CollisionBehavior.ApplyCollisionBehaviorOnStay(ball, collision.collider);
    }

    private void OnTriggerStay(Collider col) {
      if (CollisionBehavior != null && col.transform.TryGetComponent(out Ball ball))
        CollisionBehavior.ApplyCollisionBehaviorOnStay(ball, col);
    }

    public virtual GameObject Initialize(ObstacleData obstacleData, Transform parent) {
      float xPos = obstacleData.Position.X;
      float yPos = obstacleData.Position.Y - Y_OBSTACLE_SPAWN_DISTANCE;
      float zPos = obstacleData.Position.Z;
      Vector3 spawnLocation = new(xPos, yPos, zPos);
      Quaternion spawnRoation = Quaternion.Euler(0, 0, obstacleData.Rotation);
      GameObject obstacle = Instantiate(obstaclePrefab, spawnLocation, spawnRoation, parent);
      obstacle.transform.localScale = new Vector3(obstacleData.Scale, 1f, 1f);
      LinkId = obstacleData.LinkId;
      return obstacle;
    }

    public virtual GameObject InitializeDebug(ObstacleData obstacleData, Transform parent) {
      float xPos = obstacleData.Position.X;
      float yPos = obstacleData.Position.Y;
      float zPos = obstacleData.Position.Z;
      Vector3 spawnLocation = new(xPos, yPos, zPos);
      Quaternion spawnRotation = Quaternion.Euler(0, 0, obstacleData.Rotation);
      GameObject obstacle = Instantiate(obstaclePrefab, spawnLocation, spawnRotation, parent);
      obstacle.transform.localScale = new Vector3(obstacleData.Scale, 1f, 1f);
      LinkId = obstacleData.LinkId;
      return obstacle;
    }
  }
}
