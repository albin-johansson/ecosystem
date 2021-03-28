using Reese.Spatial;
using Unity.Entities;

namespace Ecosystem.Systems.Collisions
{
  [UpdateInGroup(typeof(SimulationSystemGroup))]
  [UpdateAfter(typeof(SpatialStartSystem))]
  [UpdateBefore(typeof(SpatialEndSystem))]
  public sealed class CollisionSystemGroup : ComponentSystemGroup
  {
  }
}