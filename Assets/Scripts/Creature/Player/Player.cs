using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerCombat))]
    public class Player : Creature
    {
        private PlayerMovement pMovement;
        private PlayerCombat pCombat;

        [SerializeField] private Camera cam;

        protected override void Start()
        {
            base.Start();
            pMovement = GetComponent<PlayerMovement>();
            pCombat = GetComponent<PlayerCombat>();
        }

        private void Update()
        {
            pMovement.HandleMovement();
            pCombat.HandleCombat();
        }

        public Camera GetCamera() {
            return cam;
        }
    }
}
