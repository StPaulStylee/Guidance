using Guidance.Stage.Data;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace Guidance.Stage {
  public static class Utilities {
    public static string StageDataFileName { get; } = "StageData_Test";
    //public static string StageDataFileName { get; } = "StageData_Prod";

    public static void InitializeStageDataToPersistentPath() {
      string persistentPath = Path.Combine(Application.persistentDataPath, StageDataFileName);
      string streamingDataPath = Path.Combine(Application.streamingAssetsPath, StageDataFileName);

      if (!File.Exists(persistentPath)) {
        // If I want to build to Android or WebGL I think I need to use UnityWebRequest to read the file?
        File.Copy(streamingDataPath, persistentPath);
      }
    }

    public static StageData[] GetStageData() {
      if (Application.isEditor) {
        string json = File.ReadAllText(Application.dataPath + $"/_Data/{StageDataFileName}.json");
        return JsonConvert.DeserializeObject<StageData[]>(json);
      }
      TextAsset jsonFile = Resources.Load<TextAsset>(StageDataFileName);
      return JsonConvert.DeserializeObject<StageData[]>(jsonFile.text);
    }
  }
}
