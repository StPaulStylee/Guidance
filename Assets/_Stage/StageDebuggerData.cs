using Guidance.Data;
using Guidance.Gameplay.Game.Controller;

namespace Guidance.Stage.Data {
  public class StageDebuggerData {
    public StageData[] Data { get; set; }
    public GameController Controller { get; set; }
    public StageDebuggerData(StageData[] data, GameController controller) {
      Data = data;
      Controller = controller;
    }
  }
}
