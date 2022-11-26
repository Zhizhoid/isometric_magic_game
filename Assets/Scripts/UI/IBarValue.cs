using System;

namespace UI {
    public interface IBarValue {
        public float GetMaxValue();
        public float GetCurrentValue();
        public event Action currentValueChanged;
    }
}
