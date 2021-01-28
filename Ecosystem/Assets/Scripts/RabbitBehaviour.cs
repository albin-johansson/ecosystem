using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitBehaviour : MonoBehaviour
{
    public Camera cam;
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform food;
    public LayerMask whatIsGround, whatIsFood;

    public bool foodInSightRange;
    public float sightRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void ChaseFood(){
        agent.SetDestination(food.position);
    }

    // Update is called once per frame
    void Update()
    {
        foodInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsFood);

        if(foodInSightRange) ChaseFood();

        if (Input.GetMouseButtonDown(0)){
            Ray ray =cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit)){
                agent.SetDestination(hit.point);
            }
        }
    }
}
