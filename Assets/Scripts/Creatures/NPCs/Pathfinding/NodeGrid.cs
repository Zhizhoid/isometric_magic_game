using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.NPCs.Pathfinding {
    public class NodeGrid : MonoBehaviour
    {
        [SerializeField] private LayerMask unwalkable;
        [SerializeField] private Vector2 gridWorldSize;
        [SerializeField][Range(0.1f, 1f)] private float nodeSize;

        [SerializeField] Transform player;

        struct Int2 {
            public int x;
            public int y;
            
            public Int2(int _x, int _y)
            {
                x = _x;
                y = _y;
            }
            public static Int2 operator +(Int2 a, Int2 b) => new Int2(a.x + a.x, a.y + a.y);
            public static Int2 operator -(Int2 a, Int2 b) => new Int2(a.x - a.x, a.y - a.y);
            public static bool operator ==(Int2 a, Int2 b) => a.x == b.x && a.y == b.y;
            public static bool operator !=(Int2 a, Int2 b) => a.x != b.x || a.y != b.y;
        }

        private Node[,] grid;
        private Int2 gridSize;
        private Vector2 worldBottomLeft;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1f, gridWorldSize.y));
            
            if(grid != null)
            {
                for(int x = 0; x < gridSize.x; x++)
                {
                    for (int y = 0; y < gridSize.y; y++)
                    {
                        Node node = grid[x, y];

                        Gizmos.color = new Int2(x, y) != worldPosToNodeCoords(new Vector2(player.position.x, player.position.z))
                                                         ? (node.walkable ? Color.green : Color.red)
                                                         : Color.blue;

                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, transform.position.y, node.worldPosition.y),
                                        new Vector3(nodeSize * 0.9f, 0.2f, nodeSize * 0.9f));
                    }
                }
            }
        }

        private void Start()
        {
            createGrid();
        }

        private Int2 worldPosToNodeCoords(Vector2 worldPos)
        {
            Int2 coords;
            coords.x = Mathf.RoundToInt(Mathf.Clamp01((worldPos.x - worldBottomLeft.x) / gridWorldSize.x) * gridSize.x);
            coords.y = Mathf.RoundToInt(Mathf.Clamp01((worldPos.y - worldBottomLeft.y) / gridWorldSize.y) * gridSize.y);

            return coords;
        }

        private void createGrid()
        {
            gridSize.x = Mathf.RoundToInt(gridWorldSize.x / nodeSize);
            gridSize.y = Mathf.RoundToInt(gridWorldSize.y / nodeSize);

            grid = new Node[gridSize.x, gridSize.y];

            worldBottomLeft = new Vector2(transform.position.x, transform.position.z) - gridWorldSize / 2 + Vector2.one * nodeSize / 2;

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    Vector2 worldPos2D = worldBottomLeft + new Vector2(x * nodeSize, y * nodeSize);
                    bool walkable = !Physics.CheckBox(
                        new Vector3(worldPos2D.x, transform.position.y, worldPos2D.y),
                        Vector3.one * (nodeSize / 2),
                        Quaternion.identity,
                        unwalkable
                    );

                    grid[x, y] = new Node(worldPos2D, walkable);
                }
            }
        }
    }
}
