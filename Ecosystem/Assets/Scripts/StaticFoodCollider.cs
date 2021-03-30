using System;
using Ecosystem.Util;
using UnityEditor.UIElements;
using UnityEngine;

namespace Ecosystem
{
  public class StaticFoodCollider : MonoBehaviour
  {
    [SerializeField] private StationaryFoodGeneration stationaryFoodGeneration;

    private int _count;
    private float _time;
    private float _rate = 1;

    private void Update()
    {
      if (_count < 1)
      {
        return;
      }

      if (_time < _rate)
      {
        _time += Time.deltaTime;
        return;
      }

      _time = 0;
      for (int i = 0; i < _count; i++)
      {
        stationaryFoodGeneration.AmountOfBerries -= 1;
        var location = stationaryFoodGeneration.BerriesPlaced.Pop();
        transform.GetChild(location).gameObject.SetActive(false);
        if (stationaryFoodGeneration.AmountOfBerries < 1)
        {
          gameObject.SetActive(false);
          break;
        }
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      if (Tags.IsBerryEater(other.gameObject))
      {
        _count++;
      }
    }

    private void OnTriggerExit(Collider other)
    {
      if (Tags.IsBerryEater(other.gameObject))
      {
        _count--;
      }
    }
  }
}