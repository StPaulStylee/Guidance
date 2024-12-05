using System.Collections.Generic;

namespace _Stage {
  public class StageCreatorData {
    public const string PREFAB_PATH = "Assets/_Obstacle";

    public static readonly List<ObstacleAssetData> ObstacleAssetData = new() {
      new ObstacleAssetData(ObstacleAssetDataKey.GREEN, "Obstacle_Green/"),
      new ObstacleAssetData(ObstacleAssetDataKey.WHITE, "Obstacle_White/"),
      new ObstacleAssetData(ObstacleAssetDataKey.RED, "Obstacle_Red/"),
      new ObstacleAssetData(ObstacleAssetDataKey.BLUE, "Obstacle_Blue/"),
      new ObstacleAssetData(ObstacleAssetDataKey.YELLOW, "Obstacle_Yellow/"),
      new ObstacleAssetData(ObstacleAssetDataKey.GOLD, "Obstacle_Gold/")
    };
  }

  public class ObstacleAssetDataKey {
    public const string GREEN = "green";
    public const string WHITE = "white";
    public const string RED = "red";
    public const string BLUE = "blue";
    public const string YELLOW = "yellow";
    public const string GOLD = "gold";
  }

  public class ObstacleAssetData {
    public ObstacleAssetData(string name, string subdirectory) {
      Name = name;
      Subdirectory = subdirectory;
    }

    public string Name { get; set; }
    public string Subdirectory { get; set; }
  }
}
