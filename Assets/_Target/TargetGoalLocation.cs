using System;
using UnityEngine;

namespace Guidance.Gameplay.Stage {
  public class TargetGoalLocation : MonoBehaviour {
    public event Action OnTargetReached;
    private MeshCollider m_MeshCollider;

    public bool IsMeshColliderEnabled => m_MeshCollider.enabled;

    private void Awake() {
      m_MeshCollider = GetComponent<MeshCollider>();
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

    public void EnableMeshCollider() {
      m_MeshCollider.enabled = true;
    }

    public void DisableMeshCollider() {
      m_MeshCollider.enabled = false;
    }
  }
}
