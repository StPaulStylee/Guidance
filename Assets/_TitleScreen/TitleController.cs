using System.Collections;
using UnityEngine;

namespace Guidance.Title {
  public class TitleController : MonoBehaviour {
    [SerializeField] private TitleMaterial_SO[] m_TitleMaterial_SOs;
    [SerializeField] private GameObject m_GuidanceText;
    [SerializeField] float m_PulseDuration = 1f;
    private Coroutine m_PulsingCoroutine;
    private bool isPulsingActive = true;

    private enum PulseColorOption {
      Default, Max
    }

    private void Start() {
      m_PulsingCoroutine = StartCoroutine(PulseEmission());
    }



    private IEnumerator PulseEmission() {
      while (isPulsingActive) {
        foreach (var titleMaterial in m_TitleMaterial_SOs) {
          yield return PulseToEmissionColor(PulseColorOption.Max);
          yield return PulseToEmissionColor(PulseColorOption.Default);
        }
      }
    }

    private IEnumerator PulseToEmissionColor(PulseColorOption option) {
      float timeElapsed = 0f;
      while (timeElapsed < m_PulseDuration) {
        float t = (Mathf.Sin((timeElapsed / m_PulseDuration) * Mathf.PI * 2 - Mathf.PI / 2) + 1) / 2;
        foreach (var titleMaterial in m_TitleMaterial_SOs) {
          Color targetColor = option == PulseColorOption.Default ? titleMaterial.DefaultColor : titleMaterial.MaxEmissionColor;
          Color currentEmission = titleMaterial.Material.GetColor("_EmissionColor");
          Color newEmissionColor = Color.Lerp(currentEmission, targetColor, t);
          titleMaterial.Material.SetColor("_EmissionColor", newEmissionColor);
          timeElapsed += Time.deltaTime;
          yield return null;
        }
      }
      foreach (var titleMaterial in m_TitleMaterial_SOs) {
        Color targetColor = option == PulseColorOption.Default ? titleMaterial.DefaultColor : titleMaterial.MaxEmissionColor;
        titleMaterial.Material.SetColor("_EmissionColor", targetColor);
      }
    }

    private void OnDisable() {
      if (m_PulsingCoroutine != null) {
        StopCoroutine(m_PulsingCoroutine);
        foreach (var titleMaterial in m_TitleMaterial_SOs) {
          titleMaterial.Material.SetColor("_EmissionColor", titleMaterial.DefaultColor);
        }
      }
    }
  }
}
