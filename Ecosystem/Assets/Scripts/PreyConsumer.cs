﻿using UnityEngine;

public sealed class PreyConsumer : MonoBehaviour
{
    [SerializeField] private double rate = 0.02;
    [SerializeField] private double threshold = 0.02;
    [SerializeField] private HungerBar hungerBar;

    private double Hunger { get; set; }

    private void Start()
    {
        hungerBar.SetMaxHunger(1f);   //Should be set to max hunger before death
    }

    private void Update()
    {
        Hunger += rate * Time.deltaTime;
        hungerBar.SetHunger((float)Hunger);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Prey>() != null)
        {
            Destroy(other.gameObject);
            Hunger = 0;
        }
    }

    internal bool IsHungry()
    {
        return Hunger > threshold;
    }
}
