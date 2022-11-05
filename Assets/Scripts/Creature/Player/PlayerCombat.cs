using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Magic;

namespace Creature.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private Spell spell1;
        [SerializeField] private Spell spell2;

        private CastStats castStats = new CastStats();

        public void HandleCombat()
        {
            if (Input.GetMouseButtonDown(0))
            {
                refreshCastStats();
                spell1.Cast(castStats);
                //Debug.Log("I am casting a spell - projectile");
            }

            if (Input.GetMouseButtonDown(1))
            {
                refreshCastStats();

                //Debug.Log("I am casting a spell - puddle");
                //spell2.Cast();
            }
        }

        private void refreshCastStats()
        {
            castStats.castPosition = transform.position;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                castStats.castDirection = new Vector3(hit.point.x - transform.position.x, 0f, hit.point.z - transform.position.z).normalized;
            }

            castStats.casterID = gameObject.GetInstanceID();
        }
    }
}
