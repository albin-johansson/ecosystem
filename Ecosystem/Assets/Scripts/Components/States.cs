using System;
using Ecosystem.Logging;
using Unity.Entities;

namespace Ecosystem.Components
{
  [Serializable]
  public struct Roaming : IComponentData
  {
  }

  [Serializable]
  public struct Dead : IComponentData
  {
    public CauseOfDeath cause;
  }
}