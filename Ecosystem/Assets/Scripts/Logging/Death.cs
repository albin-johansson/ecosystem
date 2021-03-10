using System;

namespace Ecosystem.Logging
{
  /// <summary>
  ///   Provides information about a death event.
  /// </summary>
  [Serializable]
  public sealed class Death
  {
    /// <summary>
    ///   The cause of death.
    /// </summary>
    public CauseOfDeath cause;
  }
}