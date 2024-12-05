using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Ball {
  public class PathTraveledRenderer : MonoBehaviour {
    [FormerlySerializedAs("m_DrawDuration")] [SerializeField]
    private float drawDuration = 2f;

    private bool _isActivelyCapturingPathData;
    private LineRenderer _lineRenderer;
    private List<Vector3> _pathPositions;
    public float PositionTolerance { get; } = 0.1f;

    private void Awake() {
      _lineRenderer = GetComponent<LineRenderer>();
      _pathPositions = new List<Vector3>();
    }

    private void Start() {
      _lineRenderer.positionCount = 0;
    }

    private void FixedUpdate() {
      if (_isActivelyCapturingPathData) _pathPositions.Add(transform.position);
    }

    public void ClearDataCapture() {
      _pathPositions.Clear();
    }

    public bool DisableDataCapture() {
      return _isActivelyCapturingPathData = false;
    }

    public bool EnableDataCapture() {
      return _isActivelyCapturingPathData = true;
    }

    public IEnumerator DrawPathTraveledOverDuration() {
      float elapsedTime = 0f;
      int positionsToDraw;
      while (elapsedTime < drawDuration) {
        positionsToDraw = Mathf.FloorToInt(elapsedTime / drawDuration * _pathPositions.Count);
        _lineRenderer.positionCount = positionsToDraw;
        for (int i = 0; i < positionsToDraw; i++)
          _lineRenderer.SetPosition(i, _pathPositions[_pathPositions.Count - 1 - i]);
        elapsedTime += Time.deltaTime;
        yield return null;
      }

      _lineRenderer.positionCount = _pathPositions.Count;
      for (int i = 0; i < _pathPositions.Count; i++)
        _lineRenderer.SetPosition(i, _pathPositions[_pathPositions.Count - 1 - i]);
      _lineRenderer.positionCount = 0;
    }
  }
}
