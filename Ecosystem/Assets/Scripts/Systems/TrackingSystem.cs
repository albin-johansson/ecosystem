using Unity.Entities;
using Reese.Nav;
using Unity.Mathematics;

namespace Ecosystem.Systems
{
  public sealed class TrackingSystem : SystemBase
  {
    protected override void OnUpdate()
    {
    }

    public void SetDestination(Entity entity, float3 destination)
    {
      EntityManager.AddComponentData(entity, new NavDestination
      {
              Teleport = false,
              Tolerance = 10f,
              CustomLerp = false,
              WorldPoint = destination
      });
    }
  }
}