using UnityEngine;
using UnityEngine.AI;

namespace Ecosystem.Util
{
  public sealed class Terrains : MonoBehaviour
  {
    [SerializeField] private Terrain terrain;
    public static Terrains instance;
    private const float Range = 10.0f;

    public static int Walkable;

    /// <summary>
    ///   Makes the class able to be used as a singleton 
    /// </summary>
    private void Awake()
    {
      Walkable = 1 << NavMesh.GetAreaFromName("Walkable");
      instance = this;
    }

    /// <summary>
    ///   Attempts to find a random but walkable position in the specified terrain. This function
    ///   will give up if it can't find a walkable positions after 50 iterations.
    /// </summary>
    /// <param name="terrain">the terrain to obtain a position in.</param>
    /// <param name="position">the resulting walkable position, set to <c>Vector3.zero</c> on failure.</param>
    /// <returns><c>true</c> on sucess; <c>false</c> otherwise.</returns>
    public static bool RandomWalkablePosition(Terrain terrain, out Vector3 position)
    {
      var terrainData = terrain.terrainData;
      var size = terrainData.bounds.size;

      for (var i = 0; i < 50; ++i)
      {
        var origin = new Vector3(Random.Range(0, size.x), 0, Random.Range(0, size.z));
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

    public Vector3 SampleTerrainHeight(Vector3 position)
    {
      position.y = terrain.SampleHeight(position);
      return position;
    }
  }
}