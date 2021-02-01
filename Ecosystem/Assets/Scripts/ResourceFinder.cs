using UnityEngine;
using UnityEngine.AI;

public sealed class ResourceFinder : MonoBehaviour
{
  [SerializeField] private NavMeshAgent navAgent;
  [SerializeField] private FoodConsumer foodConsumer;
  [SerializeField] private WaterConsumer waterConsumer;

  private void OnTriggerEnter(Collider other)
  {
    // Hunger has implicit priority
    if (foodConsumer.IsHungry() && other.GetComponent<Food>() != null ||
        waterConsumer.IsThirsty() && other.GetComponent<Water>() != null)
    {
      navAgent.SetDestination(other.transform.position);
    }
  }
}