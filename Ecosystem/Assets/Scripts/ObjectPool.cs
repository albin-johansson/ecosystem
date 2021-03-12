using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem
{
  public class ObjectPool : MonoBehaviour
  {
    [SerializeField] private GameObject prefab;
    [SerializeField] private Queue<GameObject> animalPool = new Queue<GameObject>();
    [SerializeField] private int poolStartSize;

    private void Start()
    {
      for (var i = 0; i < poolStartSize; i++)
      {
        var animal = Instantiate(prefab);
        animalPool.Enqueue(animal);
        animal.SetActive(false);
      }
    }
    public GameObject GetAnimal(){

      if (animalPool.Count > 0)
      {
        var animal = animalPool.Dequeue();
        return animal;
      }
      else
      {
        var animal = Instantiate(prefab);
        return animal;
      }
    }
    
   public void ReturnAnimal(GameObject animal)
    {
      animalPool.Enqueue(animal);
      animal.SetActive(false);
    }
  }
}