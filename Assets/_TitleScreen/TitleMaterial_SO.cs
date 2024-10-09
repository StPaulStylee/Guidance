using UnityEngine;



namespace Guidance.Title {
  [CreateAssetMenu(fileName = "Title Material", menuName = "ScriptableObjects/Title Material", order = 2)]

  public class TitleMaterial_SO : ScriptableObject {
    public Material Material => m_Material;
    public Color DefaultColor => m_DefaultColor;
    public Color MaxEmissionColor => m_MaxEmissionColor;
    public Color DefaultBaseColor => m_DefaultBaseColor;
    public Color TransparentBaseColor => m_TransparentBaseColor;
    [SerializeField] private Material m_Material;
    [SerializeField][ColorUsage(false, true)] private Color m_DefaultColor;
    [SerializeField][ColorUsage(false, true)] private Color m_MaxEmissionColor;
    [SerializeField] private Color m_DefaultBaseColor;
    [SerializeField] private Color m_TransparentBaseColor;
  }
}
