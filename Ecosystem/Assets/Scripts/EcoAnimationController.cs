using UnityEngine;

public class EcoAnimationController : MonoBehaviour
{
  [SerializeField] private AnimationStatesController animationStatesController;
  private Animator _animator;
  private int isWalkingHash;
  private int isRunningHash;
  private int isDeadHash;
  private int isAttackingHash;
  private AnimationState animationState;

  private void Start()
  {
    _animator = GetComponent<Animator>();
    switch (_animator.runtimeAnimatorController.name)
    {
      case "Wolf":
        isWalkingHash = Animator.StringToHash("isWalking");
        break;
      case "Rabbit":
        isWalkingHash = Animator.StringToHash("isJumping");
        break;
      default:
        break;
    }

    isRunningHash = Animator.StringToHash("isRunning");
    isDeadHash = Animator.StringToHash("isDead_0");
    isAttackingHash = Animator.StringToHash("isAttacking");
    animationState = AnimationState.Idle;
  }

  private void Update()
  {
    var incomingState = animationStatesController.AnimAnimationState;
    if (incomingState == animationState)
    {
      return;
    }

    animationState = incomingState;
    ResetAnimatorParameters();
    SetAnimatorParameter(animationState);
  }

  private void SetAnimatorParameter(AnimationState newAnimationState)
  {
    switch (newAnimationState)
    {
      case AnimationState.Walking:
        _animator.SetBool(isWalkingHash, true);
        break;
      case AnimationState.Running:
        _animator.SetBool(isRunningHash, true);
        break;
      case AnimationState.Dead:
        _animator.SetBool(isDeadHash, true);
        break;
      case AnimationState.Attacking:
        _animator.SetBool(isAttackingHash, true);
        break;
      case AnimationState.Idle:
        break;
      default:
        break;
    }
  }

  private void ResetAnimatorParameters()
  {
    foreach (var parameter in _animator.parameters)
    {
      _animator.ResetTrigger(parameter.name);
    }
  }
}