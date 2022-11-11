using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    using Creature;
    public class Puddle : MonoBehaviour
    {
        [SerializeField] private float lifetime;

        private void Start()
        {
            StartCoroutine(setDespawn());
        }

        private void OnTriggerEnter(Collider other)
        {
            Creature creature;
            if (other.TryGetComponent<Creature>(out creature))
            {
                creature.TakeDamage();
            }
        }

        private IEnumerator setDespawn()
        {
            float end = Time.time + lifetime;
            while (Time.time < end)
            {
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}
