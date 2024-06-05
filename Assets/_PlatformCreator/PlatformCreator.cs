using Guidance.Gameplay.Game.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Guidance.Gameplay {
  public class PlatformCreator : MonoBehaviour, IStageTransition {
    public event Action OnPlatformCreated;
    [SerializeField] private GameObject m_PlatformPrefab;
    [SerializeField] private float m_ScaleModifier = 23.5f;


    private GameObject m_CurrentCube;
    private Platform m_CurrentCubePlatform;
    private List<Platform> m_CreatedPlatforms;

    [SerializeField] private float m_EmissionTransitionLengthInSeconds = 1f;
    private Vector3 m_InitialMousePosition;
    private Vector3 m_CurrentMousePosition;
    private bool m_IsEnabled = false;
    public bool IsEnabled {
      get {
        return m_IsEnabled;
      }
      set {
        if (value) {
          Cursor.visible = true;
        } else {
          Cursor.visible = false;
        }
        m_IsEnabled = value;
      }
    }

    private Camera m_MainCamera;
    private float m_CameraOffset;

    private void Awake() {
      m_CreatedPlatforms = new List<Platform>();
    }

    private void Start() {
      m_MainCamera = Camera.main;
      m_CameraOffset = m_MainCamera.transform.position.z;
      IsEnabled = true;
    }

    void Update() {
      if (!m_IsEnabled) {
        return;
      }
      // Temporary
      //if (Input.GetKeyDown(KeyCode.R)) {
      //  SceneManager.LoadSceneAsync(0);
      //}
      if (Input.GetMouseButtonDown(0)) {
        CreatePlatform();
      }

      if (Input.GetMouseButton(0)) {
        UpdatePlatformSize();
      }

      if (Input.GetMouseButtonUp(0)) {
        m_CurrentCubePlatform.PerformTransitionEmission();
        OnPlatformCreated?.Invoke();
        ResetMousePosition();
      }
    }

    public void ShiftForStageTransition() {
      foreach (Platform platform in m_CreatedPlatforms) {
        StartCoroutine(StageTransitionManager.ShiftForNextStage(platform.transform));
      }
    }

    private void CreatePlatform() {
      m_InitialMousePosition = Input.mousePosition;
      m_InitialMousePosition.z = m_CameraOffset;
      Vector3 spawnPosition = m_MainCamera.ScreenToWorldPoint(new Vector3(m_InitialMousePosition.x, m_InitialMousePosition.y, -m_CameraOffset));

      m_CurrentCube = Instantiate(m_PlatformPrefab, spawnPosition, m_PlatformPrefab.transform.rotation, transform);
      m_CurrentCubePlatform = m_CurrentCube.GetComponent<Platform>();
      m_CreatedPlatforms.Add(m_CurrentCubePlatform);
    }

    private void UpdatePlatformSize() {
      if (m_CurrentCube != null) {
        m_CurrentMousePosition = Input.mousePosition;
        m_CurrentMousePosition.z = m_CameraOffset;
        float distance = Vector3.Distance(m_InitialMousePosition, m_CurrentMousePosition);

        Vector3 direction = (m_CurrentMousePosition - m_InitialMousePosition);
        m_CurrentCube.transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
        m_CurrentCube.transform.localScale = new Vector3((distance / m_ScaleModifier), m_CurrentCube.transform.localScale.y, m_CurrentCube.transform.localScale.z);
      }
    }

    private void ResetMousePosition() {
      m_InitialMousePosition = Vector3.zero;
      m_CurrentMousePosition = Vector3.zero;
    }
  }
}
