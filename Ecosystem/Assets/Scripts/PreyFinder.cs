using UnityEngine;
using UnityEngine.AI;

public sealed class PreyFinder : MonoBehaviour
{
  [SerializeField] private NavMeshAgent navAgent;
  [SerializeField] private PreyConsumer preyConsumer;
  [SerializeField] private WaterConsumer waterConsumer;

  private void OnTriggerEnter(Collider other)
  {
    // Hunger has implicit priority
    if (preyConsumer.IsHungry() && other.GetComponent<Prey>() != null ||
        waterConsumer.IsThirsty() && other.GetComponent<Water>() != null)
    {
      navAgent.SetDestination(other.transform.position);
    }
  }
}
