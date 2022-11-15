using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(Slider))]
    public class HealthBar : MonoBehaviour {
        [SerializeField] private HealthController hc;
        [SerializeField] private float lerpSpeed = 10f;
        
        private Slider slider;
        private float targetValue = 1f;

        private void Start() {
            slider = GetComponent<Slider>();

            hc.currentHealthChanged += () => {
                targetValue = hc.GetCurrentHealth() / hc.GetMaxHealth();
                Debug.Log(hc.GetCurrentHealth());
            };
        }

        private void Update() {
            slider.value = Mathf.Lerp(slider.value, targetValue, lerpSpeed * Time.deltaTime);
        }
    }
}
