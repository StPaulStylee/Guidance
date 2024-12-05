# if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using _Ball.Obstacles;
using _Data;
using _Obstacle;
using _Target;
using Guidance.Stage;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Stage {
  public class StageCreator : OdinEditorWindow {
    [FormerlySerializedAs("m_StageData")] [SerializeField]
    private StageData[] stageData;

    private ObstacleData[] _obstacleData;
    private Dictionary<string, GameObject> _obstaclePrefabs;
    private Transform _rootTransform;
    private int _stageNumber;
    private Position _targetLocation;

    protected override void OnEnable() {
      base.OnEnable();
      _rootTransform = GameObject.Find("Game").transform;
      _obstacleData = new ObstacleData[] { };
      stageData = Utilities.GetStageData();
      LoadObstaclePrefabs();
    }

    [OnInspectorGUI]
    private void Space1() {
      GUILayout.Space(20);
    }

    [MenuItem("Tools/Stage Editor")]
    private static void OpenWindow() {
      GetWindow<StageCreator>().Show();
    }

    private void LoadObstaclePrefabs() {
      _obstaclePrefabs = new Dictionary<string, GameObject>();
      foreach (ObstacleAssetData dataAsset in StageCreatorData.ObstacleAssetData) {
        string path = Path.Combine(StageCreatorData.PREFAB_PATH, dataAsset.Subdirectory);
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { path });
        string assetPath = AssetDatabase.GUIDToAssetPath(prefabGuids[0]);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        _obstaclePrefabs.Add(dataAsset.Name, prefab);
      }
    }

    private void SaveStageData() {
      string path = Application.dataPath + $"/_Data/{Utilities.StageDataFileName}.json";
      for (int i = 0; i < stageData.Length; i++) stageData[i].StageNumber = i;
      string json = JsonConvert.SerializeObject(stageData, new JsonSerializerSettings {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Formatting = Formatting.Indented
      });
      Debug.Log(path);
      File.WriteAllText(path, json);
    }

    private void CaptureStage() {
      ArrayUtility.Clear(ref _obstacleData);
      _stageNumber = stageData.Length;
      _targetLocation = GetTargetPosition(_rootTransform.GetComponentInChildren<Target>().transform.position);
      Obstacle[] obstacles = _rootTransform.GetComponentsInChildren<Obstacle>();
      foreach (Obstacle obstacle in obstacles) {
        ObstacleData data = new() {
          Position = new Position { X = obstacle.transform.position.x, Y = obstacle.transform.position.y, Z = 0f },
          Rotation = obstacle.transform.eulerAngles.z,
          Scale = obstacle.transform.localScale.x,
          TypeId = obstacle.TypeId,
          LinkId = obstacle.LinkId,
          RotationDirection = GetRotationDirection(obstacle)
        };
        ArrayUtility.Add(ref _obstacleData, data);
        Debug.Log(
          $"TargetLocation: {_targetLocation}, Position: {data.Position}, Rotation: {data.Rotation}, Scale: {data.Scale}, TypeId: {data.TypeId}");
      }
    }

    private SpinDirection GetRotationDirection(Obstacle obstacle) {
      if (obstacle is YellowObstacle yellowObstacle) {
        return yellowObstacle.SpinDirection;
      }

      return SpinDirection.None;
    }

    [ResponsiveButtonGroup("Add Obstacle")]
    [Button("Add White Obstacle")]
    private void AddWhiteObstacle() {
      GameObject prefab = _obstaclePrefabs[ObstacleAssetDataKey.WHITE];
      Instantiate(prefab, Vector3.zero, Quaternion.identity, _rootTransform);
    }

    [ResponsiveButtonGroup("Add Obstacle")]
    [Button("Add Green Obstacle")]
    private void AddGreenObstacle() {
      GameObject prefab = _obstaclePrefabs[ObstacleAssetDataKey.GREEN];
      Instantiate(prefab, Vector3.zero, Quaternion.identity, _rootTransform);
    }

    [ResponsiveButtonGroup("Add Obstacle")]
    [Button("Add Red Obstacle")]
    private void AddRedObstacle() {
      GameObject prefab = _obstaclePrefabs[ObstacleAssetDataKey.RED];
      Instantiate(prefab, Vector3.zero, Quaternion.identity, _rootTransform);
    }

    [ResponsiveButtonGroup("Add Obstacle")]
    [Button("Add Blue Obstacle")]
    private void AddBlueObstacle() {
      GameObject prefab = _obstaclePrefabs[ObstacleAssetDataKey.BLUE];
      Instantiate(prefab, Vector3.zero, Quaternion.identity, _rootTransform);
    }

    [ResponsiveButtonGroup("Add Obstacle")]
    [Button("Add Yellow Obstacle")]
    private void AddYellowObstacle() {
      GameObject prefab = _obstaclePrefabs[ObstacleAssetDataKey.YELLOW];
      Instantiate(prefab, Vector3.zero, Quaternion.identity, _rootTransform);
    }

    [ResponsiveButtonGroup("Add Obstacle")]
    [Button("Add Gold Obstacle")]
    private void AddGoldObstacle() {
      GameObject prefab = _obstaclePrefabs[ObstacleAssetDataKey.GOLD];
      Instantiate(prefab, Vector3.zero, Quaternion.identity, _rootTransform);
    }

    [OnInspectorGUI]
    private void Space2() {
      GUILayout.Space(20);
    }

    [Button("Add/Update Stage Data")]
    private void AddStage() {
      CaptureStage();
      if (StageDebugger.EditingData.IsEditing) {
        int index = StageDebugger.EditingData.StageBeingEdited;
        stageData[index].Obstacles = _obstacleData;
        stageData[index].TargetLocation = _targetLocation;
      }
      else {
        ArrayUtility.Add(ref stageData, new StageData {
          Obstacles = _obstacleData,
          StageNumber = _stageNumber,
          TargetLocation = _targetLocation
        });
      }

      SaveStageData();
    }

    [Button("Reload Stage Data Array")]
    private void ReloadStageData() {
      stageData = Utilities.GetStageData();
    }

    [Button("Save Stage Data Array")]
    private void SaveStageDataArray() {
      SaveStageData();
    }

    [Button("Reset Stage Debugger")]
    private void ResetStageDebugger() {
      StageDebugger.ResetStageDebug();
    }

    private Position GetTargetPosition(Vector3 pos) {
      return new Position { X = pos.x, Y = pos.y, Z = 0f };
    }
  }
}
#endif
