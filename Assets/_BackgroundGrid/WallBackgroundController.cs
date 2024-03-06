using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Guidance.Gameplay.BackgroundGrid {
  public class WallBackgroundController : MonoBehaviour {
    public bool IsCreatingNewWallSection { get; private set; } = false;

    [SerializeField] private float m_MoveSpeed = 1f;
    [SerializeField] private bool m_IsMoving = false;
    [SerializeField] private GameObject m_WallSectionPrefab;

    private List<WallSection> m_WallSections = null;
    [SerializeField] private WallSection m_CurrentHeadWallSection;
    private int m_CurrentHeadWallSectionIndex;
    [SerializeField] private WallSection m_CurrentTailWallSection;
    private int m_CurrentTailWallSectionIndex;
    private WallSectionsContainer m_WallSectionsContainer;

    private const int INCREMENT_DISTANCE = 10;
    private const int WALL_BACKGROUND_LENGTH = 40;
    private float m_YDistanceTraveled = 0f;

    private float temporaryYTargetPosition;

    private Camera m_Camera;

    public int WallBackgroundLength => WALL_BACKGROUND_LENGTH;

    private void Awake() {
      m_Camera = Camera.main;
      m_WallSectionsContainer = GetComponentInChildren<WallSectionsContainer>();
      m_WallSections = GetComponentsInChildren<WallSection>().OrderBy(section => section.Id).ToList();
      m_CurrentHeadWallSection = m_WallSections[0];
      m_CurrentTailWallSection = m_WallSections[m_WallSections.Count - 1];
      m_CurrentHeadWallSectionIndex = 0;
    }

    private void Update() {
      MoveWallBackground();
    }

    public void TransitionToNextStage() {
      IsCreatingNewWallSection = true;
      StartCoroutine(ExecuteNextStageProcedure());
    }

    private IEnumerator ExecuteNextStageProcedure() {
      AttachNewWallSection();

      // Get position data here
      temporaryYTargetPosition = m_CurrentTailWallSection.transform.position.y;

      yield return StartCoroutine(ShiftCameraForNextStage());
      yield return StartCoroutine(ManageWallSectionsAfterAddition());
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
        // Always remove [0] because you've popped off the first element in the prior iteration
        WallSection sectionToRemove = m_WallSections[0];
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

    private IEnumerator ManageWallSectionsAfterAddition() {
      // Get the position of the current tail so I know the location to target
      //float oldTailYPosition = m_CurrentTailWallSection.transform.position.y;
      Debug.Log(temporaryYTargetPosition);
      m_CurrentHeadWallSection = m_WallSections[4];
      m_CurrentTailWallSection = m_WallSections[7];

      //float distanceTraveled = 0f;
      float yPositionAtStart = m_CurrentTailWallSection.transform.position.y;
      while (m_CurrentTailWallSection.transform.position.y < temporaryYTargetPosition) {
        // distanceTraveled = m_CurrentTailWallSection.transform.position.y - yPositionAtStart;
        yield return null;
      }
      Debug.Log(m_CurrentTailWallSection.transform.position.y);
      //Debug.Log(distanceTraveled);
      RemoveOldWallSections();

      m_CurrentHeadWallSectionIndex = 0;
      m_CurrentTailWallSectionIndex = m_WallSections.Count - 1;

      //m_CurrentHeadWallSection = m_WallSections[0];
      //m_CurrentHeadWallSectionIndex = 0;
      IsCreatingNewWallSection = false;
    }

    private IEnumerator ShiftCameraForNextStage() {
      float distanceToMove = 11.25f;
      Vector3 currentPosition = m_Camera.transform.position;
      Vector3 targetPosition = new Vector3(currentPosition.x, currentPosition.y - distanceToMove, currentPosition.z);
      float moveSpeed = 2f;
      while (Vector3.Distance(m_Camera.transform.position, targetPosition) > 0.01f) {
        Vector3 position = Vector3.Lerp(m_Camera.transform.position, targetPosition, Time.deltaTime * moveSpeed);
        m_Camera.transform.position = position;
        yield return null;
      }
      m_Camera.transform.position = targetPosition;
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
