using System.Linq;
using UnityEngine;

namespace Guidance.Gameplay.BackgroundGrid {
  public class WallBackgroundController : MonoBehaviour {
    public bool IsSettingHeadWallSection = false;

    [SerializeField] private float m_MoveSpeed = 1f;
    [SerializeField] private bool m_IsMoving = false;
    [SerializeField] private GameObject m_WallSectionsPrefab;

    private WallSection[] m_WallSections = null;
    private WallSection m_CurrentHeadWallSection;
    private int m_CurrentHeadWallSectionIndex;

    private const int INCREMENT_DISTANCE = 10;
    private const int WALL_BACKGROUND_LENGTH = 40;
    private float m_YDistanceTraveled = 0f;

    public int WallBackgroundLength => WALL_BACKGROUND_LENGTH;

    private void Awake() {
      m_WallSections = GetComponentsInChildren<WallSection>().OrderBy(section => section.Id).ToArray();
      m_CurrentHeadWallSection = m_WallSections[0];
      m_CurrentHeadWallSectionIndex = 0;
      IsSettingHeadWallSection = true;
    }

    private void Update() {
      MoveWallBackground();
    }

    public void AttachNewWall() {
      Vector3 location = new Vector3(transform.position.x, transform.position.y - WALL_BACKGROUND_LENGTH, transform.position.z);
      Instantiate(m_WallSectionsPrefab, location, Quaternion.identity, transform);
    }



    private void MoveWallBackground() {
      if (!m_IsMoving) {
        return;
      }
      if (m_YDistanceTraveled > INCREMENT_DISTANCE && IsSettingHeadWallSection) {
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
