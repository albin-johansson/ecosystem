using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EcoRabbitAnimationController : MonoBehaviour

{
  [SerializeField] private AnimationStatesController animationStatesController;
  private Animator _animator;
  private int _isJumpingHash;
  private int _isRunningHash;
  private int _isDeadHash;
  private State _state;

  private void Start()
  {
    _animator = GetComponent<Animator>();
    _isJumpingHash = Animator.StringToHash("isJumping");
    _isRunningHash = Animator.StringToHash("isRunning");
    _isDeadHash = Animator.StringToHash("isDead_0");
    _state = State.Idle;
  }

  private void Update()
  {
    var incomingState = animationStatesController.Animstate;
    if (incomingState == _state)
    {
      return;
    }

    _state = incomingState;
    ClearParameters();
    SetParameter(_state);
  }

  private void SetParameter(State newState)
  {
    switch (newState)
    {
      case State.Walking:
        _animator.SetBool(_isJumpingHash, true);
        break;
      case State.Running:
        _animator.SetBool(_isRunningHash, true);
        break;
      case State.Dead:
        _animator.SetBool(_isDeadHash, true);
        break;
    }
  }

  private void ClearParameters()
  {
    foreach (var parameter in _animator.parameters)
    {
      _animator.ResetTrigger(parameter.name);
    }
  }
}