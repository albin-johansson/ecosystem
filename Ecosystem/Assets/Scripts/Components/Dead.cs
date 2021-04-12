using System;
using Ecosystem.Logging;
using Unity.Entities;

namespace Ecosystem.Components
{
  [Serializable]
  public struct Dead : IComponentData
  {
    public double when;
    public CauseOfDeath cause;
  }
}