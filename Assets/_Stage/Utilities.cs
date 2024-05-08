using Guidance.Stage.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace Guidance.Stage {
  public static class Utilities {
    public static StageData[] GetStageData() {
      TextAsset jsonFile = Resources.Load<TextAsset>("StageData_Test");
      return JsonConvert.DeserializeObject<StageData[]>(jsonFile.text);
    }
  }
}
