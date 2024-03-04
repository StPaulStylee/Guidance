using System.Linq;
using UnityEngine;

namespace Guidance.Gameplay.BackgroundGrid {
  public class WallBackgroundController : MonoBehaviour {
    // Should this be private?
    public bool IsSettingHeadWallSection = false;

    [SerializeField] private float m_MoveSpeed = 1f;
    [SerializeField] private bool m_IsMoving = false;
    [SerializeField] private GameObject m_WallSectionPrefab;

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
      // I want to try and add 4 more single wall sections to the current WallSections GO. I need to first 
      // get the position of the bottom most wall section minus another 5 units on the Y. Then place the first
      // new wall section there. Then spawn 3 more at a distance of 10 units apart for each
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
      m_CurrentHeadWallSection.transform.Translate(Vector3.up * -WALL_BACKGROUND_LENGTH, Space.Self);
      m_CurrentHeadWallSectionIndex++;
      if (m_CurrentHeadWallSectionIndex >= m_WallSections.Length) {
        m_CurrentHeadWallSectionIndex = 0;
      }
      m_CurrentHeadWallSection = m_WallSections[m_CurrentHeadWallSectionIndex];
    }
  }
}
