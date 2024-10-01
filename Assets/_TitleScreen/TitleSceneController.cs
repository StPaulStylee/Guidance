using Guidance.Gameplay;
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
      StartCoroutine(PerformVerticalDissolveDown());
    }


    private void OnCameraDeactivated(ICinemachineMixer arg0, ICinemachineCamera arg1) {
      StartCoroutine(MoveBallToStart());
    }

    private void OnCameraActivated(ICinemachineCamera.ActivationEventParams evt) {
      //Debug.Log(evt.IncomingCamera);
    }

    private IEnumerator PerformVerticalDissolveDown() {
      float elapsedTime = 0f;
      m_BallMaterial.SetFloat("_DisAmount", -2.0f);
      while (m_BallMaterial.GetFloat("_DisAmount") < 2.0f) {
        m_BallMaterial.SetFloat("_DisAmount", Mathf.Lerp(-2.0f, 2.0f, elapsedTime / VerticalDissolveTime));
        //float amountToAdd = Mathf.Lerp(-2.0f, 2.0f, elapsedTime / VerticalDissolveTime);
        elapsedTime += Time.deltaTime;
        yield return null;
      }
      m_BallMaterial.SetFloat("_DisAmount", 2.0f);
      //m_BallMaterial.SetFloat("_DisAmount", 2.0f);
      //while (m_BallMaterial.GetFloat("_DisAmount") > -2.0f) {
      //  //float amountToAdd = Mathf.Lerp(2.0f, -2.0f, elapsedTime / VerticalDissolveTime);
      //  m_BallMaterial.SetFloat("_DisAmount", Mathf.Lerp(2.0f, -2.0f, elapsedTime / VerticalDissolveTime));
      //  elapsedTime += Time.deltaTime;
      //  yield return null;
      //}
    }

    private IEnumerator PerformVerticalDissolveUp() {
      float elapsedTime = 0f;
      m_BallMaterial.SetFloat("_DisAmount", 2.0f);
      while (m_BallMaterial.GetFloat("_DisAmount") > -2.0f) {
        //float amountToAdd = Mathf.Lerp(2.0f, -2.0f, elapsedTime / VerticalDissolveTime);
        m_BallMaterial.SetFloat("_DisAmount", Mathf.Lerp(2.0f, -2.0f, elapsedTime / VerticalDissolveTime));
        elapsedTime += Time.deltaTime;
        yield return null;
      }
      m_BallMaterial.SetFloat("_DisAmount", -2.0f);
    }

    private IEnumerator MoveBallToStart() {
      MeshRenderer meshRenderer = m_Ball.GetComponent<MeshRenderer>();
      meshRenderer.enabled = false;
      float elapsedTime = 0f;
      Vector3 initialPosition = m_Ball.transform.position;
      while (elapsedTime < BallMoveTime) {
        m_Ball.transform.position = Vector3.Lerp(initialPosition, m_BallStopPoint.transform.position, elapsedTime / BallMoveTime);
        elapsedTime += Time.deltaTime;
        yield return null;
      }
      m_Ball.transform.position = m_BallStopPoint.transform.position;
      meshRenderer.enabled = true;
    }

    //private void OnCameraDeactivated(ICinemachineCamera.ActivationEventParams evt) {
    //  if (evt.IncomingCamera === Camera) {
    //    Debug.Log("We've got one!");
    //  }
    //}
  }
}