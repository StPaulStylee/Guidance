using UnityEngine;



namespace Guidance.Title {
  [CreateAssetMenu(fileName = "Title Material", menuName = "ScriptableObjects/Title Material", order = 2)]

  public class TitleMaterial_SO : ScriptableObject {
    [SerializeField] private Material m_Material;
    [SerializeField][ColorUsage(false, true)] private Color m_DefaultColor;
    [SerializeField][ColorUsage(false, true)] private Color m_MaxEmissionColor;
  }
}
