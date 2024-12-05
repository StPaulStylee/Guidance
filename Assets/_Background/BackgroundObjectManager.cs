using _Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Background {
  public class BackgroundObjectManager : MonoBehaviour {
    [FormerlySerializedAs("mNotifier")] [FormerlySerializedAs("m_Notifier")] [SerializeField]
    private BackgroundObjectNotifier notifier;

    [FormerlySerializedAs("m_SpaceShipPrefabs")] [SerializeField]
    private GameObject[] spaceShipPrefabs;

    [FormerlySerializedAs("m_SpawnPoints")] [SerializeField]
    private SpawnPoint[] spawnPoints;

    private readonly float _maxSpawnDelay = 1f;
    private GameObject _currentBackgroundObject;

    private void Start() {
      CreateSpaceship();
    }

    private void OnEnable() {
      notifier.OnObjectTriggerExit += DestroyBackgroundObject;
    }

    private void OnDisable() {
      notifier.OnObjectTriggerExit -= DestroyBackgroundObject;
    }

    private void DestroyBackgroundObject(GameObject backgroundObject) {
      if (_currentBackgroundObject != null) {
        Destroy(backgroundObject);
        Invoke("CreateSpaceship", Random.Range(0f, _maxSpawnDelay));
      }
    }

    private GameObject GetRandomSpaceShip() {
      int index = Random.Range(0, spaceShipPrefabs.Length);
      return spaceShipPrefabs[index];
    }

    private void CreateBackgroundObject(GameObject go) {
      SpawnPoint spawnPoint = GetRandomSpawnPoint();
      BackgroundObjectSpawnData spawnData = spawnPoint.GetSpawnData();
      _currentBackgroundObject = Instantiate(go, spawnData.Position, spawnData.Rotation, transform);
    }

    private SpawnPoint GetRandomSpawnPoint() {
      int index = Random.Range(0, spawnPoints.Length);
      return spawnPoints[index];
    }

    private void CreateSpaceship() {
      CreateBackgroundObject(GetRandomSpaceShip());
    }
  }
}