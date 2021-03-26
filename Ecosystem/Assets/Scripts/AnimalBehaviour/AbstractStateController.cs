using UnityEngine;

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


    public abstract void Start();

    public abstract void SwitchState(AnimalState state);

    public void Update()
    {
      var newState = State.Tick();
      if (newState != State.Type())
      {
        SwitchState(newState);
        StateText.SetText(newState.ToString());
      }
    }

    public void OnTriggerEnter(Collider other)
    {
      memoryController.SaveToInVision(other.gameObject);
      State.OnTriggerEnter(other);
    }

  public void OnTriggerExit(Collider other)
  {
    StartCoroutine(memoryController.RemoveFromInVision(other.gameObject));
      State.OnTriggerExit(other);
    }
  }
}