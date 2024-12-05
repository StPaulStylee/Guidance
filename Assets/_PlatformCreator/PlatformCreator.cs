using System;
using System.Collections;
using System.Collections.Generic;
using _Game;
using _Platform;
using UnityEngine;
using UnityEngine.Serialization;

namespace _PlatformCreator {
  public class PlatformCreator : MonoBehaviour, IStageTransition {
    private const float SCALE_MODIFIER = 0.03f;

    [FormerlySerializedAs("m_PlatformPrefab")] [SerializeField]
    private GameObject platformPrefab;

    [FormerlySerializedAs("m_EmissionTransitionLengthInSeconds")] [SerializeField]
    private float emissionTransitionLengthInSeconds = 1f;

    private float _cameraOffset;
    private List<Platform> _createdPlatforms;

    private GameObject _currentCube;
    private Platform _currentCubePlatform;
    private Vector3 _currentMousePosition;
    private Vector3 _initialMousePosition;
    private bool _isEnabled;

    private Camera _mainCamera;

    public bool IsEnabled {
      get => _isEnabled;
      set {
        if (value) {
          Cursor.visible = true;
        }
        else {
          Cursor.visible = false;
        }

        _isEnabled = value;
      }
    }

    private void Awake() {
      _createdPlatforms = new List<Platform>();
    }

    private void Start() {
      _mainCamera = Camera.main;
      if (_mainCamera != null) {
        _cameraOffset = _mainCamera.transform.position.z;
      }

      IsEnabled = true;
    }

    private void Update() {
      if (!_isEnabled) {
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
        _currentCubePlatform.PerformTransitionEmission();
        OnPlatformCreated?.Invoke();
        ResetMousePosition();
      }
    }

    public void ShiftForStageTransition() {
      foreach (Platform platform in _createdPlatforms)
        StartCoroutine(StageTransitionManager.ShiftForNextStage(platform.transform));
    }

    public event Action OnPlatformCreated;

    public IEnumerator ShiftForStageRestart() {
      foreach (Platform platform in _createdPlatforms) {
        StartCoroutine(StageTransitionManager.ShiftForNextStage(platform.transform));
        yield return null;
      }
    }

    private void CreatePlatform() {
      _initialMousePosition = Input.mousePosition;
      _initialMousePosition.z = _cameraOffset;
      Vector3 spawnPosition =
        _mainCamera.ScreenToWorldPoint(
          new Vector3(_initialMousePosition.x, _initialMousePosition.y, -_cameraOffset));

      _currentCube = Instantiate(platformPrefab, spawnPosition, platformPrefab.transform.rotation, transform);
      _currentCubePlatform = _currentCube.GetComponent<Platform>();
      _createdPlatforms.Add(_currentCubePlatform);
    }

    private void UpdatePlatformSize() {
      if (_currentCube) {
        _currentMousePosition = Input.mousePosition;
        _currentMousePosition.z = _cameraOffset;
        float distance = Vector3.Distance(_initialMousePosition, _currentMousePosition);
        float normalizedDistance = distance / Screen.width;

        Vector3 direction = _currentMousePosition - _initialMousePosition;
        _currentCube.transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
        _currentCube.transform.localScale = new Vector3(normalizedDistance / SCALE_MODIFIER,
          _currentCube.transform.localScale.y, _currentCube.transform.localScale.z);
      }
    }

    private void ResetMousePosition() {
      _initialMousePosition = Vector3.zero;
      _currentMousePosition = Vector3.zero;
    }
  }
}
