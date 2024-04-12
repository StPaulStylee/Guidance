using UnityEngine;

namespace Guidance.Gameplay.Obstacles {
  [CreateAssetMenu(fileName = "Obstacle", menuName = "ScriptableObjects/Obstacle", order = 1)]
  public class Obstacle_SO : ScriptableObject {
    public int TypeId { get; set; }
    public GameObject Prefab;
    private Obstacle Obstacle;

    private void Awake() {
      Obstacle = Prefab.GetComponent<Obstacle>();
      if (Obstacle == null) {
        Debug.LogError($"No Obstacle component on {this.name}!!!");
        return;
      }
      TypeId = Obstacle.TypeId;
    }
  }
}