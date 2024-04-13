using Guidance.Data;
using Guidance.Gameplay.Obstacles;
using Guidance.Gameplay.Stage;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
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

    [MenuItem("Tools/Stage Editor")]
    private static void OpenWindow() {
      GetWindow<StageCreator>().Show();
    }

    protected override void OnEnable() {
      base.OnEnable();
      m_RootTransform = GameObject.Find("Game").transform;
      m_ObstacleData = new ObstacleData[] { };
      m_StageData = Utilities.GetStageData();
    }

    [Button("Add Stage Data")]
    private void AddStage() {
      ArrayUtility.Add(ref m_StageData, new StageData {
        Obstacles = m_ObstacleData,
        StageNumber = m_StageNumber,
        TargetLocation = m_TargetLocation,
      });
      CaptureStage();
      SaveStageData();
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
          TypeId = obstacle.TypeId
        };
        ArrayUtility.Add(ref m_ObstacleData, data);
        Debug.Log($"TargetLocation: {m_TargetLocation}, Position: {data.Position}, Rotation: {data.Rotation}, Scale: {data.Scale}, TypeId: {data.TypeId}");
      }
    }

    [Button("Reload Stage Data")]
    private void ReloadStageData() {
      m_StageData = Utilities.GetStageData();
    }

    [Button("Reset Stage Debugger")]
    private void ResetStageDebugger() {
      StageDebugger.ResetStageDebug();
    }

    private void SaveStageData() {
      string path = Application.dataPath + "/_Data/StageData.json";
      string json = JsonConvert.SerializeObject(m_StageData, new JsonSerializerSettings {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Formatting = Formatting.Indented
      });
      File.WriteAllText(path, json);
    }

    private Position GetTargetPosition(Vector3 position) {
      return new Position { X = position.x, Y = Constants.TARGET_LOCATION_Y_SPAWN_LOCATION, Z = 0f };
    }
  }
}