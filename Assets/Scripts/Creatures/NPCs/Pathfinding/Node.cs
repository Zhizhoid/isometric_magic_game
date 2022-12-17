using UnityEngine;

namespace Creatures.NPCs.Pathfinding
{
    public class Node
    {
        public Vector2 worldPosition;
        public bool walkable;

        public Node(Vector2 _worldPosition, bool _walkable)
        {
            worldPosition = _worldPosition;
            walkable = _walkable;
        }
    }
}
