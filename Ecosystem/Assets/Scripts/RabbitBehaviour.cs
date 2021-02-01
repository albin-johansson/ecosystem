using UnityEngine;
using UnityEngine.AI;

public class RabbitBehaviour : MonoBehaviour
{
  public NavMeshAgent navAgent;
  public GameObject food; // FIXME should not be hardcoded
  public GameObject water; // FIXME should not be hardcoded

  [Tooltip("What is considered to be the ground")]
  public LayerMask groundMask;

  [Tooltip("What is considered to be food")]
  public LayerMask foodMask;

  [Tooltip("What is considered to be water")]
  public LayerMask waterMask;

  public float sightRange = 5;

  private double _hunger;
  private double _thirst;

  private void TargetRandomDestination()
  {
    if (navAgent.hasPath)
    {
      return;
    }

    var randomX = Random.Range(-5f, 5f);
    var randomZ = Random.Range(-5f, 5f);

    var position = transform.position;
    var destination = new Vector3(position.x + randomX, position.y, position.z + randomZ);

    if (Physics.Raycast(destination, -transform.up, 2f, groundMask))
    {
      navAgent.SetDestination(destination);
    }
  }

  private bool InSightRange(LayerMask mask)
  {
    return Physics.CheckSphere(transform.position, sightRange, mask);
  }

  private bool IsFoodInSight()
  {
    return InSightRange(foodMask);
  }

  private bool IsWaterInSight()
  {
    return InSightRange(waterMask);
  }

  private void SearchForFood()
  {
    if (IsFoodInSight())
    {
      navAgent.SetDestination(food.transform.position);
    }
    else
    {
      TargetRandomDestination();
    }
  }

  private void SearchForWater()
  {
    if (IsWaterInSight())
    {
      navAgent.SetDestination(water.transform.position);
    }
    else
    {
      TargetRandomDestination();
    }
  }

  private void Update()
  {
    _hunger += 0.1 * Time.deltaTime;
    _thirst += 0.2 * Time.deltaTime;
    if (_hunger > _thirst)
    {
      SearchForFood();
    }
    else
    {
      SearchForWater();
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Food"))
    {
      Destroy(other);
      _hunger = 0;
    }

    if (other.CompareTag("Water"))
    {
      _thirst = 0;
    }
  }
}