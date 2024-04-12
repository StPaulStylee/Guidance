using Guidance.Data;
using Guidance.Gameplay.Obstacles;
using Guidance.Gameplay.Stage;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Guidance.Editor {
  public class StageCreator : OdinEditorWindow {
    private Transform m_RootTransform;
    [SerializeField] private StageData[] m_StageData;
    [SerializeField] private int m_StageNumber;
    [SerializeField] private Vector3 m_TargetLocation;
    [SerializeField] private List<ObstacleData> m_ObstacleData;

    [MenuItem("Tools/Stage Editor")]
    private static void OpenWindow() {
      GetWindow<StageCreator>().Show();
    }

    protected override void OnEnable() {
      base.OnEnable();
      m_RootTransform = GameObject.Find("Game").transform;
      LoadStageData();
    }

    [Button("Add Stage Data")]
    private void AddStage() {
      ArrayUtility.Add(ref m_StageData, new StageData());
    }

    [Button("Capture Stage Data")]
    private void CaptureStage() {
      m_StageNumber = m_StageData.Length;
      m_TargetLocation = m_RootTransform.GetComponentInChildren<Target>().transform.position;
      Obstacle[] obstacles = m_RootTransform.GetComponentsInChildren<Obstacle>();
      foreach (Obstacle obstacle in obstacles) {
        ObstacleData data = new ObstacleData {
          Position = new Vector3(obstacle.transform.position.x, obstacle.transform.position.y, obstacle.transform.position.z),
          Rotation = obstacle.transform.eulerAngles.z,
          Scale = obstacle.transform.localScale.x,
          TypeId = obstacle.TypeId
        };
        Debug.Log($"TargetLocation: {m_TargetLocation}, Position: {data.Position}, Rotation: {data.Rotation}, Scale: {data.Scale}, TypeId: {data.TypeId}");
      }
    }

    private void LoadStageData() {
      string json = File.ReadAllText(Application.dataPath + "/_Data/StageData.json");
      m_StageData = JsonConvert.DeserializeObject<StageData[]>(json);
    }

    private void SaveStageData() {
      string path = Application.dataPath + "/_Data/StageData.json";
      string json = JsonUtility.ToJson(m_StageData, true);
      File.WriteAllText(path, json);
    }
  }
}
