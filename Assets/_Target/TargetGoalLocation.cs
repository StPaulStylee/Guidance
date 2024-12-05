using System;
using _Ball;
using UnityEngine;

namespace _Target {
  public class TargetGoalLocation : MonoBehaviour {
    private MeshCollider _meshCollider;

    public bool IsMeshColliderEnabled => _meshCollider.enabled;

    private void Awake() {
      _meshCollider = GetComponent<MeshCollider>();
    }

    private void OnCollisionEnter(Collision collision) {
      if (collision.transform.GetComponent<Ball>()) {
        OnTargetReached?.Invoke();
      }
    }

    private void OnCollisionExit(Collision collision) {
      Debug.Log(collision.gameObject.name + "collision exit");
    }

    private void OnTriggerExit(Collider other) {
      Debug.Log(other.name + "exited the target");
    }

    public event Action OnTargetReached;

    public void EnableMeshCollider() {
      _meshCollider.enabled = true;
    }

    public void DisableMeshCollider() {
      _meshCollider.enabled = false;
    }
  }
}
