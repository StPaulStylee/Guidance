using System.Collections.Generic;

namespace Guidance.Stage.Data {
  public class StageCreatorData {
    public const string PrefabPath = "Assets/_Obstacle";
    public static List<ObstacleAssetData> ObstacleAssetData = new List<ObstacleAssetData> {
      new ObstacleAssetData(ObstacleAssetDataKey.Green, "Obstacle_Green/"),
      new ObstacleAssetData(ObstacleAssetDataKey.White, "Obstacle_White/"),
      new ObstacleAssetData(ObstacleAssetDataKey.Red, "Obstacle_Red/"),
      new ObstacleAssetData(ObstacleAssetDataKey.Blue, "Obstacle_Blue/"),
      new ObstacleAssetData(ObstacleAssetDataKey.Yellow, "Obstacle_Yellow/"),
      new ObstacleAssetData(ObstacleAssetDataKey.Gold, "Obstacle_Gold/")

    };
  }

  public class ObstacleAssetDataKey {
    public const string Green = "green";
    public const string White = "white";
    public const string Red = "red";
    public const string Blue = "blue";
    public const string Yellow = "yellow";
    public const string Gold = "gold";
  }

  public class ObstacleAssetData {
    public string Name { get; set; }
    public string Subdirectory { get; set; }
    public ObstacleAssetData(string name, string subdirectory) {
      Name = name;
      Subdirectory = subdirectory;
    }
  }
}
