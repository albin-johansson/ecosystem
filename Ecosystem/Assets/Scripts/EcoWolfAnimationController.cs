using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
  public class EcoWolfAnimationController : MonoBehaviour
  {
    [SerializeField] private AnimationStatesController animationStatesController;
    private Animator _animator;
    private int _isWalkingHash;
    private int _isRunningHash;
    private int _isDeadHash;
    private int _isAttackingHash;
    private State _state;

    private void Start()
    {
      _animator = GetComponent<Animator>();
      _isWalkingHash = Animator.StringToHash("isWalking");
      _isRunningHash = Animator.StringToHash("isRunning");
      _isDeadHash = Animator.StringToHash("isDead");
      _isAttackingHash = Animator.StringToHash("isAttack-Idle");
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
          _animator.SetBool(_isWalkingHash, true);
          break;
        case State.Running:
          _animator.SetBool(_isRunningHash, true);
          break;
        case State.Dead:
          _animator.SetBool(_isDeadHash, true);
          break;
        case State.Attacking:
          _animator.SetBool(_isAttackingHash, true);
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
}