using UnityEngine;

namespace Ecosystem.AnimalBehaviour
{
  public abstract class AbstractStateController : MonoBehaviour, IStateController
  {
    public IAnimalState _state;

    public virtual void Start()
    {
    }

    public void Update()
    {
      var newState = _state.Tick();
      if (newState != _state.Type())
      {
        SwitchState(newState);
      }
    }

    public void OnTriggerEnter(Collider other)
    {
      _state.OnTriggerEnter(other);
    }

    public void OnTriggerExit(Collider other)
    {
      _state.OnTriggerExit(other);
    }

    public virtual void SwitchState(AnimalState state)
    {
    }
  }
}
