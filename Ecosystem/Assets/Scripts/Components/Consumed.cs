using System;
using Unity.Entities;

namespace Ecosystem.Components
{
  [Serializable]
  public struct Consumed : IComponentData
  {
    public double when;
  }
}