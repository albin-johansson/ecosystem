using Ecosystem.Genes;
using Ecosystem.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ecosystem.AnimalBehaviour
{
  public abstract class AbstractStateController : MonoBehaviour, IStateController
  {
    protected IAnimalState State;
    [SerializeField] protected StateText StateText;
    [SerializeField] protected WaterConsumer waterConsumer;
    [SerializeField] protected MovementController movementController;
    [SerializeField] protected EcoAnimationController animationController;
    [SerializeField] protected MemoryController memoryController;
    [SerializeField] protected Reproducer reproducer;
    [SerializeField] protected AbstractGenome genome;


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