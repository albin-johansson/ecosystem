using System.Collections;
using System.Collections.Generic;
using Ecosystem.Genes;
using UnityEngine;

namespace Ecosystem
{
    public class Reproducer : MonoBehaviour
    {
        [SerializeField] private Genome genome;
        private double gestationPeriod;
        [SerializeField] private GameObject prefab;

        private bool isPregnant = false;
        private double pregnancyElapsedTime;

        private Genome mateGenome;
    

        // Start is called before the first frame update
        void Start()
        {
            isPregnant = false;
            pregnancyElapsedTime = 0;
            gestationPeriod = genome.GetGestationPeriod();
        }

        // Update is called once per frame
        void Update()
        {
            if (isPregnant)
            {
                pregnancyElapsedTime += Time.deltaTime;
                if (pregnancyElapsedTime >= gestationPeriod)
                {
                    var transform1 = transform;
                    Debug.Log("Childbirth" + pregnancyElapsedTime + isPregnant);

                    isPregnant = false;
                    pregnancyElapsedTime = 0;
                    Instantiate(prefab, transform1.position, transform1.rotation).GetComponent<Genome>().Initialize(genome, mateGenome);
                }
            }
        }

        
        public void startPregnancy(Genome mateGenome)
        {
            isPregnant = true;
            this.mateGenome = mateGenome;
        }
        
        /// <summary>
        /// When colliding with an object, that object is saved to the animals memory, and subsequently set as a target if the
        /// priority matches.
        /// </summary>
        /// TODO Might be an improvement to only save the object and not set it as a target.
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Reproducer mateReproducer))
            {   
                Debug.Log("Reached other creature");
                if (genome.MatchesGenome(mateReproducer.genome))
                {
                    Debug.Log("Matching genome");
                    if (!mateReproducer.isPregnant && !mateReproducer.genome.GetIsMale())
                    {
                        Debug.Log("Mating");
                        other.GetComponent<Reproducer>().startPregnancy(genome);
                    }
                }

            }
        }

        public bool IsPregnant => isPregnant;
    }
}
