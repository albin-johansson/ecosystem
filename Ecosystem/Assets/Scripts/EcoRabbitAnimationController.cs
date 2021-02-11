using UnityEngine;

public sealed class EcoRabbitAnimationController : MonoBehaviour
{
  [SerializeField] private AnimationStatesController animationStatesController;
  private Animator _animator;
  private int isJumpingHash;
  private int isRunningHash;
  private int isDeadHash;
  private AnimationState animationState;

  private void Start()
  {
    _animator = GetComponent<Animator>();
    isJumpingHash = Animator.StringToHash("isJumping");
    isRunningHash = Animator.StringToHash("isRunning");
    isDeadHash = Animator.StringToHash("isDead_0");
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
        _animator.SetBool(isJumpingHash, true);
        break;
      case AnimationState.Running:
        _animator.SetBool(isRunningHash, true);
        break;
      case AnimationState.Dead:
        _animator.SetBool(isDeadHash, true);
        break;
      case AnimationState.Idle:
        break;
      case AnimationState.Attacking:
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