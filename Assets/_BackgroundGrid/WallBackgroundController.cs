using System.Linq;
using UnityEngine;

namespace Guidance.Gameplay.BackgroundGrid {
  public class WallBackgroundController : MonoBehaviour {
    [SerializeField] private float m_MoveSpeed = 1f;

    private WallSection[] m_WallSections = null;
    private WallSection m_CurrentHeadWallSection;
    private int m_CurrentHeadWallSectionIndex;

    private const int INCREMENT_DISTANCE = 10;
    private const int WALL_BACKGROUND_LENGTH = 40;
    private float m_YDistanceTraveled = 0f;

    private void Awake() {
      m_WallSections = GetComponentsInChildren<WallSection>().OrderBy(section => section.Id).ToArray();
      m_CurrentHeadWallSection = m_WallSections[0];
      m_CurrentHeadWallSectionIndex = 0;
    }

    private void Update() {
      MoveWallBackground();
    }

    private void MoveWallBackground() {
      if (m_YDistanceTraveled > INCREMENT_DISTANCE) {
        m_YDistanceTraveled = 0f;
        SetCurrentHeadWallSection();
      }
      Vector3 translation = Vector3.up * m_MoveSpeed * Time.deltaTime;
      m_YDistanceTraveled += translation.y;
      transform.Translate(translation);
    }

    private void SetCurrentHeadWallSection() {
      // Vector3.forward is used here because given the walls current rotation the walls z-axis is pointed in the global y direction.
      // When this is ready bring the walls into blender as reset their transforms and then change this to Vector3.up
      m_CurrentHeadWallSection.transform.Translate(Vector3.forward * -WALL_BACKGROUND_LENGTH, Space.Self);
      m_CurrentHeadWallSectionIndex++;
      if (m_CurrentHeadWallSectionIndex >= m_WallSections.Length) {
        m_CurrentHeadWallSectionIndex = 0;
      }
      m_CurrentHeadWallSection = m_WallSections[m_CurrentHeadWallSectionIndex];
    }
  }
}
