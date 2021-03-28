using Unity.Entities;
using Unity.Physics.Systems;

namespace Ecosystem.Systems.Movement
{
  [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
  [UpdateBefore(typeof(BuildPhysicsWorld))]
  public class MovementSystemGroup : ComponentSystemGroup
  {
  }
}