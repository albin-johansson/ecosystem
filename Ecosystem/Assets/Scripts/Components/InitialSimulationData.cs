using System;
using Unity.Entities;

namespace Ecosystem.Components
{
  [Serializable]
  public struct InitialSimulationData : IComponentData
  {
    public int initialRabbitCount;
    public int initialWolfCount;
    public int initialCarrotCount;
  }
}