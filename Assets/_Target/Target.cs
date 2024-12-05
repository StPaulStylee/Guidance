using UnityEngine;

namespace _Target {
  public class Target : MonoBehaviour {
    public TargetGoalLocation GoalLocation;

    private void Awake() {
      GoalLocation = GetComponentInChildren<TargetGoalLocation>();
    }
  }
}
