using System.Collections.Generic;
using UnityEngine;

namespace Creatures.NPCs.Pathfinding
{
    public class Node : IHeapItem
    {
        public Vector2 worldPosition;
        public bool walkable;
        public Vector2Int coords;

        public int gCost; // the smallest known distance from start node
        public int hCost; // estimated distance to target node
        public int fCost // sum of gCost and hCost
        {
            get { return gCost + hCost; }
        }
        public Node parent;

        public float distanceToClosestUnwalkableNode;

        private int heapIndex;
        public int HeapIndex
        {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }

        public Node(Vector2 _worldPosition, bool _walkable, Vector2Int _coords)
        {
            worldPosition = _worldPosition;
            walkable = _walkable;
            coords = _coords;
            gCost = 0;
            hCost = 0;
            parent = null;
            distanceToClosestUnwalkableNode = float.PositiveInfinity;
        }
    }
}
