using UnityEngine;

public static class CubeMeshData {
  public static Vector3[] Vertices = {
    new Vector3( 1,  1,  1), // North Face (Z going away. Just like Unity)
    new Vector3(-1,  1,  1),
    new Vector3(-1, -1,  1),
    new Vector3( 1, -1,  1),
    new Vector3(-1,  1, -1),// South Face
    new Vector3( 1,  1, -1),
    new Vector3( 1, -1, -1),
    new Vector3(-1, -1, -1),
  };

  public static int[][] FaceTriangles = {
    new int[] { 0, 1, 2, 3}, // North Face
    new int[] { 5, 0, 3, 6}, // East Face
    new int[] { 4, 5, 6, 7}, // South Face
    new int[] { 1, 4, 7, 2}, // West Face
    new int[] { 1, 0, 5, 4}, // Top Face
    new int[] { 2, 3, 6, 7}, // Bottom Face
  };
}
