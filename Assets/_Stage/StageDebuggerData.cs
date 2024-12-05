using _Data;
using _Game;
using UnityEngine;

namespace _Stage {
  public class StageDebuggerData {
    public StageDebuggerData(StageData[] data, GameController controller, GameObject root, GameObject stageViewer,
      StageManager stageManager) {
      Data = data;
      Controller = controller;
      Root = root;
      StageViewer = stageViewer;
      StageManager = stageManager;
      Editing = new EditingData();
    }

    public StageData[] Data { get; set; }
    public GameController Controller { get; set; }
    public StageManager StageManager { get; set; }
    public GameObject Root { get; set; }
    public GameObject StageViewer { get; set; }
    public EditingData Editing { get; set; }

    public class EditingData {
      public bool IsEditing = false;
      public int StageBeingEdited = 0;
    }
  }
}
