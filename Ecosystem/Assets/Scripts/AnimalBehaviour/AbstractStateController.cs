using Ecosystem.Genes;
using Ecosystem.UI;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour
{
  public abstract class AbstractStateController : MonoBehaviour, IStateController
  {
    protected IAnimalState State;
    [SerializeField] protected StateText stateText;
    [SerializeField] protected StaminaController staminaController;
    [SerializeField] protected WaterConsumer waterConsumer;
    [SerializeField] protected MovementController movementController;
    [SerializeField] protected EcoAnimationController animationController;
    [SerializeField] protected MemoryController memoryController;
    [SerializeField] protected Reproducer reproducer;
    [SerializeField] protected AbstractGenome genome;
    [SerializeField] protected SphereCollider sphereCollider;


    public abstract void Start();

    public abstract void SwitchState(AnimalState state);

    public void Update()
    {
      var newState = State.Tick();
      if (newState != State.Type())
      {
        SwitchState(newState);
        //Uncomment for debugging
        stateText.SetText(newState.ToString());
      }
    }

    public void OnTriggerEnter(Collider other)
    {
      if (movementController.IsReachable(other.gameObject.transform.position))
      {
        State.OnTriggerEnter(other);
      }
    }

    public void OnTriggerExit(Collider other)
    {
      State.OnTriggerExit(other);
    }
  }
}