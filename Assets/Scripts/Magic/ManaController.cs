using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic {
    public class ManaController : MonoBehaviour, UI.IBarValue {
        [SerializeField] private float maxMana = 100f;
        [SerializeField] private float refillSpeed = 10f;

        private float currentMana;

        public event Action<float> currentValueChanged;

        private void Start() {
            currentMana = maxMana;
        }

        private void Update() {
            if (currentMana < maxMana) {
                float prevCurrentMana = currentMana;
                currentMana = Mathf.Min(currentMana + refillSpeed * Time.deltaTime, maxMana);
                currentValueChanged?.Invoke(currentMana - prevCurrentMana);
            }
        }

        public bool UseMana(float amount) {
            if (amount > currentMana) {
                return false;
            }
            else {
                currentMana -= amount;
                currentValueChanged?.Invoke(-amount);
                return true;
            }
        }

        public float GetCurrentValue() {
            return currentMana;
        }

        public float GetMaxValue() {
            return maxMana;
        }
    }
}
