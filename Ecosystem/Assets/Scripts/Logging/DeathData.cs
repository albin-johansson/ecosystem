using UnityEngine;
using UnityEngine.Analytics;

namespace Ecosystem.Logging
{
  public readonly struct DeathData
  {
    private CauseOfDeath CauseOfDeath { get; }
    private long TimeOfDeath { get; }
    private GameObject DeadObject { get; }

    public DeathData(CauseOfDeath cause, GameObject gameObject)
    {
      TimeOfDeath = AnalyticsSessionInfo.sessionElapsedTime;
      CauseOfDeath = cause;
      DeadObject = gameObject;
    }
  }
}