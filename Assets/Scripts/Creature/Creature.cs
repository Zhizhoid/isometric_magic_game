using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Creature
{
    [RequireComponent(typeof(HealthController))]
    public class Creature : MonoBehaviour {
        private HealthController healthController;
        protected virtual void Start() {
            healthController = GetComponent <HealthController>();
            healthController.currentHealthChanged += onCurrentHealthChanged;
        }

        public void TakeDamage() {
            Debug.Log(gameObject.name + " took damage");
            healthController.ChangeCurrentHealth(-1f);
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
