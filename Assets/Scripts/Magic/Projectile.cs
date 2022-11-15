using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    using Creature;

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Projectile : MonoBehaviour
    {
        public Vector3 direction;
        public int casterID;

        [SerializeField] private float lifetime;
        [SerializeField] private float velocity;
        [SerializeField] private float acceleration;

        private void Start()
        {
            StartCoroutine(setDespawn());    
        }

        private void Update()
        {
            transform.position += direction.normalized * velocity * Time.deltaTime;
            velocity += acceleration * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Environment"))
            {
                Destroy(gameObject);
                return;
            }
            
            if (other.TryGetComponent(out Creature creature) && other.gameObject.GetInstanceID() != casterID)
            {
                creature.TakeDamage();
                Destroy(gameObject);
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
