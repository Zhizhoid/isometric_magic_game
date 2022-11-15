using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    using Creature;
    
    [RequireComponent(typeof(Rigidbody))]
    public class Puddle : MonoBehaviour
    {
        [SerializeField] private float lifetime;

        private void Start()
        {
            StartCoroutine(setDespawn());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Creature creature))
            {
                Debug.Log(1);
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