using System.Collections;
using UnityEngine;

namespace Guidance.Gameplay {
  [RequireComponent(typeof(MeshRenderer))]
  public class Platform : MonoBehaviour {
    [ColorUsage(true, true)]
    [SerializeField] private Color m_TargetColor = new Color(2.4778893f, 2.21879411f, 2.99607873f, 1f);
    [SerializeField] private float m_EmissionTransitionLengthInSeconds = 1f;
    [SerializeField] private bool m_IsEmissionDisabled = false;
    private Color m_CurrentEmission;
    private Material m_Material;

    public Color GetTargetColor() => m_TargetColor;

    private void Awake() {
      m_Material = GetComponent<MeshRenderer>().material;
      if (m_IsEmissionDisabled) {
        m_Material.SetColor("_EmissionColor", m_TargetColor);
      }
    }

    public Coroutine PerformTransitionEmission() {
      return StartCoroutine(TransitionEmission());
    }

    private IEnumerator TransitionEmission() {
      m_CurrentEmission = m_Material.GetColor("_EmissionColor");
      float timeElapsed = 0f;
      while (timeElapsed < m_EmissionTransitionLengthInSeconds) {
        Color currentEmissionColor = Color.Lerp(m_CurrentEmission, m_TargetColor, timeElapsed / m_EmissionTransitionLengthInSeconds);
        m_Material.SetColor("_EmissionColor", currentEmissionColor);
        timeElapsed += Time.deltaTime;
        yield return null;
      }
    }
  }
}
