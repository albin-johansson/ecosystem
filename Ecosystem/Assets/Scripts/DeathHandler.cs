using UnityEngine;
using UnityEngine.AI;

namespace Ecosystem
{
  /// <summary>
  /// Handles killing the game object.
  /// </summary>
  public sealed class DeathHandler : MonoBehaviour
  {
    [SerializeField] private AnimationStatesController animationStatesController;

    private void DestroyObjectDelayed()
    {
      Destroy(gameObject.gameObject, 3);
    }

    public void Die(CauseOfDeath cause)
    {
      animationStatesController.AnimationState = AnimationState.Dead;
      DestroyObjectDelayed();
    }
  }
}