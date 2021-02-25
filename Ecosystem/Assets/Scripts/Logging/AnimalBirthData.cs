using UnityEngine;
using UnityEngine.Analytics;

namespace Ecosystem.Logging
{
  public struct AnimalBirthData
  {
    public GameObject Animal { get; }
    public long Time { get; }

    public AnimalBirthData(GameObject animal)
    {
      Time = AnalyticsSessionInfo.sessionElapsedTime;
      Animal = animal;
    }
  }
}