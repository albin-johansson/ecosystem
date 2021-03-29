using UnityEngine;
using UnityEngine.AI;

namespace Ecosystem
{
  public class EcoAnimationController : MonoBehaviour
  {
    [SerializeField] private NavMeshAgent navMeshAgent;
    private Animator _animator;
    private int _isWalkingHash;
    private int _isRunningHash;
    private int _isDeadHash;
    private int _isAttackingHash;
    private int _animationSpeedMultiplier;
    private float _navMeshAgentSpeed;
    private float _navMeshAgentVelocity;

    private bool _isDead;

    private void OnEnable()
    {
      _animator = GetComponent<Animator>();
      _isWalkingHash = Animator.StringToHash("isWalking");
      _isRunningHash = Animator.StringToHash("isRunning");
      _isDeadHash = Animator.StringToHash("isDead");
      _isAttackingHash = Animator.StringToHash("isAttacking");
      _animationSpeedMultiplier = Animator.StringToHash("animationSpeedMultiplier");
      _navMeshAgentSpeed = navMeshAgent.speed;
      _isDead = false;
      ResetAnimatorParameters();
      _navMeshAgentSpeed = navMeshAgent.speed;
    }

    private void Update()
    {
      if (navMeshAgent.speed > 0)
      {
        _navMeshAgentSpeed = navMeshAgent.speed;
        _navMeshAgentVelocity = navMeshAgent.velocity.magnitude;
      }

      _animator.SetFloat(_animationSpeedMultiplier, _navMeshAgentVelocity);
    }

    private void ResetAnimatorParameters()
    {
      foreach (var parameter in _animator.parameters)
      {
        if (parameter.type == AnimatorControllerParameterType.Bool)
        {
          _animator.ResetTrigger(parameter.name);
        }
      }
    }

    public void EnterAttackAnimation()
    {
      ResetAnimatorParameters();
      navMeshAgent.speed = 0;
      _animator.SetTrigger(_isAttackingHash);
      IdleAnimation();
    }

    public void MoveAnimation()
    {
      if (_isDead)
      {
        return;
      }
      ResetAnimatorParameters();
      navMeshAgent.speed = _navMeshAgentSpeed;
      _animator.SetBool(_isWalkingHash, true);
    }

    public void RunAnimation()
    {
      if (_isDead)
      {
        return;
      }
      ResetAnimatorParameters();
      navMeshAgent.speed = _navMeshAgentSpeed;
      _animator.SetBool(_isRunningHash, true);
    }

    public void EnterDeathAnimation()
    {
      ResetAnimatorParameters();
      navMeshAgent.speed = 0;
      _animator.SetBool(_isDeadHash, true);
      _isDead = true;
    }

    public void IdleAnimation()
    {
      ResetAnimatorParameters();
    }

    public bool IsIdle()
    {
      return !_animator.GetBool(_isAttackingHash) && _animator.GetAnimatorTransitionInfo(0).IsName("Attack -> Idle");
    }
  }
}