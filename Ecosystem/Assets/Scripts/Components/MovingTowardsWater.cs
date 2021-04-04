using System;
using Unity.Entities;

namespace Ecosystem.Components
{
  [Serializable]
  public struct MovingTowardsWater : IComponentData
  {
    public Entity Water;
  }
}