using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Health;

namespace Magic
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Projectile : MagicalEntity
    {
        private Vector3 direction;
        private int casterID;

        [SerializeField] private float velocity;
        [SerializeField] private float acceleration;

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
            
            if (other.TryGetComponent(out IDamageble damageble) && other.gameObject.GetInstanceID() != casterID)
            {
                damageble.TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        public void SetCasterID(int id)
        {
            casterID = id;
        }

        public void SetDirection(Vector3 _direction)
        {
            direction = _direction;
        }
    }
}
