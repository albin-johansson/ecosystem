using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RabbitBehaviour : MonoBehaviour
{
    public Camera cam;
    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject food, water;
    public LayerMask whatIsGround, whatIsFood, whatIsWater;
    public double hunger, thirst;
    public Vector3 walkPoint;
    
    

    public bool foodInSightRange, waterInSightRange;
    public float sightRange;

    // Start is called before the first frame update
    void Start()
    {
        hunger = 0;
        thirst = 0;
    }

    
    private void SearchFood(){
        foodInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsFood);
        if(foodInSightRange){
            agent.SetDestination(food.transform.position);
        } else {
            Search();
        }
        
    }

    private void SearchWater(){
        waterInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsWater);
        if(waterInSightRange){
            agent.SetDestination(water.transform.position);
        } else {
            Search();
        }
         
    }

    private void Search(){
        if(!agent.hasPath){
            float randomZ = Random.Range(-5f, 5f);
            float randomX= Random.Range(-5f, 5f);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)){
                agent.SetDestination(walkPoint);
            }
        }
    }


    
    void Update()
    {   
        hunger += 0.1*Time.deltaTime;
        thirst += 0.2*Time.deltaTime;
        if(hunger > thirst){
            SearchFood();
        } else {
            SearchWater();
        }
    }

    private void OnTriggerEnter(Collider other){
         if(other.tag =="Food"){
             Debug.Log("Food detected");
              Destroy(other);
         }
         
          if(other.tag =="Water"){
             Debug.Log("Water detected");
             thirst = 0;
         }

        

    }

}
