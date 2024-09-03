using System.Collections;
using UnityEngine;

namespace Guidance.Title {
  public class TitleController : MonoBehaviour {
    [SerializeField] private TitleMaterial_SO[] m_TitleMaterial_SOs;
    [SerializeField] private GameObject m_GuidanceText;
    [SerializeField] float m_PulseDuration = .25f;
    private Coroutine m_PulsingCoroutine;
    private bool isPulsingActive = true;
    //private float m_PulsePhaseDuration;

    // Before I continue I need to make a copy of all the materials so I don't actually edit 
    // the provided materials because those changes are permanent.

    private void Awake() {
      //m_PulsePhaseDuration = m_PulseDuration / 2;
    }

    private void Start() {
      //m_PulsingCoroutine = StartCoroutine(PulseEmission());
    }

    private IEnumerator PulseEmission() {
      while (isPulsingActive) {
        foreach (var titleMaterial in m_TitleMaterial_SOs) {
          yield return PulseToEmissionColor(titleMaterial.MaxEmissionColor);
          yield return PulseToEmissionColor(titleMaterial.DefaultColor);
        }
      }
    }

    private IEnumerator PulseToEmissionColor(Color targetColor) {
      foreach (var titleMaterial in m_TitleMaterial_SOs) {
        Color currentEmission = titleMaterial.Material.GetColor("_EmissionColor");
        float timeElapsed = 0f;
        while (timeElapsed < m_PulseDuration) {
          Color newEmissionColor = Color.Lerp(currentEmission, targetColor, timeElapsed / m_PulseDuration);
          titleMaterial.Material.SetColor("_EmissionColor", newEmissionColor);
          timeElapsed += Time.deltaTime;
          yield return null;
        }
        titleMaterial.Material.SetColor("_EmissionColor", targetColor);
      }
    }

    private void OnDisable() {
      if (m_PulsingCoroutine != null) {
        StopCoroutine(m_PulsingCoroutine);
      }
    }
  }
}
