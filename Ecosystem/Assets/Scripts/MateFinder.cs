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
            if (genome == null)
            {
                Debug.Log("Fuuuck");
            }
            if (other.TryGetComponent(out Genome otherGenome1))
            {
                Debug.Log("Found other genome");
            }
            if (other.TryGetComponent(out Genome otherGenome))
            {
                Debug.Log("Found other creature");
                if (genome.MatchesGenome(otherGenome))
                {
                    Debug.Log("Found creature is valid mate");
                    targetTracker.SetTarget(other.gameObject);
                }
            }
            }
    }
}
