using System;
using Unity.Entities;

namespace Ecosystem.Components
{
  [Serializable]
  public struct MovingTowardsResource : IComponentData
  {
    public Entity Resource;
  }
}