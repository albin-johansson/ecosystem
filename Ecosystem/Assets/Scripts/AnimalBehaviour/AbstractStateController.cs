using Ecosystem.UI;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour
{
  public abstract class AbstractStateController : MonoBehaviour, IStateController
  {
    protected IAnimalState State;
    [SerializeField] private StateText StateText;

    public abstract void Start();

    public abstract void SwitchState(AnimalState state);

    public void Update()
    {
      var newState = State.Tick();
      if (newState != State.Type())
      {
        SwitchState(newState);
        //Uncomment for debugging
        //StateText.SetText(newState.ToString());
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