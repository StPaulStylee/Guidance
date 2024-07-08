using Guidance.Background.Object;
using UnityEngine;

namespace Guidance.Background.Manager {
  public class BackgroundObjectManager : MonoBehaviour {
    [SerializeField] private BoxCollider m_BoundingBox;
    [SerializeField] private GameObject[] m_SpaceShipPrefabs;
    [SerializeField] private SpawnPoint[] m_SpawnPoints;
    // Start is called before the first frame update
    void Start() {
      CreateBackgroundObject(GetRandomSpaceShip());
    }

    // Update is called once per frame
    void Update() {

    }

    private GameObject GetRandomSpaceShip() {
      int index = Random.Range(0, m_SpaceShipPrefabs.Length);
      return m_SpaceShipPrefabs[index];
    }

    private void CreateBackgroundObject(GameObject go) {
      SpawnPoint spawnPoint = GetRandomSpawnPoint();
      BackgroundObjectSpawnData spawnData = spawnPoint.GetSpawnData();
      // need position and rotation
      Instantiate(go, spawnData.Position, spawnData.Rotation, transform);
    }

    private SpawnPoint GetRandomSpawnPoint() {
      int index = Random.Range(0, m_SpawnPoints.Length);
      return m_SpawnPoints[index];
    }
  }
}
