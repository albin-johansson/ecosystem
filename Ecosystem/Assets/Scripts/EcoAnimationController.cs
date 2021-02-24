﻿using System;
using UnityEngine;
using UnityEngine.AI;

namespace Ecosystem
{
  public class EcoAnimationController : MonoBehaviour
  {
    [SerializeField] private AnimationStatesController animationStatesController;
    [SerializeField] private NavMeshAgent navMeshAgent;
    private Animator _animator;
    private int _isWalkingHash;
    private int _isRunningHash;
    private int _isDeadHash;
    private int _isAttackingHash;
    private int _animationSpeedMultiplier;
    private AnimationState _animationState;

    private void Start()
    {
      _animator = GetComponent<Animator>();
      _isWalkingHash = Animator.StringToHash("isWalking");
      _isRunningHash = Animator.StringToHash("isRunning");
      _isDeadHash = Animator.StringToHash("isDead");
      _isAttackingHash = Animator.StringToHash("isAttacking");
      _animationSpeedMultiplier = Animator.StringToHash("animationSpeedMultiplier");
      _animationState = AnimationState.Idle;
    }

    private void Update()
    {
      _animator.SetFloat(_animationSpeedMultiplier, navMeshAgent.velocity.magnitude);
      var incomingState = animationStatesController.AnimationState;
      if (incomingState == _animationState)
      {
        return;
      }
      _animationState = incomingState;
      ResetAnimatorParameters();
      SetAnimatorParameter(_animationState);
    }

    private void SetAnimatorParameter(AnimationState newAnimationState)
    {
      switch (newAnimationState)
      {
        case AnimationState.Walking:
          _animator.SetBool(_isWalkingHash, true);
          break;
        case AnimationState.Running:
          _animator.SetBool(_isRunningHash, true);
          break;
        case AnimationState.Dead:
          navMeshAgent.speed = 0;
          _animator.SetBool(_isDeadHash, true);
          break;
        case AnimationState.Attacking:
          _animator.SetBool(_isAttackingHash, true);
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
        if (parameter.type == UnityEngine.AnimatorControllerParameterType.Bool)
        {
          _animator.ResetTrigger(parameter.name);
        }
      }
    }
  }
}