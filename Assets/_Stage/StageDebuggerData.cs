using Guidance.Data;
using Guidance.Gameplay.Game.Controller;
using Guidance.Gameplay.Stage;
using UnityEngine;

namespace Guidance.Stage.Data {
  public class StageDebuggerData {
    public StageData[] Data { get; set; }
    public GameController Controller { get; set; }
    public StageManager StageManager { get; set; }
    public GameObject Root { get; set; }
    public GameObject StageViewer { get; set; }
    public StageDebuggerData(StageData[] data, GameController controller, GameObject root, GameObject stageViewer, StageManager stageManager) {
      Data = data;
      Controller = controller;
      Root = root;
      StageViewer = stageViewer;
      StageManager = stageManager;
    }
  }
}
