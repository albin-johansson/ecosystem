using System;
using System.Diagnostics;
using System.Linq.Expressions;
using Ecosystem.AnimalBehaviour;
using TMPro;
using UnityEngine;

namespace Ecosystem.UI
{
  public sealed class StateText : MonoBehaviour
  {
    [SerializeField] private TMP_Text textMesh;
    private string _text;

    public void SetText(AnimalState state)
    {
      switch (state)
      {
        case AnimalState.LookingForWater:
          _text = "Looking for water";
          break;
        case AnimalState.LookingForFood:
          _text = "Looking for food";
          break;
        case AnimalState.RunningTowardsWater:
          _text = "Going to water";
          break;
        case AnimalState.RunningTowardsFood:
          _text = "Going to food";
          break;
        case AnimalState.ChasingPrey:
          _text = "Chasing prey";
          break;
        case AnimalState.LookingForMate:
          _text = "Looking for mate";
          break;
        default:
          _text = state.ToString();
          break;
      }
      textMesh.text = _text;
    }
  }
}