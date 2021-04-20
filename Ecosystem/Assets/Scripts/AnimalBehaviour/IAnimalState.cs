using UnityEngine;

namespace Ecosystem.AnimalBehaviour
{
  public interface IAnimalState
  {
    void Begin(GameObject target);

    GameObject End();

    AnimalState Tick();

    void OnSphereEnter(Collider other);

    void OnSphereExit(Collider other);

    AnimalState Type();
  }
}