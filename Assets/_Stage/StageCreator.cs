# if UNITY_EDITOR
using Guidance.Data;
using Guidance.Gameplay.Obstacles;
using Guidance.Gameplay.Stage;
using Guidance.Stage.Data;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Guidance.Stage {
  public class StageCreator : OdinEditorWindow {
    private Transform m_RootTransform;
    [SerializeField] private StageData[] m_StageData;
    private int m_StageNumber;
    private Position m_TargetLocation;
    private ObstacleData[] m_ObstacleData;
    private Dictionary<string, GameObject> m_ObstaclePrefabs;

    [MenuItem("Tools/Stage Editor")]
    private static void OpenWindow() {
      GetWindow<StageCreator>().Show();
    }


    protected override void OnEnable() {
      base.OnEnable();
      m_RootTransform = GameObject.Find("Game").transform;
      m_ObstacleData = new ObstacleData[] { };
      m_StageData = Utilities.GetStageData();
      LoadObstaclePrefabs();
    }

    private void LoadObstaclePrefabs() {
      m_ObstaclePrefabs = new Dictionary<string, GameObject>();
      foreach (ObstacleAssetData dataAsset in StageCreatorData.ObstacleAssetData) {
        string path = Path.Combine(StageCreatorData.PrefabPath, dataAsset.Subdirectory);
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { path });
        string assetPath = AssetDatabase.GUIDToAssetPath(prefabGuids[0]);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        m_ObstaclePrefabs.Add(dataAsset.Name, prefab);
      }
    }

    private void SaveStageData() {
      string path = Application.dataPath + "/_Data/StageData.json";
      for (int i = 0; i < m_StageData.Length; i++) {
        m_StageData[i].StageNumber = i;
      }
      string json = JsonConvert.SerializeObject(m_StageData, new JsonSerializerSettings {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Formatting = Formatting.Indented
      });
      File.WriteAllText(path, json);
    }


    private void CaptureStage() {
      ArrayUtility.Clear(ref m_ObstacleData);
      m_StageNumber = m_StageData.Length;
      m_TargetLocation = GetTargetPosition(m_RootTransform.GetComponentInChildren<Target>().transform.position);
      Obstacle[] obstacles = m_RootTransform.GetComponentsInChildren<Obstacle>();
      foreach (Obstacle obstacle in obstacles) {
        ObstacleData data = new ObstacleData {
          Position = new Position { X = obstacle.transform.position.x, Y = obstacle.transform.position.y, Z = 0f },
          Rotation = obstacle.transform.eulerAngles.z,
          Scale = obstacle.transform.localScale.x,
          TypeId = obstacle.TypeId,
          LinkId = obstacle.LinkId,
        };
        ArrayUtility.Add(ref m_ObstacleData, data);
        Debug.Log($"TargetLocation: {m_TargetLocation}, Position: {data.Position}, Rotation: {data.Rotation}, Scale: {data.Scale}, TypeId: {data.TypeId}");
      }
    }

    [ButtonGroup("Add Obstacle")]
    [Button("Add White Obstacle")]
    private void AddWhiteObstacle() {
      GameObject prefab = m_ObstaclePrefabs[ObstacleAssetDataKey.White];
      Instantiate(prefab, Vector3.zero, Quaternion.identity, m_RootTransform);
    }

    [ButtonGroup("Add Obstacle")]
    [Button("Add Green Obstacle")]
    private void AddGreenObstacle() {
      GameObject prefab = m_ObstaclePrefabs[ObstacleAssetDataKey.Green];
      Instantiate(prefab, Vector3.zero, Quaternion.identity, m_RootTransform);
    }

    [ButtonGroup("Add Obstacle")]
    [Button("Add Red Obstacle")]
    private void AddRedObstacle() {
      GameObject prefab = m_ObstaclePrefabs[ObstacleAssetDataKey.Red];
      Instantiate(prefab, Vector3.zero, Quaternion.identity, m_RootTransform);
    }


    [Button("Add/Update Stage Data")]
    private void AddStage() {
      CaptureStage();
      if (StageDebugger.EditingData.IsEditing) {
        int index = StageDebugger.EditingData.StageBeingEdited;
        m_StageData[index].Obstacles = m_ObstacleData;
        m_StageData[index].TargetLocation = m_TargetLocation;
      } else {
        ArrayUtility.Add(ref m_StageData, new StageData {
          Obstacles = m_ObstacleData,
          StageNumber = m_StageNumber,
          TargetLocation = m_TargetLocation,
        });
      }
      SaveStageData();
    }

    [Button("Reload Stage Data Array")]
    private void ReloadStageData() {
      m_StageData = Utilities.GetStageData();
    }

    [Button("Save Stage Data Array")]
    private void SaveStageDataArray() {
      SaveStageData();
    }

    [Button("Reset Stage Debugger")]
    private void ResetStageDebugger() {
      StageDebugger.ResetStageDebug();
    }


    private Position GetTargetPosition(Vector3 position) {
      return new Position { X = position.x, Y = Constants.TARGET_LOCATION_Y_SPAWN_LOCATION, Z = 0f };
    }
  }
}
#endif