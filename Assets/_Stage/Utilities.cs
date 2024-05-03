using Guidance.Stage.Data;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace Guidance.Stage {
  public static class Utilities {
    public static StageData[] GetStageData() {
      string json = File.ReadAllText(Application.dataPath + "/_Data/StageData.json");
      return JsonConvert.DeserializeObject<StageData[]>(json);
    }
  }
}
