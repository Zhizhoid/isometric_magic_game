using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Health
{
    [RequireComponent(typeof(HealthController))]
    public class StatusController : MonoBehaviour
    {
        private HashSet<StatusEffect> statusEffects = new HashSet<StatusEffect>();
        public void AddStatusEffect(StatusEffect statusEffect)
        {
            statusEffects.Add(statusEffect);
            Debug.Log(statusEffects.Count);
        }
    }
}
