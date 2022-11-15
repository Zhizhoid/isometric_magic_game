using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Creature
{
    [RequireComponent(typeof(HealthController))]
    public class Creature : MonoBehaviour, IDamageble {
        private HealthController healthController;
        protected virtual void Start() {
            healthController = GetComponent <HealthController>();
            healthController.currentHealthChanged += onCurrentHealthChanged;
        }

        public void TakeDamage(Damage damage) {
            Debug.Log(gameObject.name + " took " + damage.amount + " damage");
            healthController.ChangeCurrentHealth(-damage.amount);
        }

        private void onCurrentHealthChanged() {
            if (healthController.GetCurrentHealth() <= 0f) {
                die();
            }
        }
        private void die() {
            gameObject.SetActive(false);
        }
    }
}
