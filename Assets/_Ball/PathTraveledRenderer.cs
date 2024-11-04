using UnityEngine;

namespace Guidance.Gameplay {
  public class PathTraveledRenderer : MonoBehaviour {
    public float PositionTolerance { get; } = 0.1f;
    private LineRenderer m_LineRenderer;
    private void Awake() {
      m_LineRenderer = GetComponent<LineRenderer>();
      m_LineRenderer.enabled = false;
    }
  }
}
