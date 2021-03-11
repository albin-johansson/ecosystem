using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem
{
  public class StaticFoodCollider : MonoBehaviour
  {
    [SerializeField] private StationaryFoodGeneration stationaryFoodGeneration;


    //Handles the consuming of berries, when all berries has been consumed the bush will deactivate itself until new berries are available. 
    private void OnTriggerEnter(Collider other)
    {
      if (Tags.IsPrey(other.gameObject))
      {
        stationaryFoodGeneration.AmountOfBerries -= 1;
        var location = stationaryFoodGeneration.BerriesPlaced.Pop();
        transform.GetChild(location).gameObject.SetActive(false);
        if (stationaryFoodGeneration.AmountOfBerries < 1)
        {
          gameObject.SetActive(false);
        }
      }
    }
  }
}