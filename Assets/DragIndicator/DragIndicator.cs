using UnityEngine;

namespace Guidance.DragIndicator {
  [RequireComponent(typeof(LineRenderer))]
  public class DragIndicator : MonoBehaviour {
    [SerializeField] private GameObject m_CubePrefab;
    private GameObject m_CurrentCube;
    private Camera m_Camera;
    private float m_CameraOffsetZ;

    private Vector3 m_StartPosition;
    private Vector3 m_EndPosition;

    private LineRenderer m_LineRenderer;
    private float m_LineAngle;

    private void Awake() {
      m_LineRenderer = GetComponent<LineRenderer>();
    }

    private void Start() {
      m_Camera = Camera.main;
      m_CameraOffsetZ = -m_Camera.transform.position.z;
    }

    private void Update() {
      //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));

      if (Input.GetMouseButtonDown(0)) {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = m_CameraOffsetZ;
        m_StartPosition = m_Camera.ScreenToWorldPoint(mousePos);
        m_CurrentCube = Instantiate(m_CubePrefab, m_StartPosition, m_CubePrefab.transform.rotation);
        //ConfigureLineRenderer();
        //m_LineRenderer.SetPosition(0, m_StartPosition);
        //Debug.Log(Input.mousePosition);
        //Debug.Log(m_StartPosition);
      }
      if (Input.GetMouseButton(0)) {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = m_CameraOffsetZ;
        m_EndPosition = m_Camera.ScreenToWorldPoint(mousePos);
        var rotation = Vector3.Angle(m_StartPosition, m_EndPosition);
        var quaternion = Quaternion.Euler(0f, 0f, rotation) * m_CurrentCube.transform.rotation;
        m_CurrentCube.transform.rotation = quaternion;
        //m_LineRenderer.SetPosition(1, m_EndPosition);
      }
      if (Input.GetMouseButtonUp(0)) {
        //Vector3 lineDirection = m_EndPosition - m_StartPosition;
        //m_LineAngle = Vector3.Angle(m_StartPosition, m_EndPosition);
        //Quaternion quaternion = Quaternion.Euler(0, 0, m_LineAngle) * m_CubePrefab.transform.rotation;
        ////Instantiate(m_CubePrefab, m_StartPosition, m_CubePrefab.transform.rotation);
        //Instantiate(m_CubePrefab, m_StartPosition, quaternion);


        //m_LineRenderer.enabled = false;
      }
    }

    private void ConfigureLineRenderer() {
      m_LineRenderer.enabled = true;
      m_LineRenderer.useWorldSpace = true;
      m_LineRenderer.positionCount = 2;
    }

  }
}
