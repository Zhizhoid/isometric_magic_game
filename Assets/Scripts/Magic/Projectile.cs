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

        [SerializeField] private float velocity;
        [SerializeField] private float acceleration;

        private void Update()
        {
            transform.position += direction.normalized * velocity * Time.deltaTime;
            velocity += acceleration * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Environment")
            {
                Destroy(gameObject);
                return;
            }

            Creature creature;
            if (other.TryGetComponent<Creature>(out creature) && other.GetInstanceID() != casterID)
            {
                creature.TakeDamage();
            }
        }
    }
}
