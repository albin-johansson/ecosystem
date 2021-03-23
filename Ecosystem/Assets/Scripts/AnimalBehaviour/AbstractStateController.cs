using UnityEngine;

namespace Ecosystem.AnimalBehaviour
{
  public abstract class AbstractStateController : MonoBehaviour, IStateController
  {
    protected IAnimalState State;

    public abstract void Start();

    public abstract void SwitchState(AnimalState state);

    public void Update()
    {
      var newState = State.Tick();
      if (newState != State.Type())
      {
        SwitchState(newState);
      }
    }

    public void OnTriggerEnter(Collider other)
    {
      State.OnTriggerEnter(other);
    }

    public void OnTriggerExit(Collider other)
    {
      State.OnTriggerExit(other);
    }
  }
}