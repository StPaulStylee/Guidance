using System;
using UnityEngine;

namespace Guidance.Gameplay.Targets {
  public class TargetGoalLocation : MonoBehaviour {
    public event Action OnTargetReached;
    private void OnCollisionEnter(Collision collision) {
      if (collision.transform.GetComponent<Ball>()) {
        OnTargetReached?.Invoke();
      }
    }
  }
}
