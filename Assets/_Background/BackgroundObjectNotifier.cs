using System;
using UnityEngine;

public class BackgroundObjectNotifier : MonoBehaviour {
  public event Action<GameObject> OnObjectTriggerExit;
  private void OnTriggerExit(Collider other) {
    OnObjectTriggerExit?.Invoke(other.gameObject);
  }
}
