using UnityEngine;

namespace Guidance.Gameplay {
  public class Platform : MonoBehaviour {
    [ColorUsage(true, true)]
    [SerializeField] private Color m_TargetColor = new Color(2.4778893f, 2.21879411f, 2.99607873f, 1f);
    public Color GetTargetColor() => m_TargetColor;
  }
}
