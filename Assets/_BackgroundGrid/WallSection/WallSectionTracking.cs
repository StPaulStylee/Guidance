using UnityEngine;

namespace Guidance.Gameplay.BackgroundGrid {
  public struct WallSectionTracking {
    public WallSection CurrentWallSectionHead;
    public int CurrentWallSectionHeadIndex;
    public WallSection CurrentWallSectionTail;
    public Transform WallSectionParent;
  }
}
