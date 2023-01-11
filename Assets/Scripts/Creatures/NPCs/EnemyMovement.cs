using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures.NPCs.Pathfinding;

namespace Creatures.NPCs
{
    [RequireComponent(typeof(CharacterController))]
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;

        private CharacterController cc;

        private Vector3[] waypoints = null;
        private int nextWaypointIndex = 0;
        private Vector3 currentWaypointAdjY;

        private void Awake()
        {
            cc = GetComponent<CharacterController>();
            currentWaypointAdjY = transform.position;
        }

        public void HandleMovement(Vector3 targetPos)
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            PathRequestManager.RequestPath(transform.position, targetPos, cc.radius, (_waypoints, success) =>
            {
                if (success)
                {
                    waypoints = _waypoints;
                    nextWaypointIndex = 0;
                    currentWaypointAdjY = transform.position;
                }
            });
            //}

            float distanceToTargetSquared = MyMath.SquareOf(currentWaypointAdjY.x - transform.position.x) +
                                            MyMath.SquareOf(currentWaypointAdjY.y - transform.position.y) +
                                            MyMath.SquareOf(currentWaypointAdjY.z - transform.position.z);
            bool currentWaypointChanged = false;

            if (waypoints != null && nextWaypointIndex < waypoints.Length)
            {
                if (distanceToTargetSquared <= MyMath.SquareOf(0.1f))
                {
                    currentWaypointAdjY = waypoints[nextWaypointIndex] + Vector3.up * transform.position.y;
                    nextWaypointIndex++;
                    currentWaypointChanged = true;

                    //Debug.DrawLine(me.transform.position, currentWaypointAdjY, Color.black, 3f);
                    //Debug.Log(currentWaypointAdjY);
                }
            }

            transform.LookAt(currentWaypointAdjY);
            if (distanceToTargetSquared > MyMath.SquareOf(0.1f) || currentWaypointChanged)
            {
                cc.Move((currentWaypointAdjY - transform.position).normalized * moveSpeed * Time.deltaTime);
            }
        }
    }
}
