using Guidance.Stage.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace Guidance.Stage {
  public static class Utilities {
    //public static string StageDataFileName { get; } = "StageData_Test";
    public static string StageDataFileName { get; } = "StageData_Prod";

    public static StageData[] GetStageData() {
      TextAsset jsonFile = Resources.Load<TextAsset>(StageDataFileName);
      return JsonConvert.DeserializeObject<StageData[]>(jsonFile.text);
    }
  }
}
