using UnityEngine;
using UnityEngine.Analytics;

namespace Ecosystem
{
  public struct DeathData
  {
    private CauseOfDeath causeOfDeath { get; }
    private long timeOfDeath { get; }
    private GameObject deadObject { get; }

    public DeathData(CauseOfDeath cause, GameObject gameObject)
    {
      causeOfDeath = cause;
      deadObject = gameObject;
      timeOfDeath = AnalyticsSessionInfo.sessionElapsedTime;
    }
  }
}