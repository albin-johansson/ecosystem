using UnityEngine;

namespace Ecosystem.AnimalBehaviour
{
  public interface IAnimalState
  {
    void Begin(GameObject target);

    GameObject End();

    AnimalState Tick();

    void OnTriggerEnter(Collider other);

    void OnTriggerExit(Collider other);

    AnimalState Type();
  }
}