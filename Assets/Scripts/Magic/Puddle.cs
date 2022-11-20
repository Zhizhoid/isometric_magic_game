using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Health;

namespace Magic
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Puddle : MagicalEntity
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageble damageble)) {
                hitTarget(damageble);
            }
        }
    }
}
