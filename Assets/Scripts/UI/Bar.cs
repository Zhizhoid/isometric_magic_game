using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(Slider))]
    public class Bar : MonoBehaviour {
        
        [SerializeField] private GameObject barValueObject;
        [SerializeField] private Component barValueComponent;
        private IBarValue barValue;
        [SerializeField] private float lerpSpeed = 10f;
        
        private Slider slider;
        private float targetValue = 1f;

        private void Start() {
            //barValue = barValueObject.GetComponent<IBarValue>();
            barValue = barValueComponent as IBarValue;
            slider = GetComponent<Slider>();

            barValue.currentValueChanged += (delta) => {
                targetValue = barValue.GetCurrentValue() / barValue.GetMaxValue();
            };
        }

        private void Update() {
            slider.value = Mathf.Lerp(slider.value, targetValue, lerpSpeed * Time.deltaTime);
        }
    }
}