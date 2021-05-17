using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Ecosystem.Util
{
  public static class Terrains
  {
    private const float Range = 10.0f;

    public static readonly int Walkable = 1 << NavMesh.GetAreaFromName("Walkable");

    /// <summary>
    ///   Attempts to find a random but walkable position in the specified terrain. This function
    ///   will eventually give up if it can't find a walkable position.
    /// </summary>
    /// <param name="terrain">the terrain to obtain a position in.</param>
    /// <param name="position">the resulting walkable position, set to <c>Vector3.zero</c> on failure.</param>
    /// <returns><c>true</c> on success; <c>false</c> otherwise.</returns>
    public static bool RandomWalkablePosition(Terrain terrain, out Vector3 position)
    {
      var terrainData = terrain.terrainData;
      var size = terrainData.bounds.size;
      var terrainPosition = terrain.transform.position;

      for (var i = 0; i < 20; ++i)
      {
        var x = terrainPosition.x + Random.Range(0, size.x);
        var z = terrainPosition.z + Random.Range(0, size.z);

        var origin = new Vector3(x, 0, z);
        var y = terrain.SampleHeight(origin) + 10;

        origin.y = y;

        var randomPoint = origin + Random.insideUnitSphere * Range;
        if (NavMesh.SamplePosition(randomPoint, out var hit, 10.0f, Walkable))
        {
          position = hit.position;
          return true;
        }
      }

      position = Vector3.zero;
      return false;
    }
  }
}