using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ecosystem
{
  public sealed class LoggingManager : MonoBehaviour
  {
    private static List<GameObject> _trackedObjects = new List<GameObject>();
    private static List<DeathData> _deathData = new List<DeathData>();
    private static GameObject[] _rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();


    public void OnApplicationQuit()
    {
      //save data.  
      throw new NotImplementedException();
    }

    public static void OnDeath(DeathData deathData)
    {
      //Save deathData. 
      _deathData.Add(deathData);
    }

    public static void OnBirth(AnimalBirthData animalBirthData)
    {
      //TODO: what else should be done?
      _trackedObjects.Add(animalBirthData.Animal);
    }

    //TODO: track the food objects somehow. From _rootGameObjects is probably expensive. 
  }
}