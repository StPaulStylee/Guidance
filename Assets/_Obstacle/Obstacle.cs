using Guidance.Data;
using Guidance.Stage.Data;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  public abstract class Obstacle : MonoBehaviour {
    public ObstacleType TypeId;
    [OdinSerialize][ShowInInspector] public int? LinkId;
    protected ICollisionBehavior CollisionBehavior;
    protected readonly float m_YObstacleSpawnDistance = Constants.Y_STAGE_LENGTH;
    [SerializeField] protected GameObject m_ObstalePrefab;

    public virtual GameObject Initialize(ObstacleData obstacleData, Transform parent) {
      float xPos = obstacleData.Position.X;
      float yPos = obstacleData.Position.Y - m_YObstacleSpawnDistance;
      float zPos = obstacleData.Position.Z;
      Vector3 spawnLocation = new Vector3(xPos, yPos, zPos);
      Quaternion spawnRoation = Quaternion.Euler(0, 0, obstacleData.Rotation);
      GameObject obstacle = Instantiate(m_ObstalePrefab, spawnLocation, spawnRoation, parent);
      obstacle.transform.localScale = new Vector3(obstacleData.Scale, 1f, 1f);
      LinkId = obstacleData.LinkId;
      return obstacle;
    }

    public virtual GameObject InitializeDebug(ObstacleData obstacleData, Transform parent) {
      float xPos = obstacleData.Position.X;
      float yPos = obstacleData.Position.Y;
      float zPos = obstacleData.Position.Z;
      Vector3 spawnLocation = new Vector3(xPos, yPos, zPos);
      Quaternion spawnRoation = Quaternion.Euler(0, 0, obstacleData.Rotation);
      GameObject obstacle = Instantiate(m_ObstalePrefab, spawnLocation, spawnRoation, parent);
      obstacle.transform.localScale = new Vector3(obstacleData.Scale, 1f, 1f);
      LinkId = obstacleData.LinkId;
      return obstacle;
    }

    private void OnCollisionEnter(Collision collision) {
      if (CollisionBehavior != null && collision.transform.TryGetComponent(out Ball ball)) {
        CollisionBehavior.ApplyCollisionBehaviorOnEnter(ball, collision.collider);
      }
    }

    private void OnCollisionStay(Collision collision) {
      if (CollisionBehavior != null && collision.transform.TryGetComponent(out Ball ball)) {
        CollisionBehavior.ApplyCollisionBehaviorOnStay(ball, collision.collider);
      }
    }

    private void OnTriggerStay(Collider collider) {
      if (CollisionBehavior != null && collider.transform.TryGetComponent(out Ball ball)) {
        CollisionBehavior.ApplyCollisionBehaviorOnStay(ball, collider);
      }
    }
  }
}
