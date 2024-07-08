using System;
using UnityEngine;

public class BackgroundObjectNotifier : MonoBehaviour {
  public event Action OnObjectTriggerExit;
  private void OnTriggerExit(Collider other) {
    Debug.Log(other.gameObject.name);
  }
}
