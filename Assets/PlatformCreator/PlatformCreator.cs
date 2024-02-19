using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Guidance.Gameplay {
  public class PlatformCreator : MonoBehaviour {
    public static Action OnPlatformCreated;
    [SerializeField] private GameObject m_PlatformPrefab;
    [SerializeField] private float m_ScaleModifier = 23.5f;


    private GameObject m_CurrentCube;
    private Material m_CurrentCubeMaterial;
    private Color m_CurrentEmission;
    private Color m_EmissionTarget;

    [SerializeField] private float m_EmissionTransitionLengthInSeconds = 1f;
    private Vector3 m_InitialMousePosition;
    private Vector3 m_CurrentMousePosition;

    private Camera m_MainCamera;
    private float m_CameraOffset;

    private void Start() {
      m_MainCamera = Camera.main;
      m_CameraOffset = m_MainCamera.transform.position.z;
      m_EmissionTarget = m_PlatformPrefab.GetComponent<Platform>().GetTargetColor();
    }

    void Update() {
      if (Input.GetMouseButtonDown(0)) {
        CreateCube();
      }

      if (Input.GetMouseButton(0)) {
        UpdateCubeSize();
      }

      if (Input.GetMouseButtonUp(0)) {
        StartCoroutine(TransitionEmission());
        OnPlatformCreated?.Invoke();
        ResetMousePosition();
      }
      if (Input.GetKeyDown(KeyCode.R)) {
        SceneManager.LoadSceneAsync(0);
      }
    }

    void CreateCube() {
      m_InitialMousePosition = Input.mousePosition;
      m_InitialMousePosition.z = m_CameraOffset;
      Vector3 spawnPosition = m_MainCamera.ScreenToWorldPoint(new Vector3(m_InitialMousePosition.x, m_InitialMousePosition.y, -m_CameraOffset));

      m_CurrentCube = Instantiate(m_PlatformPrefab, spawnPosition, m_PlatformPrefab.transform.rotation, transform);
      m_CurrentCubeMaterial = m_CurrentCube.GetComponent<MeshRenderer>().material;
    }

    void UpdateCubeSize() {
      if (m_CurrentCube != null) {
        m_CurrentMousePosition = Input.mousePosition;
        m_CurrentMousePosition.z = m_CameraOffset;
        float distance = Vector3.Distance(m_InitialMousePosition, m_CurrentMousePosition);

        Vector3 direction = (m_CurrentMousePosition - m_InitialMousePosition);
        m_CurrentCube.transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
        m_CurrentCube.transform.localScale = new Vector3((distance / m_ScaleModifier), m_CurrentCube.transform.localScale.y, m_CurrentCube.transform.localScale.z);
      }
    }

    private IEnumerator TransitionEmission() {
      m_CurrentEmission = m_CurrentCubeMaterial.GetColor("_EmissionColor");
      float timeElapsed = 0f;
      while (timeElapsed < m_EmissionTransitionLengthInSeconds) {
        Color currentEmissionColor = Color.Lerp(m_CurrentEmission, m_EmissionTarget, timeElapsed / m_EmissionTransitionLengthInSeconds);
        m_CurrentCubeMaterial.SetColor("_EmissionColor", currentEmissionColor);
        timeElapsed += Time.deltaTime;
        yield return null;
      }
    }

    void ResetMousePosition() {
      m_InitialMousePosition = Vector3.zero;
      m_CurrentMousePosition = Vector3.zero;
    }
  }
}
