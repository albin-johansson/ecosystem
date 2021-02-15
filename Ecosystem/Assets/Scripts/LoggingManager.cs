using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class LoggingManager : MonoBehaviour
{
  private List<GameObject> trackedObjects = new List<GameObject>();

  // called zero
  private void Awake()
  {
    Debug.Log("Awake");
  }

  // called first
  private void OnEnable()
  {
    foreach (var gameObject
      in SceneManager.GetActiveScene().GetRootGameObjects())
    {
      if (false)
      {
        trackedObjects.Add(gameObject);
      }
    }
  }

  // called second
  private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
  }

  // called third
  private void Start()
  {
    Debug.Log("Start");
    SceneManager.GetActiveScene().GetRootGameObjects();
  }

  void OnDisable()
  {
    int i = 0;
    // Write to file/json/store data

    /*
    Debug.Log("OnDisable");
    SceneManager.sceneLoaded -= OnSceneLoaded;
    */
  }

  private void UpdateData()
  {
    var time = AnalyticsSessionInfo.sessionElapsedTime;
  }
}