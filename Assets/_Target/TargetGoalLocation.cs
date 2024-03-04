using System;
using UnityEngine;

namespace Guidance.Gameplay {
  public class TargetGoalLocation : MonoBehaviour {
    public static event Action OnTargetReached;
    private void OnCollisionEnter(Collision collision) {
      if (collision.transform.GetComponent<Ball>()) {
        OnTargetReached?.Invoke();
      }
    }
  }

}
