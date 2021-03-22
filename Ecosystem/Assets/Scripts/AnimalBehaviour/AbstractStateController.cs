using UnityEngine;

namespace Ecosystem.AnimalBehaviour
{
  public abstract class AbstractStateController : MonoBehaviour, IStateController
  {
    protected IAnimalState State;

    public virtual void Start()
    {
    }

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

    public virtual void SwitchState(AnimalState state)
    {
    }
  }
}