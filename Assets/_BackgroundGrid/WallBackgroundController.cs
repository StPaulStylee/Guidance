using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Guidance.Gameplay.BackgroundGrid {
  public class WallBackgroundController : MonoBehaviour {
    public bool IsCreatingNewWallSection { get; set; } = false;

    [SerializeField] private float m_MoveSpeed = 1f;
    [SerializeField] private bool m_IsMoving = false;
    [SerializeField] private GameObject m_WallSectionPrefab;

    private List<WallSection> m_WallSections = null;
    private WallSection m_CurrentHeadWallSection;
    private int m_CurrentHeadWallSectionIndex;
    private WallSection m_CurrentTailWallSection;
    private int m_CurrentTailWallSectionIndex;
    private WallSectionsContainer m_WallSectionsContainer;

    private const int INCREMENT_DISTANCE = 10;
    private const int WALL_BACKGROUND_LENGTH = 40;
    private float m_YDistanceTraveled = 0f;

    public int WallBackgroundLength => WALL_BACKGROUND_LENGTH;

    private void Awake() {
      m_WallSectionsContainer = GetComponentInChildren<WallSectionsContainer>();
      m_WallSections = GetComponentsInChildren<WallSection>().OrderBy(section => section.Id).ToList();
      m_CurrentHeadWallSection = m_WallSections[0];
      m_CurrentTailWallSection = m_WallSections[m_WallSections.Count - 1];
      m_CurrentHeadWallSectionIndex = 0;
    }

    private void Update() {
      MoveWallBackground();
    }

    public void ExecuteNewWallProcedure() {
      AttachNewWallSection();
      StartCoroutine(TrackWallBackgroundMovement());

      // I am debugging why this coroutine isn't working as expected. Currently it starts
      // and then stops almost instantaneously, which isn't correct. Also, it seems like the
      // remove logic isn't working correctly either. I need to ensure that the items in 
      // the list are in the correct order.

      //RemoveOldWallSections();
      //IsCreatingNewWallSection = false;
    }

    private void RemoveOldWallSections() {
      // Perhaps I could add a property to the WallSection like a "isOld" flag
      // and I can filter using LINQ
      for (int i = 0; i <= 3; i++) {
        WallSection sectionToRemove = m_WallSections[i];
        m_WallSections.Remove(sectionToRemove);
        Destroy(sectionToRemove.gameObject);
      }
    }

    private void AttachNewWallSection() {
      Vector3 seedPosition = m_CurrentTailWallSection.transform.position;
      for (int i = 0; i <= 3; i++) {
        Vector3 spawnPosition = new Vector3(seedPosition.x, seedPosition.y - ((i + 1) * 10f), seedPosition.z);
        GameObject wallSectionGO = Instantiate(m_WallSectionPrefab, spawnPosition, Quaternion.identity, m_WallSectionsContainer.transform);
        WallSection wallSection = wallSectionGO.GetComponent<WallSection>();
        wallSectionGO.name = $"WallSection_{m_WallSections.Count}";
        wallSection.Id = m_WallSections.Count;
        m_WallSections.Add(wallSection);
      }
    }

    private IEnumerator TrackWallBackgroundMovement() {
      float distanceTraveled = 0f;
      float yPositionAtStart = transform.position.y;
      Debug.Log("Tracking");
      while (distanceTraveled < WALL_BACKGROUND_LENGTH) {
        distanceTraveled += transform.position.y - yPositionAtStart;
        yield return null;
      }
      Debug.Log("Done");
      //IsCreatingNewWallSection = false;
    }

    private void MoveWallBackground() {
      if (!m_IsMoving) {
        return;
      }
      if (m_YDistanceTraveled > INCREMENT_DISTANCE && !IsCreatingNewWallSection) {
        m_YDistanceTraveled = 0f;
        SetCurrentHeadWallSection();
      }
      Vector3 translation = Vector3.up * m_MoveSpeed * Time.deltaTime;
      m_YDistanceTraveled += translation.y;
      transform.Translate(translation);
    }

    private void SetCurrentHeadWallSection() {
      m_CurrentHeadWallSection.transform.Translate(Vector3.up * -WALL_BACKGROUND_LENGTH, Space.Self);
      m_CurrentTailWallSection = m_CurrentHeadWallSection;
      m_CurrentTailWallSectionIndex = m_CurrentHeadWallSectionIndex;
      m_CurrentHeadWallSectionIndex++;
      if (m_CurrentHeadWallSectionIndex >= m_WallSections.Count) {
        m_CurrentHeadWallSectionIndex = 0;
      }
      m_CurrentHeadWallSection = m_WallSections[m_CurrentHeadWallSectionIndex];
    }
  }
}
