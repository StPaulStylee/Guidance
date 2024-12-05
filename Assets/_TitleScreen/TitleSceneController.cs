using System;
using System.Collections;
using _Ball;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace _TitleScreen {
  public class TitleSceneController : MonoBehaviour {
    public CinemachineCamera Camera;
    public float BallMoveTime = 5f;

    [FormerlySerializedAs("m_ActivateDelay")] [SerializeField]
    private float activateDelay = 3f;

    private Ball _ball;
    private Material _ballMaterial;

    private BallStopPoint _ballStopPoint;

    // private bool _isBallMoving;
    private TitleController _titleController;

    private void Start() {
      // _isBallMoving = false;
      _titleController = FindObjectOfType<TitleController>();
      _ball = FindObjectOfType<Ball>();
      _ballStopPoint = FindObjectOfType<BallStopPoint>();
      if (_ball == null) {
        Debug.LogWarning("No Ball found in Title scene");
      }

      if (_ballStopPoint == null) {
        Debug.LogWarning("No Stop Point found in TitleSceneController");
      }

      _ballMaterial = _ball.BallMaterial;
    }

    private void Update() {
      if (Input.GetKeyDown(KeyCode.Escape)) {
        StartCoroutine(DissolveBallDown());
      }
    }

    public event Action OnTitleSceneEnd;

    private IEnumerator MoveBallToStart() {
      float elapsedTime = 0f;
      Vector3 initialPosition = _ball.transform.position;
      while (elapsedTime < BallMoveTime) {
        _ball.transform.position =
          Vector3.Lerp(initialPosition, _ballStopPoint.transform.position, elapsedTime / BallMoveTime);
        elapsedTime += Time.deltaTime;
        yield return null;
      }

      _ball.transform.position = _ballStopPoint.transform.position;
      StartCoroutine(ActivateGame());
    }

    private IEnumerator ActivateGame() {
      yield return new WaitForSeconds(activateDelay);
      OnTitleSceneEnd?.Invoke();
    }

    public IEnumerator Test() {
      yield return StartCoroutine(BallDissolveManager.PerformVerticalDissolveDown(_ballMaterial));
      StartCoroutine(MoveBallToStart());
      yield return StartCoroutine(BallDissolveManager.PerformVerticalDissolveUp(_ballMaterial));
    }

    public IEnumerator DissolveBallDown() {
      StopCoroutine(_titleController.PulseEmission());
      yield return StartCoroutine(_titleController.FadeOut());
      yield return StartCoroutine(BallDissolveManager.PerformVerticalDissolveDown(_ballMaterial));
      Camera.enabled = false;
    }

    // Cinemachine Camera Events
    public void MoveBall() {
      _ball.transform.position = _ballStopPoint.transform.position;
      StartCoroutine(ActivateGame());
    }

    public void DissolveBallUp() {
      StartCoroutine(BallDissolveManager.PerformVerticalDissolveUp(_ballMaterial));
    }
  }
}
