using UnityEngine;

namespace Guidance.Gameplay.Targets {
  public class Target : MonoBehaviour {
    public TargetGoalLocation GoalLocation;
    private void Awake() {
      GoalLocation = GetComponentInChildren<TargetGoalLocation>();
    }
  }
}
