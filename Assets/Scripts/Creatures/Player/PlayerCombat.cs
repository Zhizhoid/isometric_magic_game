using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Magic;

namespace Creatures.Player
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(ManaController))]
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private Spell spell1;
        [SerializeField] private Spell spell2;

        private Player player;
        private CastStats castStats = new CastStats();

        private void Start() {
            player = GetComponent<Player>();

            castStats.casterID = gameObject.GetInstanceID();
            castStats.casterManaController = GetComponent<ManaController>();
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
            int layerMask =~ LayerMask.GetMask("Ignore Raycast");
            if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, layerMask))
            {
                castStats.castPoint = hit.point;
            }
        }
    }
}
