using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ecosystem
{
  public class LoggingManager : MonoBehaviour
  {
    [SerializeField] private Text textField;
    private static List<GameObject> _trackedObjects = new List<GameObject>();

    private static List<DeathData> _deathData = new List<DeathData>();
    //private GameObject[] _rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

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

    public void Update()
    {
      textField.text = "Number of dead:" + _deathData.Count;
    }

    //TODO: track the food objects somehow. From _rootGameObjects is probably expensive. 

    public void OnApplicationQuit()
    {
      //save data.  
      //throw new NotImplementedException();
    }
  }
}