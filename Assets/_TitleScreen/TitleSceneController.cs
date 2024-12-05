using Guidance.Gameplay;
using Guidance.Gameplay.Game.Manager;
using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace Guidance.Title {
  public class TitleSceneController : MonoBehaviour {
    public event Action OnTitleSceneEnd;

    public CinemachineCamera Camera;
    public float BallMoveTime = 5f;
    private Ball m_Ball;
    private Material m_BallMaterial;
    private BallStopPoint m_BallStopPoint;
    private bool m_IsBallMoving = false;
    [SerializeField] float m_ActivateDelay = 3f;
    private TitleController m_TitleController;

    private void Start() {
      m_TitleController = FindObjectOfType<TitleController>();
      m_Ball = FindObjectOfType<Ball>();
      m_BallStopPoint = FindObjectOfType<BallStopPoint>();
      if (m_Ball == null) {
        Debug.LogWarning("No Ball found in Title scene");
      }
      if (m_BallStopPoint == null) {
        Debug.LogWarning("No Stop Point found in TitleSceneController");
      }
      m_BallMaterial = m_Ball.BallMaterial;
    }

    private void Update() {
      if (Input.GetKeyDown(KeyCode.Escape)) {
        StartCoroutine(DissolveBallDown());
      }
    }

    private IEnumerator MoveBallToStart() {
      float elapsedTime = 0f;
      Vector3 initialPosition = m_Ball.transform.position;
      while (elapsedTime < BallMoveTime) {
        m_Ball.transform.position = Vector3.Lerp(initialPosition, m_BallStopPoint.transform.position, elapsedTime / BallMoveTime);
        elapsedTime += Time.deltaTime;
        yield return null;
      }
      m_Ball.transform.position = m_BallStopPoint.transform.position;
      StartCoroutine(ActivateGame());
    }

    private IEnumerator ActivateGame() {
      yield return new WaitForSeconds(m_ActivateDelay);
      OnTitleSceneEnd?.Invoke();
    }

    public IEnumerator Test() {
      yield return StartCoroutine(BallDissolveManager.PerformVerticalDissolveDown(m_BallMaterial));
      StartCoroutine(MoveBallToStart());
      yield return StartCoroutine(BallDissolveManager.PerformVerticalDissolveUp(m_BallMaterial));
    }

    public IEnumerator DissolveBallDown() {
      StopCoroutine(m_TitleController.PulseEmission());
      yield return StartCoroutine(m_TitleController.FadeOut());
      yield return StartCoroutine(BallDissolveManager.PerformVerticalDissolveDown(m_BallMaterial));
      Camera.enabled = false;
    }

    // Cinemachine Camera Events
    public void MoveBall() {
      m_Ball.transform.position = m_BallStopPoint.transform.position;
      StartCoroutine(ActivateGame());
    }

    public void DissolveBallUp() {
      StartCoroutine(BallDissolveManager.PerformVerticalDissolveUp(m_BallMaterial));
    }
  }
}
