using Ecosystem.Consumer;
using Ecosystem.Genes;
using Ecosystem.UI;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour
{
  public abstract class AbstractStateController : MonoBehaviour
  {
    [SerializeField] protected StateText stateText;
    [SerializeField] protected GenderIcon genderIcon;
    [SerializeField] protected StaminaController staminaController;
    [SerializeField] protected WaterConsumer waterConsumer;
    [SerializeField] protected MovementController movementController;
    [SerializeField] protected EcoAnimationController animationController;
    [SerializeField] protected MemoryController memoryController;
    [SerializeField] protected Reproducer reproducer;
    [SerializeField] protected AbstractGenome genome;
    [SerializeField] protected SphereCollider sphereCollider;

    protected IConsumer Consumer;

    protected IAnimalState State;

    protected abstract void SwitchState(AnimalState state);

    protected virtual void Initialize()
    {
    }

    private void Start()
    {
      Initialize();
      genderIcon.SetGenderIcon(genome.IsMale);
    }

    private void Update()
    {
      var newState = State.Tick();
      if (newState != State.Type())
      {
        SwitchState(newState);
        stateText.SetText(newState);
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      if (movementController.IsReachable(other.gameObject.transform.position))
      {
        State.OnSphereEnter(other);
      }
    }

    private void OnTriggerExit(Collider other)
    {
      State.OnSphereExit(other);
    }
  }
}