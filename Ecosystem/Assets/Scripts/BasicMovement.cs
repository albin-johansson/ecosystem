using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public sealed class BasicMovement : MonoBehaviour
{
  [SerializeField] private NavMeshAgent navAgent;
  [SerializeField] private BasicNeeds basicNeeds;

  [SerializeField, Tooltip("What is considered to be the ground")]
  private LayerMask groundMask;


  private (string, Vector3)[] memory;

  private int temp = 0;

  void Start()
  {
    memory = new (string, Vector3)[5];
    for (int i = 0; i < memory.Length; i++)
    {
      memory[i].Item1 = "";
    }
  }

  void Update()
  {
    if (!navAgent.hasPath)
    {
      string prio = basicNeeds.getPriority();
      Vector3 dest = RecallFromMemory(prio);
      navAgent.SetDestination(dest);
    }

  }

  private Vector3 RecallFromMemory(string s)
  {
    List<Vector3> select = new List<Vector3>();
    for (int i = 0; i < memory.Length; i++)
    {
      if (memory[i].Item1.Equals(s))
      {
        if (memory[i].Item1.Equals("Food"))
        {
          memory[i].Item1 = "";
        }
        select.Add(memory[i].Item2);
      }
    }

    if (select.Count > 0)
    {
      return select[Random.Range(0, select.Count - 1)];
    }

    return TargetRandomDestination();
  }


  private void OnTriggerEnter(Collider other)
  {
    if (other.GetComponent<Food>())
    {
      memory[temp] = ("Food", other.gameObject.transform.position);
      temp++;
      if (temp > 4)
      {
        temp = 0;
      }
    } else if (other.GetComponent<Water>())
    {
      memory[temp] = ("Water", other.gameObject.transform.position);
      temp++;
      if (temp > 4)
      {
        temp = 0;
      }
    }
  }

  private Vector3 TargetRandomDestination()
  {
    var randomX = Random.Range(-5f, 5f);
    var randomZ = Random.Range(-5f, 5f);

    var position = transform.position;
    var destination = new Vector3(position.x + randomX, position.y, position.z + randomZ);

    if (Physics.Raycast(destination, -transform.up, 2f, groundMask))
    {
      return destination;
    }

    return position;
  }
}