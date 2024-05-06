using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  [CreateAssetMenu(fileName = "Obstacle", menuName = "ScriptableObjects/Obstacle", order = 1)]
  public class Obstacle_SO : ScriptableObject {
    [field: SerializeField] public ObstacleType TypeId { get; set; }
    public GameObject Prefab;
    [SerializeField] private Obstacle Obstacle;
  }
}
