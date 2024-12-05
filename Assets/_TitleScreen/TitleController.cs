using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace _TitleScreen {
  public class TitleController : MonoBehaviour {
    private const bool IS_PULSING_ACTIVE = true;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    [FormerlySerializedAs("titleMaterial_SOs")] [FormerlySerializedAs("m_TitleMaterial_SOs")] [SerializeField]
    private TitleMaterial_SO[] titleMaterialSOs;

    [FormerlySerializedAs("m_CanvasGroup")] [SerializeField]
    private CanvasGroup canvasGroup;

    [FormerlySerializedAs("m_PulseDuration")] [SerializeField]
    private float pulseDuration = 1f;

    [FormerlySerializedAs("m_FadeOutTime")] [SerializeField]
    private float fadeOutTime = 5f;

    private Coroutine _fadeCoroutine;

    private Coroutine _pulsingCoroutine;

    private void Start() {
      _pulsingCoroutine = StartCoroutine(PulseEmission());
      //m_FadeCoroutine = StartCoroutine(FadeOut());
    }

    private void OnDisable() {
      if (_pulsingCoroutine != null) {
        StopCoroutine(_pulsingCoroutine);
        foreach (TitleMaterial_SO titleMaterial in titleMaterialSOs) titleMaterial.Material.SetColor(EmissionColor, titleMaterial.DefaultColor);
      }

      if (_fadeCoroutine != null) {
        Debug.Log("No fade coroutine");
        StopCoroutine(_fadeCoroutine);
      }

      foreach (TitleMaterial_SO titleMaterial in titleMaterialSOs) {
        titleMaterial.Material.SetColor(BaseColor, titleMaterial.DefaultBaseColor);
        titleMaterial.Material.SetColor(EmissionColor, titleMaterial.DefaultColor);
      }

      if (canvasGroup != null) {
        canvasGroup.alpha = 1f;
      }
    }

    public IEnumerator PulseEmission() {
      while (IS_PULSING_ACTIVE)
        foreach (TitleMaterial_SO titleMaterial in titleMaterialSOs) {
          yield return PulseToEmissionColor(PulseColorOption.Max);
          yield return PulseToEmissionColor(PulseColorOption.Default);
        }
    }

    public IEnumerator LerpToMaxEmission() {
      //foreach (var titleMaterial in m_TitleMaterial_SOs) {
      yield return PulseToEmissionColor(PulseColorOption.Max);
      //}
    }

    private IEnumerator PulseToEmissionColor(PulseColorOption option) {
      float timeElapsed = 0f;
      while (timeElapsed < pulseDuration) {
        float t = (Mathf.Sin(timeElapsed / pulseDuration * Mathf.PI * 2 - Mathf.PI / 2) + 1) / 2;
        foreach (TitleMaterial_SO titleMaterial in titleMaterialSOs) {
          Color targetColor = option == PulseColorOption.Default ? titleMaterial.DefaultColor : titleMaterial.MaxEmissionColor;
          Color currentEmission = titleMaterial.Material.GetColor(EmissionColor);
          Color newEmissionColor = Color.Lerp(currentEmission, targetColor, t);
          titleMaterial.Material.SetColor(EmissionColor, newEmissionColor);
          timeElapsed += Time.deltaTime;
          yield return null;
        }
      }

      foreach (TitleMaterial_SO titleMaterial in titleMaterialSOs) {
        Color targetColor = option == PulseColorOption.Default ? titleMaterial.DefaultColor : titleMaterial.MaxEmissionColor;
        titleMaterial.Material.SetColor(EmissionColor, targetColor);
      }
    }

    public IEnumerator FadeOut() {
      float timeElapsed = 0f;
      while (timeElapsed < fadeOutTime)
        foreach (TitleMaterial_SO titleMaterial in titleMaterialSOs) {
          Color targetBaseColor = titleMaterial.TransparentBaseColor;
          Color currentBaseColor = titleMaterial.Material.GetColor(BaseColor);
          Color targetEmissiveColor = titleMaterial.MinEmissionColor;
          Color currentEmission = titleMaterial.Material.GetColor(EmissionColor);
          Color newBaseColor = Color.Lerp(currentBaseColor, targetBaseColor, timeElapsed / fadeOutTime);
          Color newEmissiveColor = Color.Lerp(currentEmission, targetEmissiveColor, timeElapsed / fadeOutTime);
          canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, timeElapsed / fadeOutTime);
          titleMaterial.Material.SetColor(BaseColor, newBaseColor);
          titleMaterial.Material.SetColor(EmissionColor, newEmissiveColor);
          timeElapsed += Time.deltaTime;
          yield return null;
        }

      foreach (TitleMaterial_SO titleMaterial in titleMaterialSOs) {
        Color endBaseColor = titleMaterial.TransparentBaseColor;
        Color endEmissiveColor = titleMaterial.MinEmissionColor;
        titleMaterial.Material.SetColor(BaseColor, endBaseColor);
        titleMaterial.Material.SetColor(EmissionColor, endEmissiveColor);
      }
    }

    private enum PulseColorOption {
      Default,
      Max
    }
  }
}
