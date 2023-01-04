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
        private readonly float moveSpeed;
        private readonly CharacterController cc;
        private readonly float attackDistance;

        private Vector3[] waypoints = null;
        private int nextWaypointIndex = 0;
        private Vector3 currentWaypointAdjY;

        public PursueState(Enemy _me, Creature _target, Animator _animator, float _pursueDistance, float _attackDistance, float _moveSpeed, CharacterController _cc) : base(_me, _target, _animator)
        {
            pursueDistance = _pursueDistance;
            moveSpeed = _moveSpeed;
            cc = _cc;
            attackDistance = _attackDistance;

            currentWaypointAdjY = me.transform.position;
        }
    
        public override void OnEnter()
        {
            // Debug.Log("Starting to persue " + target.name);
            animator.SetInteger("State", 1);
        }

        public override void OnUpdate()
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
                PathRequestManager.RequestPath(me.transform.position, target.transform.position, (_waypoints, success) =>
                {
                    if (success)
                    {
                        waypoints = _waypoints;
                        nextWaypointIndex = 0;
                        currentWaypointAdjY = me.transform.position;
                    }
                });
            //}

            float distanceToTargetSquared = MyMath.SquareOf(currentWaypointAdjY.x - me.transform.position.x) +
                                            MyMath.SquareOf(currentWaypointAdjY.y - me.transform.position.y) +
                                            MyMath.SquareOf(currentWaypointAdjY.z - me.transform.position.z);
            bool currentWaypointChanged = false;

            if (waypoints != null && nextWaypointIndex < waypoints.Length)
            {
                if (distanceToTargetSquared <= MyMath.SquareOf(0.1f))
                {
                    currentWaypointAdjY = waypoints[nextWaypointIndex] + Vector3.up * me.transform.position.y;
                    nextWaypointIndex++;
                    currentWaypointChanged = true;

                    Debug.DrawLine(me.transform.position, currentWaypointAdjY, Color.black, 3f);
                    Debug.Log(currentWaypointAdjY);
                }
            }

            me.transform.LookAt(currentWaypointAdjY);
            if (distanceToTargetSquared > MyMath.SquareOf(0.1f) || currentWaypointChanged)
            {
                cc.Move((currentWaypointAdjY - me.transform.position).normalized * moveSpeed * Time.deltaTime);
            }

            //me.transform.LookAt(target.transform.position);
            //cc.Move((target.transform.position - me.transform.position).normalized * moveSpeed * Time.deltaTime);
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
        private readonly float moveSpeed;
        private readonly CharacterController cc;
        private readonly Spell spell;
        private readonly float spellDelay;
        private readonly CastStats castStats;
        
        private float lastCastTime = 0f;

        //private Vector3[] waypoints = null;
        //private int currentWaypointIndex = 0;
        //private Vector3 currentWaypointAdjY;

        public AttackState(Enemy _me, Creature _target, Animator _animator, float _attackDistance, float _moveSpeed, CharacterController _cc, Spell _spell, float _spellDelay, CastStats _castStats) : base(_me, _target, _animator)
        {
            me = _me;
            target = _target;
            attackDistance = _attackDistance;
            moveSpeed = _moveSpeed;
            cc = _cc;
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
            //PathRequestManager.RequestPath(me.transform.position, target.transform.position, (_waypoints, success) =>
            //{
            //    if (success)
            //    {
            //        waypoints = _waypoints;
            //        currentWaypointIndex = 0;
            //    }
            //});

            //if (currentWaypointIndex < waypoints.Length)
            //{
            //    currentWaypointAdjY = waypoints[currentWaypointIndex] + Vector3.up * me.transform.position.y;
            //    currentWaypointIndex++;
            //}
            //else
            //{
            //    currentWaypointAdjY = me.transform.position;
            //}

            //me.transform.LookAt(currentWaypointAdjY);
            //cc.Move((currentWaypointAdjY - me.transform.position).normalized * moveSpeed * Time.deltaTime);

            //Debug.DrawLine(me.transform.position, currentWaypointAdjY);

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