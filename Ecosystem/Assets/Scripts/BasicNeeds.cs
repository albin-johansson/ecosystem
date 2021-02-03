using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNeeds : MonoBehaviour
{
    [SerializeField] private FoodConsumer foodConsumer;
    [SerializeField] private WaterConsumer waterConsumer;

    // Update is called once per frame
    void Update()
    {
        
    }

    internal string getPriority()
    {
        if (foodConsumer.IsHungry())
        {
            return "Food";
        }
        else if (waterConsumer.IsThirsty())
        {
            return "Water";
        }

        return "Idle";
    }
}
