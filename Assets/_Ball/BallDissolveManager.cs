using System.Collections;
using UnityEngine;

namespace Guidance.Gameplay.Game.Manager {
  public static class BallDissolveManager {
    private static float VerticalDissolveTime = 2.0f;

    public static IEnumerator PerformVerticalDissolveDown(Material ballMaterial) {
      float elapsedTime = 0f;
      ballMaterial.SetFloat("_DisAmount", -2.0f);
      while (elapsedTime < VerticalDissolveTime) {
        ballMaterial.SetFloat("_DisAmount", Mathf.Lerp(-2.0f, 2.0f, elapsedTime / VerticalDissolveTime));
        //float amountToAdd = Mathf.Lerp(-2.0f, 2.0f, elapsedTime / VerticalDissolveTime);
        elapsedTime += Time.deltaTime;
        yield return null;
      }
      ballMaterial.SetFloat("_DisAmount", 2.0f);
      //m_BallMaterial.SetFloat("_DisAmount", 2.0f);
      //while (m_BallMaterial.GetFloat("_DisAmount") > -2.0f) {
      //  //float amountToAdd = Mathf.Lerp(2.0f, -2.0f, elapsedTime / VerticalDissolveTime);
      //  m_BallMaterial.SetFloat("_DisAmount", Mathf.Lerp(2.0f, -2.0f, elapsedTime / VerticalDissolveTime));
      //  elapsedTime += Time.deltaTime;
      //  yield return null;
      //}
    }

    public static IEnumerator PerformVerticalDissolveUp(Material ballMaterial) {
      float elapsedTime = 0f;
      ballMaterial.SetFloat("_DisAmount", 2.0f);
      while (ballMaterial.GetFloat("_DisAmount") > -2.0f) {
        //float amountToAdd = Mathf.Lerp(2.0f, -2.0f, elapsedTime / VerticalDissolveTime);
        ballMaterial.SetFloat("_DisAmount", Mathf.Lerp(2.0f, -2.0f, elapsedTime / VerticalDissolveTime));
        elapsedTime += Time.deltaTime;
        yield return null;
      }
      ballMaterial.SetFloat("_DisAmount", -2.0f);
    }

    public static void SetDissolved(Material ballMaterial, bool isDissolved) {
      if (isDissolved) {
        ballMaterial.SetFloat("_DisAmount", -2.0f);
        return;
      }
      ballMaterial.SetFloat("_DisAmount", 2.0f);
    }
  }
}
