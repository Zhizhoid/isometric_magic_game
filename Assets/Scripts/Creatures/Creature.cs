using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Health;


namespace Creatures
{
    [RequireComponent(typeof(StatusController))]
    [RequireComponent(typeof(HealthController))]
    public class Creature : MonoBehaviour, IDamageble {
        private HealthController healthController;
        private StatusController statusController;

        protected virtual void Start() {
            healthController = GetComponent<HealthController>();
            healthController.currentValueChanged += onCurrentHealthChanged;

            statusController = GetComponent<StatusController>();
        }

        public void AddStatusEffect(StatusEffect statusEffect)
        {
            statusController.AddStatusEffect(statusEffect);
        }

        public void TakeDamage(Damage damage) {
            Debug.Log(gameObject.name + " took " + damage.amount + " damage");
            healthController.ChangeCurrentHealth(-damage.amount);
        }

        private void onCurrentHealthChanged(float delta) {
            if (healthController.GetCurrentHealth() <= 0f) {
                die();
            }
        }
        private void die() {
            Debug.Log(gameObject.name + " died");
            gameObject.SetActive(false);
        }
    }
}
