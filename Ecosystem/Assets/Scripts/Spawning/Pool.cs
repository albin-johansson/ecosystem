using System;
using UnityEngine;

namespace Ecosystem.Spawning
{
  /// <summary>
  ///   Contains the value needed to initiate the object pools in ObjectPoolHandler.
  /// </summary>
  [Serializable]
  public sealed class Pool
  {
    public GameObject prefab;
    public int size;
  }
}