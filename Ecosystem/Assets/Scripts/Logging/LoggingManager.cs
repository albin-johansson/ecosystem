using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

namespace Ecosystem.Logging
{
  public sealed class LoggingManager : MonoBehaviour
  {
    [SerializeField] private Text aliveCountText;
    [SerializeField] private Text deadCountText;
    [SerializeField] private Text timePassedText;

    private void Start()
    {
      foreach (var unused in GameObject.FindGameObjectsWithTag("Prey"))
      {
        ++_aliveCount;
      }

      foreach (var unused in GameObject.FindGameObjectsWithTag("Predator"))
      {
        ++_aliveCount;
      }

      aliveCountText.text = _aliveCount.ToString();
      deadCountText.text = "0";

      DeathHandler.OnDeath += LogDeath; // Yes this is allocated once, it's fine
    }

    private void Update()
    {
      var now = AnalyticsSessionInfo.sessionElapsedTime;
      if (now > _nextUpdateTime)
      {
        timePassedText.text = now.ToString();
        _nextUpdateTime = now + 1_000; // Update time label once every second
      }
    }

    private void OnApplicationQuit()
    {
    }

    private void LogDeath(CauseOfDeath cause, GameObject deadObject)
    {
      --_aliveCount;
      ++_deadCount;

      aliveCountText.text = _aliveCount.ToString();
      deadCountText.text = _deadCount.ToString();
    }

    private void LogBirth(AnimalBirthData animalBirthData)
    {
      ++_aliveCount;
    }
  }
}