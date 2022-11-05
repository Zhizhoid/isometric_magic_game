using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Magic;

namespace Creature.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private Spell spell1;
        [SerializeField] private Spell spell2;

        private Player player;
        private CastStats castStats = new CastStats();

        private void Start() {
            player = GetComponent<Player>();
        }

        public void HandleCombat()
        {
            if (Input.GetMouseButtonDown(0))
            {
                refreshCastStats();
                spell1.Cast(castStats);
            }

            if (Input.GetMouseButtonDown(1))
            {
                refreshCastStats();
                spell2.Cast(castStats);
            }
        }

        private void refreshCastStats()
        {
            castStats.castPosition = transform.position;
            
            Ray ray = player.GetCamera().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                castStats.castDirection = new Vector3(hit.point.x - transform.position.x, 0f, hit.point.z - transform.position.z).normalized;
            }

            castStats.casterID = gameObject.GetInstanceID();
        }
    }
}
