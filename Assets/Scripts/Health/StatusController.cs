using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Health
{
    [RequireComponent(typeof(HealthController))]
    public class StatusController : MonoBehaviour {
        private HealthController healthController;

        class CurrentEffect {
            public StatusEffect effect;
            public float liftTime;
            public float nextTick;

            public CurrentEffect(StatusEffect _effect) {
                effect = _effect;
                liftTime = Time.time + effect.lifetime;
                nextTick = Time.time;
            }
        }
        
        private Dictionary<string, CurrentEffect> currentEffects = new Dictionary<string, CurrentEffect>();

        private void Start() {
            healthController = GetComponent<HealthController>();
        }

        private void Update() {
            List<string> effectsToLift = new List<string>();
            foreach (var valuePair in currentEffects) {
                if(Time.time >= valuePair.Value.nextTick)
                {
                    int ticksToDo = Mathf.FloorToInt( (Time.time - valuePair.Value.nextTick) / valuePair.Value.effect.tickRate ) + 1;
                    for(int i = 0; i < ticksToDo; i++)
                    {
                        healthController.ChangeCurrentHealth(-valuePair.Value.effect.tickDamage.amount);
                    }
                    valuePair.Value.nextTick += valuePair.Value.effect.tickRate * ticksToDo;
                }
                if(Time.time >= valuePair.Value.liftTime)
                {
                    effectsToLift.Add(valuePair.Key);
                }
            }
            if(effectsToLift.Count > 0)
            {
                foreach(var key in effectsToLift)
                {
                    currentEffects.Remove(key);
                }
            }
        }

        public void AddStatusEffect(StatusEffect statusEffect)
        {
            if (!currentEffects.ContainsKey(statusEffect.name)) {
                currentEffects[statusEffect.name] = new CurrentEffect(statusEffect);
                Debug.Log(currentEffects[statusEffect.name].effect.name);
            }
        }
    }
}
