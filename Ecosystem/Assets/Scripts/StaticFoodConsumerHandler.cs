using Ecosystem.Spawning;
using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem
{
  public sealed class StaticFoodConsumerHandler : MonoBehaviour
  {
    [SerializeField] private StationaryFoodGeneration stationaryFoodGeneration;

    private const float Rate = 1;

    private int _count;
    private float _time;

    private void Update()
    {
      if (_count < 1)
      {
        return;
      }

      _time += Time.deltaTime;
      if (_time >= Rate)
      {
        for (var i = 0; i < _count; ++i)
        {
          stationaryFoodGeneration.RemoveBerry();
          if (stationaryFoodGeneration.AmountOfBerries < 1)
          {
            gameObject.SetActive(false);
            break;
          }
        }

        _time = 0;
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      if (Tags.IsBerryConsumer(other.gameObject))
      {
        ++_count;
      }
    }

    private void OnTriggerExit(Collider other)
    {
      if (Tags.IsBerryConsumer(other.gameObject))
      {
        --_count;
      }
    }
  }
}