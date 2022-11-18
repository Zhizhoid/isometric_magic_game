using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Health
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 5f;
        private float currentHealth;

        public event Action currentHealthChanged;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void ChangeCurrentHealth(float delta)
        {
            currentHealth += delta;
            currentHealthChanged?.Invoke();
        }

        public float GetMaxHealth()
        {
            return maxHealth;
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }
    }
}
