﻿using System;

namespace Ecosystem
{
  /// <summary>
  /// Provides enumerators that represent different causes of death.
  /// </summary>
  /// <remarks>
  /// It's important to not change the order of any of the enumerators,
  /// as they are saved as raw integers in log files. When adding new enumerators,
  /// add them to the end of the enum!
  /// </remarks>
  [Serializable]
  public enum CauseOfDeath
  {
    Starvation,
    Dehydration,
    Eaten
  }
}