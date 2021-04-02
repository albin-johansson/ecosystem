using System;
using Unity.Entities;

namespace Ecosystem.Components
{
  [Serializable]
  public struct MovingTowardsFood : IComponentData
  {
    public Entity Food;
  }
}