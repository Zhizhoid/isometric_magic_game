using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Creatures.NPCs.FiniteStateMachine;
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
        private CastStats castStats;

        private void Awake() {
            State[] states = new State[] {
                new IdleState(this),
                new PursueState(this),
                new AttackState(this)
            };
            fsm = new FSM(states[0], states);

            cc = GetComponent<CharacterController>();

            castStats.casterID = gameObject.GetInstanceID();
            castStats.casterManaController = GetComponent<ManaController>();
        }

        protected override void Start() {
            base.Start();
            fsm.Start();
        }

        private void Update() {
            fsm.UpdateTick();
        }

        private class IdleState : State
        {
            private Enemy me;

            public IdleState(Enemy _me)
            {
                me = _me;
            }

            public void OnEnter()
            {
                Debug.Log("Entering idle");
            }

            public void OnUpdate()
            {

            }

            public void OnExit()
            {
                Debug.Log("Exiting idle");
            }

            public Type ShouldExit()
            {
                float distanceToPlayerSquared = MyMath.SquareOf(me.target.transform.position.x - me.transform.position.x) +
                                                MyMath.SquareOf(me.target.transform.position.y - me.transform.position.y) +
                                                MyMath.SquareOf(me.target.transform.position.z - me.transform.position.z);

                if (distanceToPlayerSquared < MyMath.SquareOf(me.pursueDistance))
                {
                    return typeof(PursueState);
                }

                return null;
            }
        }

        private class PursueState : State
        {
            private Enemy me;

            public PursueState(Enemy _me)
            {
                me = _me;
            }

            public void OnEnter()
            {
                Debug.Log("Starting to persue " + me.target.name);
            }

            public void OnUpdate()
            {
                me.transform.LookAt(me.target.transform.position);
                me.cc.Move((me.target.transform.position - me.transform.position).normalized * me.moveSpeed * Time.deltaTime);
            }

            public void OnExit()
            {
                Debug.Log("Exiting pursue");
            }

            public Type ShouldExit()
            {
                float distanceToPlayerSquared = MyMath.SquareOf(me.target.transform.position.x - me.transform.position.x) +
                                                MyMath.SquareOf(me.target.transform.position.y - me.transform.position.y) +
                                                MyMath.SquareOf(me.target.transform.position.z - me.transform.position.z);

                if (distanceToPlayerSquared > MyMath.SquareOf(me.pursueDistance))
                {
                    return typeof(IdleState);
                }
                else if (distanceToPlayerSquared < MyMath.SquareOf(me.attackDistance))
                {
                    return typeof(AttackState);
                }

                return null;
            }
        }

        private class AttackState : State
        {
            private Enemy me;
            private float lastCastTime = 0f;

            public AttackState(Enemy _me)
            {
                me = _me;
            }

            public void OnEnter()
            {
                Debug.Log("Starting to attack " + me.target.name);
            }

            public void OnUpdate()
            {
                me.transform.LookAt(me.target.transform.position);
                me.cc.Move((me.target.transform.position - me.transform.position).normalized * me.moveSpeed * Time.deltaTime);

                if (Time.time >= lastCastTime + me.spellDelay)
                {
                    me.castStats.castPosition = me.transform.position;

                    me.castStats.castPoint = me.target.transform.position;

                    me.spell.Cast(me.castStats);
                    lastCastTime = Time.time;
                }
            }

            public void OnExit()
            {
                Debug.Log("Exiting attack");
            }

            public Type ShouldExit()
            {
                float distanceToPlayerSquared = MyMath.SquareOf(me.target.transform.position.x - me.transform.position.x) +
                                                MyMath.SquareOf(me.target.transform.position.y - me.transform.position.y) +
                                                MyMath.SquareOf(me.target.transform.position.z - me.transform.position.z);

                if (distanceToPlayerSquared > MyMath.SquareOf(me.attackDistance))
                {
                    return typeof(PursueState);
                }

                return null;
            }
        }
    }
}
