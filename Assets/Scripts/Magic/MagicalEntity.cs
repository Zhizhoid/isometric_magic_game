using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Health;

namespace Magic
{
    public abstract class MagicalEntity : MonoBehaviour
    {
        [SerializeField] protected float lifetime;
        [SerializeField] protected Damage damage;

        protected virtual void Start()
        {
            StartCoroutine(setDespawn());
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
