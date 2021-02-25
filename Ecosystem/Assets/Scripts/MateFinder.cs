using System.Collections;
using System.Collections.Generic;
using Ecosystem.Genes;
using UnityEngine;

namespace Ecosystem
{
    public class MateFinder : MonoBehaviour
    {
        [SerializeField] private TargetTracker targetTracker;

        private Desire _priority = Desire.Idle;
        
        void Start()
        {
        
        }
        
        void Update()
        {
            
        }

        /// <summary>
        /// When colliding with an object, that object is saved to the animals memory, and subsequently set as a target if the
        /// priority matches.
        /// </summary>
        /// TODO Might be an improvement to only save the object and not set it as a target.
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent(typeof(RabbitGenome)) != null)
                {
                    Debug.Log("Found mate");
                    targetTracker.SetTarget(other.gameObject);
                }
            }
    }
}
