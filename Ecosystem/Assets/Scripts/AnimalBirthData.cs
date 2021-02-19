using UnityEngine;
using UnityEngine.Analytics;

namespace Ecosystem
{
  public struct AnimalBirthData
  {
    public GameObject Animal { get; }
    public long Time { get; }

    public AnimalBirthData(GameObject animal)
    {
      Animal = animal;
      Time = AnalyticsSessionInfo.sessionElapsedTime;
    }
  }
}