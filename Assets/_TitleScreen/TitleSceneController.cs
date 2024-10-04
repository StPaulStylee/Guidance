using Guidance.Gameplay;
using Guidance.Gameplay.Game.Manager;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace Guidance.Title {
  public class TitleSceneController : MonoBehaviour {
    public CinemachineCamera Camera;
    public float BallMoveTime = 5f;
    private Ball m_Ball;
    private Material m_BallMaterial;
    public float VerticalDissolveTime = 2.0f;
    private BallStopPoint m_BallStopPoint;
    private bool m_IsBallMoving = false;

    private void OnEnable() {
      CinemachineCore.CameraActivatedEvent.AddListener(OnCameraActivated);
      CinemachineCore.CameraDeactivatedEvent.AddListener(OnCameraDeactivated);
    }

    private void OnDisable() {
      CinemachineCore.CameraActivatedEvent.RemoveListener(OnCameraActivated);
      CinemachineCore.CameraDeactivatedEvent.RemoveListener(OnCameraDeactivated);
    }

    private void Start() {
      m_Ball = FindObjectOfType<Ball>();
      m_BallStopPoint = FindObjectOfType<BallStopPoint>();
      if (m_Ball == null) {
        Debug.LogWarning("No Ball found in Title scene");
      }
      if (m_BallStopPoint == null) {
        Debug.LogWarning("No Stop POint found in TitleSceneController");
      }
      m_BallMaterial = m_Ball.GetComponent<Renderer>().material;
      //StartCoroutine(BallDissolveManager.PerformVerticalDissolveDown(m_BallMaterial, VerticalDissolveTime));
      //StartCoroutine(BallDissolveManager.PerformVerticalDissolveUp(m_BallMaterial, VerticalDissolveTime));

    }

    private void Update() {
      if (Input.GetKeyDown(KeyCode.Escape)) {
        StartCoroutine(DissolveBallDown());
      }
    }


    private void OnCameraDeactivated(ICinemachineMixer arg0, ICinemachineCamera arg1) {
      //StartCoroutine(BallDissolveManager.PerformVerticalDissolveDown(m_BallMaterial, VerticalDissolveTime));
      //StartCoroutine(MoveBallToStart());
      //StartCoroutine(BallDissolveManager.PerformVerticalDissolveUp(m_BallMaterial, VerticalDissolveTime));
    }

    private void OnCameraActivated(ICinemachineCamera.ActivationEventParams evt) {
      //Debug.Log(evt.IncomingCamera);
      //if (evt.IncomingCamera.Vir)
      //  StartCoroutine(Test());
    }

    private IEnumerator MoveBallToStart() {
      //MeshRenderer meshRenderer = m_Ball.GetComponent<MeshRenderer>();
      //meshRenderer.enabled = false;
      //yield return StartCoroutine(BallDissolveManager.PerformVerticalDissolveDown(m_BallMaterial, VerticalDissolveTime));
      float elapsedTime = 0f;
      Vector3 initialPosition = m_Ball.transform.position;
      while (elapsedTime < BallMoveTime) {
        m_Ball.transform.position = Vector3.Lerp(initialPosition, m_BallStopPoint.transform.position, elapsedTime / BallMoveTime);
        elapsedTime += Time.deltaTime;
        yield return null;
      }
      m_Ball.transform.position = m_BallStopPoint.transform.position;
      //meshRenderer.enabled = true;
    }

    private IEnumerator Test() {
      yield return StartCoroutine(BallDissolveManager.PerformVerticalDissolveDown(m_BallMaterial, VerticalDissolveTime));
      StartCoroutine(MoveBallToStart());
      yield return StartCoroutine(BallDissolveManager.PerformVerticalDissolveUp(m_BallMaterial, VerticalDissolveTime));
    }

    public IEnumerator DissolveBallDown() {
      yield return StartCoroutine(BallDissolveManager.PerformVerticalDissolveDown(m_BallMaterial, VerticalDissolveTime));
      Camera.enabled = false;
    }

    public void MoveBall() {
      m_Ball.transform.position = m_BallStopPoint.transform.position;
    }

    public void DissolveBallUp() {
      StartCoroutine(BallDissolveManager.PerformVerticalDissolveUp(m_BallMaterial, VerticalDissolveTime));
    }

    //private void OnCameraDeactivated(ICinemachineCamera.ActivationEventParams evt) {
    //  if (evt.IncomingCamera === Camera) {
    //    Debug.Log("We've got one!");
    //  }
    //}
  }
}