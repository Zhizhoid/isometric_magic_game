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

        [SerializeField] protected bool hasStatusEffect;
        [SerializeField] protected StatusEffect statusEffect;

        protected virtual void Start()
        {
            StartCoroutine(setDespawn());
        }

        protected void hitTarget(IDamageble damageble)
        {
            damageble.TakeDamage(damage);
            if (hasStatusEffect)
            {
                damageble.AddStatusEffect(statusEffect);
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
