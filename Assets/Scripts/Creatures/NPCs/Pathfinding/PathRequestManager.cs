using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Creatures.NPCs.Pathfinding
{
    public class PathRequestManager : MonoBehaviour
    {
        private static PathRequestManager instance;

        private Pathfinder pathfinder;

        private void Awake()
        {
            pathfinder = GetComponent<Pathfinder>();
            if (instance == null)
            {
                instance = this;
            }
        }

        public static void RequestPath(Vector3 from, Vector3 to, float seekerRadius, Action<Vector3[], bool> callback)
        {
            Vector3[] waypoints = instance.pathfinder.GetWaypoints(from, to, seekerRadius);
            bool success = waypoints != null;
            callback(waypoints, success);
        }

        private class PathRequest
        {
            public Vector3 start;
            public Vector3 target;
            public Action<Vector3[], bool> callback;

            public PathRequest(Vector3 _start, Vector3 _target, Action<Vector3[], bool> _callback)
            {
                start = _start;
                target = _target;
                callback = _callback;
            }
        }
    }
}
