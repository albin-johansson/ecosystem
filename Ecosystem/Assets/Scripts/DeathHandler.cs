﻿using UnityEngine;
using UnityEngine.AI;

namespace Ecosystem
{
  /// <summary>
  /// Handles killing the game object.
  /// </summary>
  public sealed class DeathHandler : MonoBehaviour
  {
    [SerializeField] private EcoAnimationController ecoAnimationController;

    private void DestroyObjectDelayed()
    {
      Destroy(gameObject.gameObject, 3);
    }

    public void Die(CauseOfDeath cause)
    {
      ecoAnimationController.DieAnimation();
      DestroyObjectDelayed();
    }
  }
}