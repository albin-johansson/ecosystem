using System;

namespace Ecosystem.Logging
{
  /// <summary>
  /// Provides enumerators that represent different causes of death.
  /// </summary>
  /// <remarks>
  /// It's important to not change the order of any of the enumerators,
  /// as they are saved as raw integers in log files. When adding new enumerators,
  /// add them to the end of the enum <b>AND</b> remember to extend the associated enum
  /// in the <c>causeofdeath.py</c> script.
  /// </remarks>
  [Serializable]
  public enum CauseOfDeath
  {
    Starvation,
    Dehydration,
    Eaten
  }
}