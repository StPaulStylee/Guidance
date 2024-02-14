using UnityEngine;

namespace Guidance.Gameplay {
  public class PlatformCreator : MonoBehaviour {
    public GameObject cubePrefab;
    public float scaleModifier = 10f;
    private GameObject currentCube;
    private Vector3 initialMousePosition;
    private Vector3 currentMousePosition;
    private Camera m_MainCamera;

    private void Start() {
      m_MainCamera = Camera.main;
    }

    void Update() {
      if (Input.GetMouseButtonDown(0)) {
        CreateCube();
      }

      if (Input.GetMouseButton(0)) {
        UpdateCubeSize();
      }

      if (Input.GetMouseButtonUp(0)) {
        ResetMousePosition();
      }
    }

    void CreateCube() {
      initialMousePosition = Input.mousePosition;
      initialMousePosition.z = -10;
      Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(initialMousePosition.x, initialMousePosition.y, 10f));

      currentCube = Instantiate(cubePrefab, spawnPosition, cubePrefab.transform.rotation, transform);
    }

    void UpdateCubeSize() {
      if (currentCube != null) {
        currentMousePosition = Input.mousePosition;
        currentMousePosition.z = -10;
        Vector3 targetPosition = m_MainCamera.ScreenToWorldPoint(currentMousePosition);
        float distance = Vector3.Distance(initialMousePosition, currentMousePosition);

        Vector3 direction = (currentMousePosition - initialMousePosition);
        currentCube.transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
        //float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        currentCube.transform.localScale = new Vector3((distance / scaleModifier), currentCube.transform.localScale.y, currentCube.transform.localScale.z);
      }
    }

    void ResetMousePosition() {
      Debug.Log(initialMousePosition);
      Debug.Log(currentMousePosition);
      initialMousePosition = Vector3.zero;
      currentMousePosition = Vector3.zero;
    }
  }
}
