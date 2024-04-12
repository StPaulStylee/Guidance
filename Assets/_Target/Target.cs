using UnityEngine;

namespace Guidance.Gameplay.Stage {
  public class Target : MonoBehaviour {
    public TargetGoalLocation GoalLocation;
    private void Awake() {
      GoalLocation = GetComponentInChildren<TargetGoalLocation>();
    }
  }
}
