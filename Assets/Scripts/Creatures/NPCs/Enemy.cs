using System;
using System.Collections;
using UnityEngine;
using Creatures.NPCs.FiniteStateMachine;
using Health;
using Magic;

namespace Creatures.NPCs {
    public class Enemy : Creature {
        [SerializeField] private Creature target;
        [SerializeField] private float pursueDistance;
        [SerializeField] private float attackDistance;

        [SerializeField] private float moveSpeed;

        [SerializeField] private Spell spell;
        [SerializeField] private float spellDelay = 1f;

        private FSM fsm;
        private CharacterController cc;
        private CastStats castStats = new CastStats();

        private Animator animator;

        private void Awake() {
            cc = GetComponent<CharacterController>();

            castStats.casterID = gameObject.GetInstanceID();
            castStats.casterManaController = GetComponent<ManaController>();

            animator = GetComponent<Animator>();

            State[] states = new State[] {
                new IdleState(this, target, animator, pursueDistance),
                new PursueState(this, target, animator, pursueDistance, attackDistance, moveSpeed, cc),
                new AttackState(this, target, animator, attackDistance, moveSpeed, cc, spell, spellDelay, castStats)
            };

            fsm = new FSM(states[0], states);
        }

        protected override void Start() {
            base.Start();
            fsm.Start();
            GetComponent<HealthController>().currentValueChanged += playDamagedAnimation;
        }

        private void Update() {
            fsm.UpdateTick();
        }

        private void playDamagedAnimation(float delta) {
            if (delta < 0) {
                animator.SetTrigger("TookDamage");
            }
        }
    }
}