using System.Collections;
using UnityEngine;

namespace _Ball {
  public static class BallDissolveManager {
    private static readonly float VerticalDissolveTime = 2.0f;
    private static readonly int DisAmount = Shader.PropertyToID("_DisAmount");

    public static IEnumerator PerformVerticalDissolveDown(Material ballMaterial) {
      float elapsedTime = 0f;
      ballMaterial.SetFloat(DisAmount, -2.0f);
      while (elapsedTime < VerticalDissolveTime) {
        ballMaterial.SetFloat(DisAmount, Mathf.Lerp(-2.0f, 2.0f, elapsedTime / VerticalDissolveTime));
        elapsedTime += Time.deltaTime;
        yield return null;
      }

      ballMaterial.SetFloat(DisAmount, 2.0f);
    }

    public static IEnumerator PerformVerticalDissolveUp(Material ballMaterial) {
      float elapsedTime = 0f;
      ballMaterial.SetFloat(DisAmount, 2.0f);
      while (ballMaterial.GetFloat(DisAmount) > -2.0f) {
        ballMaterial.SetFloat(DisAmount, Mathf.Lerp(2.0f, -2.0f, elapsedTime / VerticalDissolveTime));
        elapsedTime += Time.deltaTime;
        yield return null;
      }

      ballMaterial.SetFloat(DisAmount, -2.0f);
    }

    public static void SetDissolved(Material ballMaterial, bool isDissolved) {
      if (isDissolved) {
        ballMaterial.SetFloat(DisAmount, -2.0f);
        return;
      }

      ballMaterial.SetFloat(DisAmount, 2.0f);
    }
  }
}
