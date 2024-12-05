using System;
using UnityEngine;

namespace _Background {
  public class BackgroundObjectNotifier : MonoBehaviour {
    private void OnTriggerExit(Collider other) {
      OnObjectTriggerExit?.Invoke(other.gameObject);
    }

    public event Action<GameObject> OnObjectTriggerExit;
  }
}
