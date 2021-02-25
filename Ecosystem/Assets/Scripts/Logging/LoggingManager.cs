﻿using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

namespace Ecosystem.Logging
{
  public sealed class LoggingManager : MonoBehaviour
  {
    [SerializeField] private Text aliveCountText;
    [SerializeField] private Text deadCountText;
    [SerializeField] private Text foodCountText;
    [SerializeField] private Text timePassedText;

    private int _aliveCount;
    private int _deadCount;
    private int _foodCount;
    private long _nextUpdateTime;

    private void Start()
    {
      // Yes, these are allocated once, it's fine
      DeathHandler.OnDeath += LogDeath;
      FoodConsumer.OnFoodEaten += LogFoodEaten;

      /*
       * The following counting logic assumes that only the root objects of our prefabs feature
       * the identifying tags. If that wouldn't be the case, this approach would overestimate the
       * amounts.
       */
      _aliveCount = GameObject.FindGameObjectsWithTag("Prey").Length +
                    GameObject.FindGameObjectsWithTag("Predator").Length;

      _foodCount = GameObject.FindGameObjectsWithTag("Food").Length;

      aliveCountText.text = _aliveCount.ToString();
      foodCountText.text = _foodCount.ToString();
      deadCountText.text = "0";
    }

    private void Update()
    {
      var nowMilliseconds = AnalyticsSessionInfo.sessionElapsedTime;
      if (nowMilliseconds > _nextUpdateTime)
      {
        var seconds = nowMilliseconds / 1_000;
        timePassedText.text = seconds.ToString();
        _nextUpdateTime = nowMilliseconds + 1_000;
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

    private void LogFoodEaten()
    {
      --_foodCount;
      foodCountText.text = _foodCount.ToString();
    }
  }
}