using UnityEngine;
using UnityEngine.AI;

public class EcoRabbitAnimationController : MonoBehaviour
{
  [SerializeField] private AnimStatesController animStatesController;
  private NavMeshAgent navAgent;
  private Animator animator;
  private int isJumpingHash;
  private int isLookingOutHash;
  private int isRunningHash;
  private int isDeadHash;
  private int isJumpingUpHash;

  private void Start()
  {
    navAgent = GetComponent<NavMeshAgent>();
    animator = GetComponent<Animator>();
    isJumpingHash = Animator.StringToHash("isJumping");
    isLookingOutHash = Animator.StringToHash("isLookingOut");
    isRunningHash = Animator.StringToHash("isRunning");
    isDeadHash = Animator.StringToHash("isDead_0");
    isJumpingUpHash = Animator.StringToHash("isJumpingUp");
  }

  private void Update()
  {
    switch (AnimStatesController.Animstate)
    {
      case State.Idle:
        animator.SetBool(isJumpingHash, false);
        break;
      case State.Walking:
        animator.SetBool(isJumpingHash, true);
        break;
    }
  }
}