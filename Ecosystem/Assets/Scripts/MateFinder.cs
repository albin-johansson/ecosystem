using System.Collections;
using System.Collections.Generic;
using Ecosystem.Genes;
using UnityEngine;

namespace Ecosystem
{
    public class MateFinder : MonoBehaviour
    {
        [SerializeField] private TargetTracker targetTracker;
        [SerializeField] private Genome genome;

        void Start()
        {
        
        }
        
        void Update()
        {
            
        }

        /// <summary>
        /// When colliding with an object, that object is saved to the animals memory, and subsequently set as a target if the
        /// genome and sex matches.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("MateFinder collide");
            if (other.TryGetComponent(out Genome otherGenome))
                {
                    Debug.Log("Found creature");
                    if (genome.matchesGenome(otherGenome))
                    {
                        Debug.Log("Found valid mate");
                        targetTracker.SetTarget(other.gameObject);
                    }
                }
            }
    }
}
