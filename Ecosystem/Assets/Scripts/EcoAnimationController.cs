using System;
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

    private void Start()
    {
      _animator = GetComponent<Animator>();
      _isWalkingHash = Animator.StringToHash("isWalking");
      _isRunningHash = Animator.StringToHash("isRunning");
      _isDeadHash = Animator.StringToHash("isDead");
      _isAttackingHash = Animator.StringToHash("isAttacking");
      _animationSpeedMultiplier = Animator.StringToHash("animationSpeedMultiplier");
    }

    private void Update()
    {
      if (navMeshAgent.speed > 0)
      {
        _navMeshAgentSpeed = navMeshAgent.speed;
      }

      _animator.SetFloat(_animationSpeedMultiplier, navMeshAgent.velocity.magnitude);
    }

    private void ResetAnimatorParameters()
    {
      foreach (var parameter in _animator.parameters)
      {
        if (parameter.type == UnityEngine.AnimatorControllerParameterType.Bool)
        {
          _animator.ResetTrigger(parameter.name);
        }
      }
    }

    public void AttackAnimation()
    {
      ResetAnimatorParameters();
      _animator.SetTrigger(_isAttackingHash);
      IdleAnimation();
    }

    public void MoveAnimation()
    {
      ResetAnimatorParameters();
      _animator.SetBool(_isWalkingHash, true);
    }

    public void DieAnimation()
    {
      ResetAnimatorParameters();
      navMeshAgent.speed = 0;
      _animator.SetBool(_isDeadHash, true);
    }

    public void IdleAnimation()
    {
      ResetAnimatorParameters();
    }
  }
}