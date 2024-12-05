using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Platform {
  [RequireComponent(typeof(MeshRenderer))]
  public class Platform : MonoBehaviour {
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    [FormerlySerializedAs("m_TargetColor")] [ColorUsage(true, true)] [SerializeField]
    private Color targetColor = new(2.4778893f, 2.21879411f, 2.99607873f, 1f);

    [FormerlySerializedAs("m_EmissionTransitionLengthInSeconds")] [SerializeField]
    private float emissionTransitionLengthInSeconds = 1f;

    [FormerlySerializedAs("m_IsEmissionDisabled")] [SerializeField]
    private bool isEmissionDisabled;

    private Color _currentEmission;
    private Material _material;

    private void Awake() {
      _material = GetComponent<MeshRenderer>().material;
      if (isEmissionDisabled) _material.SetColor(EmissionColor, targetColor);
    }

    public Color GetTargetColor() {
      return targetColor;
    }

    public Coroutine PerformTransitionEmission() {
      return StartCoroutine(TransitionEmission());
    }

    private IEnumerator TransitionEmission() {
      _currentEmission = _material.GetColor(EmissionColor);
      float timeElapsed = 0f;
      while (timeElapsed < emissionTransitionLengthInSeconds) {
        Color currentEmissionColor = Color.Lerp(_currentEmission, targetColor,
          timeElapsed / emissionTransitionLengthInSeconds);
        _material.SetColor(EmissionColor, currentEmissionColor);
        timeElapsed += Time.deltaTime;
        yield return null;
      }
    }
  }
}
