using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guidance.Gameplay {
  public class PathTraveledRenderer : MonoBehaviour {
    public float PositionTolerance { get; } = 0.1f;
    [SerializeField] float m_DrawDuration = 2f;
    private LineRenderer m_LineRenderer;
    private List<Vector3> m_PathPositions;
    private bool m_IsActivelyCapturingPathData = false;


    private void Awake() {
      m_LineRenderer = GetComponent<LineRenderer>();
      m_PathPositions = new List<Vector3>();
    }

    private void Start() {
      m_LineRenderer.positionCount = 0;
    }

    private void FixedUpdate() {
      if (m_IsActivelyCapturingPathData) {
        m_PathPositions.Add(transform.position);
      }
    }

    public void ClearDataCapture() => m_PathPositions.Clear();
    public bool DisableDataCapture() => m_IsActivelyCapturingPathData = false;
    public bool EnableDataCapture() => m_IsActivelyCapturingPathData = true;

    public IEnumerator DrawPathTraveledOverDuration() {
      float elapsedTime = 0f;
      int positionsToDraw;
      while (elapsedTime < m_DrawDuration) {
        positionsToDraw = Mathf.FloorToInt((elapsedTime / m_DrawDuration) * m_PathPositions.Count);
        m_LineRenderer.positionCount = positionsToDraw;
        for (int i = 0; i < positionsToDraw; i++) {
          m_LineRenderer.SetPosition(i, m_PathPositions[m_PathPositions.Count - 1 - i]);
        }
        elapsedTime += Time.deltaTime;
        yield return null;
      }
      m_LineRenderer.positionCount = m_PathPositions.Count;
      for (int i = 0; i < m_PathPositions.Count; i++) {
        m_LineRenderer.SetPosition(i, m_PathPositions[m_PathPositions.Count - 1 - i]);
      }
      m_LineRenderer.positionCount = 0;
    }
  }
}
