using UnityEngine;

namespace Ecosystem
{
  public class StaticFoodCollider : MonoBehaviour
  {
    [SerializeField] private StationaryFoodGeneration stationaryFoodGeneration;

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Prey"))
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