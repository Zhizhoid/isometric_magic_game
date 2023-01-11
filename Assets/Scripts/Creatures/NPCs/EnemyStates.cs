using System;
using UnityEngine;
using Creatures.NPCs.FiniteStateMachine;
using Magic;
using Creatures.NPCs.Pathfinding;

namespace Creatures.NPCs
{
    public abstract class EnemyState : State
    {
        protected Enemy me;
        protected Creature target;
        protected Animator animator;

        protected EnemyState(Enemy _me, Creature _target, Animator _animator)
        {
            me = _me;
            target = _target;
            animator = _animator;
        }
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
        public abstract Type ShouldExit();
    }
    
    public class IdleState : EnemyState
    {
        private readonly float pursueDistance;

        public IdleState(Enemy _me, Creature _target, Animator _animator, float _pursueDistance) : base(_me, _target, _animator)
        {
            pursueDistance = _pursueDistance;
        }
    
        public override void OnEnter()
        {
            // Debug.Log("Entering idle");
            animator.SetInteger("State", 0);
        }
    
        public override void OnUpdate() {}
    
        public override void OnExit()
        {
            // Debug.Log("Exiting idle");
        }
    
        public override Type ShouldExit()
        {
            float distanceToPlayerSquared = MyMath.SquareOf(target.transform.position.x - me.transform.position.x) +
                                            MyMath.SquareOf(target.transform.position.y - me.transform.position.y) +
                                            MyMath.SquareOf(target.transform.position.z - me.transform.position.z);
    
            if (distanceToPlayerSquared < MyMath.SquareOf(pursueDistance)) {
                return typeof(PursueState);
            }
    
            return null;
        }
    }
    
    public class PursueState : EnemyState
    {
        private readonly float pursueDistance;
        private readonly float attackDistance;
        private readonly EnemyMovement enemyMovement;

        public PursueState(Enemy _me, Creature _target, Animator _animator, float _pursueDistance, float _attackDistance, EnemyMovement _enemyMovement) : base(_me, _target, _animator)
        {
            pursueDistance = _pursueDistance;
            attackDistance = _attackDistance;
            enemyMovement = _enemyMovement;
        }
    
        public override void OnEnter()
        {
            // Debug.Log("Starting to persue " + target.name);
            animator.SetInteger("State", 1);
        }

        public override void OnUpdate()
        {
            enemyMovement.HandleMovement(target.transform.position);
        }
    
        public override void OnExit()
        {
            // Debug.Log("Exiting pursue");
        }
    
        public override Type ShouldExit()
        {
            float distanceToPlayerSquared = MyMath.SquareOf(target.transform.position.x - me.transform.position.x) +
                                            MyMath.SquareOf(target.transform.position.y - me.transform.position.y) +
                                            MyMath.SquareOf(target.transform.position.z - me.transform.position.z);
    
            if (distanceToPlayerSquared > MyMath.SquareOf(pursueDistance)) {
                return typeof(IdleState);
            }
            else if (distanceToPlayerSquared < MyMath.SquareOf(attackDistance)) {
                return typeof(AttackState);
            }
    
            return null;
        }
    }
    
    public class AttackState : EnemyState
    {
        private readonly float attackDistance;
        private readonly EnemyMovement enemyMovement;
        private readonly Spell spell;
        private readonly float spellDelay;
        private readonly CastStats castStats;
        
        private float lastCastTime = 0f;

        public AttackState(Enemy _me, Creature _target, Animator _animator, float _attackDistance, EnemyMovement _enemyMovement, Spell _spell, float _spellDelay, CastStats _castStats) : base(_me, _target, _animator)
        {
            me = _me;
            target = _target;
            attackDistance = _attackDistance;
            enemyMovement = _enemyMovement;
            spell = _spell;
            spellDelay = _spellDelay;
            castStats = _castStats;
        }
    
        public override void OnEnter()
        {
            animator.SetInteger("State", 2);
            // Debug.Log("Starting to attack " + target.name);
        }
    
        public override void OnUpdate()
        {
            enemyMovement.HandleMovement(target.transform.position);

            if (Time.time >= lastCastTime + spellDelay) {
                castStats.castPosition = me.transform.position;
    
                castStats.castPoint = target.transform.position;
    
                spell.Cast(castStats);
                lastCastTime = Time.time;
            }
        }
    
        public override void OnExit()
        {
            // Debug.Log("Exiting attack");
        }
    
        public override Type ShouldExit()
        {
            float distanceToPlayerSquared = MyMath.SquareOf(target.transform.position.x - me.transform.position.x) +
                                            MyMath.SquareOf(target.transform.position.y - me.transform.position.y) +
                                            MyMath.SquareOf(target.transform.position.z - me.transform.position.z);
    
            if (distanceToPlayerSquared > MyMath.SquareOf(attackDistance)) {
                return typeof(PursueState);
            }
    
            return null;
        }
    }
}