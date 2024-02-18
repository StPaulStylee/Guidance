using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Guidance.Gameplay {
  public class PlatformCreator : MonoBehaviour {
    public static Action OnPlatformCreated;
    [SerializeField] private GameObject m_CubePrefab;
    [SerializeField] private float m_ScaleModifier = 23.5f;


    private GameObject m_CurrentCube;
    private Material m_CurrentCubeMaterial;
    private Color m_CurrentEmission;
    //[SerializeField] private Color m_EmissionTarget = new Color(1.292f, 0.155f, 2.996f, 1.000f); // Purple
    [SerializeField] private Color m_EmissionTarget = new Color(5.48566914f, 4.91207218f, 6.63286161f, 1); // White;
    //[SerializeField] private Color m_EmissionTarget = new Color(0.661077499f, 2.18867922f, 0.196155161f, 1); // Green

    [SerializeField] private float m_EmissionTransitionLengthInSeconds = 1f;
    // RGBA(0.005, 0.001, 0.012, 1.000) - Start
    // RGBA(1.292, 0.155, 2.996, 1.000) - Target
    private Vector3 m_InitialMousePosition;
    private Vector3 m_CurrentMousePosition;

    private Camera m_MainCamera;
    private float m_CameraOffset;
    private void Start() {
      m_MainCamera = Camera.main;
      m_CameraOffset = m_MainCamera.transform.position.z;
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

      m_CurrentCube = Instantiate(m_CubePrefab, spawnPosition, m_CubePrefab.transform.rotation, transform);
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
      OnPlatformCreated?.Invoke();
      m_InitialMousePosition = Vector3.zero;
      m_CurrentMousePosition = Vector3.zero;
    }
  }
}
