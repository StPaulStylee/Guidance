using Guidance.Background.Object;
using UnityEngine;

namespace Guidance.Background.Manager {
  public class BackgroundObjectManager : MonoBehaviour {
    [SerializeField] private BackgroundObjectNotifier m_Notifier;
    [SerializeField] private GameObject[] m_SpaceShipPrefabs;
    [SerializeField] private SpawnPoint[] m_SpawnPoints;
    private float m_MaxSpawnDelay = 1f;
    private GameObject m_CurrentBackgroundObject;

    private void OnEnable() {
      m_Notifier.OnObjectTriggerExit += DestroyBackgroundObject;
    }

    private void OnDisable() {
      m_Notifier.OnObjectTriggerExit -= DestroyBackgroundObject;
    }

    void Start() {
      CreateSpaceship();
    }

    private void DestroyBackgroundObject(GameObject backgroundObject) {
      if (m_CurrentBackgroundObject != null) {
        Destroy(backgroundObject);
        Invoke("CreateSpaceship", Random.Range(0f, m_MaxSpawnDelay));
      }
    }

    private GameObject GetRandomSpaceShip() {
      int index = Random.Range(0, m_SpaceShipPrefabs.Length);
      return m_SpaceShipPrefabs[index];
    }

    private void CreateBackgroundObject(GameObject go) {
      SpawnPoint spawnPoint = GetRandomSpawnPoint();
      BackgroundObjectSpawnData spawnData = spawnPoint.GetSpawnData();
      m_CurrentBackgroundObject = Instantiate(go, spawnData.Position, spawnData.Rotation, transform);
    }

    private SpawnPoint GetRandomSpawnPoint() {
      int index = Random.Range(0, m_SpawnPoints.Length);
      return m_SpawnPoints[index];
    }

    private void CreateSpaceship() {
      CreateBackgroundObject(GetRandomSpaceShip());
    }
  }
}
