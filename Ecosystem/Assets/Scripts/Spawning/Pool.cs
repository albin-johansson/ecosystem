using System;
using UnityEngine;

namespace Ecosystem.Spawning
{
  /// <summary>
  /// Contains the value needed to iniate the objectPools in ObjectPoolHandler.
  /// </summary>
  [Serializable]
  public sealed class Pool
  {
    public string key;
    public GameObject prefab;
    public int size;
  }
}