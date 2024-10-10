using UnityEngine;

namespace Guidance.Gameplay.BackgroundGrid {
  public class WallBackgroundController : MonoBehaviour {
    private static WallBackgroundController instance;
    private void Awake() {
      if (instance != null) {
        Destroy(gameObject);
        return;
      }
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    //  public bool IsCreatingNewWallSection { get; private set; } = false;

    //  [SerializeField] private float m_MoveSpeed = 1f;
    //  [SerializeField] private bool m_IsMoving = false;
    //  [SerializeField] private GameObject m_WallSectionPrefab;

    //  private List<WallSection> m_WallSections = null;
    //  [SerializeField] private WallSection m_CurrentHeadWallSection;
    //  private int m_CurrentHeadWallSectionIndex;
    //  [SerializeField] private WallSection m_CurrentTailWallSection;
    //  private WallSectionsContainer m_WallSectionsContainer;
    //  private float m_OldWallYPositionRemovalPoint;
    //  private float m_YDistanceTraveled = 0f;

    //  private const int INCREMENT_DISTANCE = 40;
    //  private const int WALL_BACKGROUND_LENGTH = 160;
    //  private const float WALL_SECTION_OFFSET = 5f;
    //  private const int NUMBER_OF_WALL_SECTIONS_TO_REMOVE = 4;
    //  private const float WALL_SECTION_Y_SCALE = 40f;

    //  public int WallBackgroundLength => WALL_BACKGROUND_LENGTH;

    //  private void Awake() {
    //    m_WallSectionsContainer = GetComponentInChildren<WallSectionsContainer>();
    //    m_WallSections = GetComponentsInChildren<WallSection>().OrderBy(section => section.Id).ToList();
    //    m_CurrentHeadWallSection = m_WallSections[0];
    //    m_CurrentTailWallSection = m_WallSections[m_WallSections.Count - 1];
    //    m_CurrentHeadWallSectionIndex = 0;
    //  }

    //  private void Update() {
    //    MoveWallBackground();
    //  }

    //  // Deprecated
    //  public void ExecuteNewWallAttachmentProcedure() {
    //    IsCreatingNewWallSection = true;
    //    AttachNewWallSection();
    //    m_OldWallYPositionRemovalPoint = m_CurrentTailWallSection.transform.position.y - WALL_SECTION_OFFSET;
    //  }

    //  // Deprecated
    //  public IEnumerator ManageWallSectionsAfterAddition() {
    //    // Maybe add a flag to WallSection component isHead/isTail???
    //    m_CurrentHeadWallSection = m_WallSections[4];
    //    m_CurrentTailWallSection = m_WallSections[7];

    //    while (m_CurrentTailWallSection.transform.position.y < m_OldWallYPositionRemovalPoint) {
    //      yield return null;
    //    }
    //    RemoveOldWallSections();

    //    // This is needed here to ensure proper setting in "SetCurrentHeadWallSection"
    //    m_CurrentHeadWallSectionIndex = 0;

    //    IsCreatingNewWallSection = false;
    //  }

    //  // Deprecated
    //  private void RemoveOldWallSections() {
    //    // Perhaps I could add a property to the WallSection like a "isOld" flag
    //    // and I can filter using LINQ
    //    for (int i = 0; i < NUMBER_OF_WALL_SECTIONS_TO_REMOVE; i++) {
    //      // Always remove [0] because you've popped off the first element in the prior iteration
    //      WallSection sectionToRemove = m_WallSections[0];
    //      m_WallSections.Remove(sectionToRemove);
    //      Destroy(sectionToRemove.gameObject);
    //    }
    //  }

    //  // Deprecated
    //  private void AttachNewWallSection() {
    //    Vector3 seedPosition = m_CurrentTailWallSection.transform.position;
    //    for (int i = 0; i < NUMBER_OF_WALL_SECTIONS_TO_REMOVE; i++) {
    //      Vector3 spawnPosition = new Vector3(seedPosition.x, seedPosition.y - ((i + 1) * WALL_SECTION_Y_SCALE), seedPosition.z);
    //      GameObject wallSectionGO = Instantiate(m_WallSectionPrefab, spawnPosition, Quaternion.identity, m_WallSectionsContainer.transform);
    //      WallSection wallSection = wallSectionGO.GetComponent<WallSection>();
    //      wallSectionGO.name = $"WallSection_{m_WallSections.Count}";
    //      wallSection.Id = m_WallSections.Count;
    //      m_WallSections.Add(wallSection);
    //    }
    //  }

    //  private void CreateWallSection() {
    //    Vector3 seedPosition = m_CurrentTailWallSection.transform.position;
    //    // This logic isn't right. The newly spawned wall sections are not getting placed in the right position
    //    Vector3 spawnPosition = new Vector3(seedPosition.x, seedPosition.y - WALL_SECTION_Y_SCALE, seedPosition.z);
    //    GameObject wallSectionGO = Instantiate(m_WallSectionPrefab, spawnPosition, Quaternion.identity, m_WallSectionsContainer.transform);
    //    WallSection wallSection = wallSectionGO.GetComponent<WallSection>();
    //    wallSectionGO.name = $"WallSection_{m_WallSections.Count}";
    //    wallSection.Id = m_WallSections.Count;
    //    m_WallSections.Add(wallSection);
    //    m_CurrentTailWallSection = m_WallSections[m_WallSections.Count - 1];
    //  }

    //  private void MoveWallBackground() {
    //    if (!m_IsMoving) {
    //      return;
    //    }
    //    if (m_YDistanceTraveled > INCREMENT_DISTANCE && !IsCreatingNewWallSection) {
    //      m_YDistanceTraveled = 0f;
    //      SetCurrentHeadWallSection();
    //      CreateWallSection();
    //    }
    //    Vector3 translation = Vector3.up * m_MoveSpeed * Time.deltaTime;
    //    m_YDistanceTraveled += translation.y;
    //    transform.Translate(translation);
    //  }

    //  private void SetCurrentHeadWallSection() {
    //    //m_CurrentHeadWallSection.transform.Translate(Vector3.up * -WALL_BACKGROUND_LENGTH, Space.Self);
    //    Destroy(m_CurrentHeadWallSection.gameObject);
    //    //m_CurrentTailWallSection = m_CurrentHeadWallSection;
    //    m_CurrentHeadWallSectionIndex++;
    //    if (m_CurrentHeadWallSectionIndex >= m_WallSections.Count) {
    //      m_CurrentHeadWallSectionIndex = 0;
    //    }
    //    m_CurrentHeadWallSection = m_WallSections[m_CurrentHeadWallSectionIndex];
    //  }
  }
}
