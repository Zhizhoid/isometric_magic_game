using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Health
{
    public class HealthController : MonoBehaviour, UI.IBarValue
    {
        [SerializeField] private float maxHealth = 5f;
        private float currentHealth;

        public event Action<float> currentValueChanged;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void ChangeCurrentHealth(float delta)
        {
            currentHealth += delta;
            currentValueChanged?.Invoke(delta);
        }

        public float GetMaxHealth()
        {
            return maxHealth;
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        public float GetMaxValue() {
            return maxHealth;
        }

        public float GetCurrentValue() {
            return currentHealth;
        }
    }
}
