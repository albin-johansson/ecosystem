using UnityEngine;

namespace Ecosystem.AnimalBehaviour
{
  public interface IStateController
  {
    void Start();

    void Update();

    void OnTriggerEnter(Collider other);

    void OnTriggerExit(Collider other);

    void SwitchState(AnimalState state);
  }
}