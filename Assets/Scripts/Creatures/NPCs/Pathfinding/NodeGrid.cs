using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Creatures.NPCs.Pathfinding
{
    public class NodeGrid : MonoBehaviour
    {
        [SerializeField] private LayerMask unwalkable;
        [SerializeField] private Vector2 gridWorldSize;
        [SerializeField][Range(0.1f, 1f)] private float nodeSize;

        private Node[,] grid;
        private Int2 gridSize;
        public Int2 Size
        {
            get
            {
                return gridSize;
            }
        }

        private Vector2 worldBottomLeft;

        private Pathfinder pathfinder;

        [SerializeField] Transform seeker; // TEST
        [SerializeField] Transform target; // TEST
        private List<Node> path; // TEST

        private void OnDrawGizmos()
        {
            if (grid != null)
            {
                path = pathfinder.FindPath(seeker.position, target.position);
                for (int x = 0; x < gridSize.x; x++)
                {
                    for (int y = 0; y < gridSize.y; y++)
                    {
                        Node node = grid[x, y];

                        Gizmos.color = node.walkable ? Color.green : Color.red;

                        if (path != null && path.Contains(node))
                        {
                            Gizmos.color = Color.yellow;
                        }

                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, transform.position.y, node.worldPosition.y),
                                        new Vector3(nodeSize * 0.9f, 0.2f, nodeSize * 0.9f));
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1f, gridWorldSize.y));
        }

        private void Awake()
        {
            pathfinder = GetComponent<Pathfinder>();
            createGrid();
        }

        public Node WorldPosToClosestWalkableNode(Vector2 worldPos)
        {
            return ClosestWalkableNode(WorldPosToNode(worldPos));
        }

        public Node WorldPosToNode(Vector2 worldPos)
        {
            int x = Mathf.CeilToInt(Mathf.Clamp01((worldPos.x - worldBottomLeft.x) / gridWorldSize.x) * (gridSize.x - 1));
            int y = Mathf.CeilToInt(Mathf.Clamp01((worldPos.y - worldBottomLeft.y) / gridWorldSize.y) * (gridSize.y - 1));

            return grid[x, y];
        }

        public Node ClosestWalkableNode(Node node)
        {
            Queue<Node> queue = new Queue<Node>();
            HashSet<Node> enqued = new HashSet<Node>();

            queue.Enqueue(node);
            enqued.Add(node);

            return closestWalkableNodeRec(queue, enqued);
        }

        private Node closestWalkableNodeRec(Queue<Node> queue, HashSet<Node> enqued)
        {
            Node node = queue.Dequeue();
            if(node.walkable)
            {
                return node;
            }

            foreach(Node neighbour in GetNeighbours(node))
            {
                if (enqued.Contains(neighbour))
                {
                    continue;
                }
                queue.Enqueue(neighbour);
                enqued.Add(node);
            }

            return closestWalkableNodeRec(queue, enqued);
        }


        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    if(x == 0 && y == 0) { continue; }

                    int neighbourX = node.coords.x + x;
                    int neighbourY = node.coords.y + y;

                    if (neighbourX >= 0 && neighbourX < gridSize.x && neighbourY >= 0 && neighbourY < gridSize.y)
                    {
                        neighbours.Add(grid[neighbourX, neighbourY]);
                    }
                }
            }

            return neighbours;
        }

        public List<Node> GetWalkableNeighbours(Node node)
        {
            List<Node> walkableNeighbours = new List<Node>();

            // 0 1 2
            // 7 # 3
            // 6 5 4
            Node[] neighbours = new Node[8];
            neighbours[0] = inBounds(node.coords + new Int2(-1, 1)) ? grid[node.coords.x - 1, node.coords.y + 1] : null;
            neighbours[1] = inBounds(node.coords + new Int2(0, 1)) ? grid[node.coords.x, node.coords.y + 1] : null;
            neighbours[2] = inBounds(node.coords + new Int2(1, 1)) ? grid[node.coords.x + 1, node.coords.y + 1] : null;
            neighbours[3] = inBounds(node.coords + new Int2(1, 0)) ? grid[node.coords.x + 1, node.coords.y] : null;
            neighbours[4] = inBounds(node.coords + new Int2(1, -1)) ? grid[node.coords.x + 1, node.coords.y - 1] : null;
            neighbours[5] = inBounds(node.coords + new Int2(0, -1)) ? grid[node.coords.x, node.coords.y - 1] : null;
            neighbours[6] = inBounds(node.coords + new Int2(-1, -1)) ? grid[node.coords.x - 1, node.coords.y - 1] : null;
            neighbours[7] = inBounds(node.coords + new Int2(-1, 0)) ? grid[node.coords.x - 1, node.coords.y] : null;

            for (int i = 0; i < 8; i++)
            {
                if (neighbours[i] == null) {
                    continue;
                }

                if (i % 2 != 0) // vertical and horizontal
                {
                    if (neighbours[i].walkable)
                    {
                        walkableNeighbours.Add(neighbours[i]);
                    }
                }
                else // diagonal
                {
                    if (neighbours[i].walkable && neighbours[ MyMath.Mod(i + 1, 8) ].walkable && neighbours[ MyMath.Mod(i - 1, 8) ].walkable)
                    {
                        walkableNeighbours.Add(neighbours[i]);
                    }
                }
            }

            return walkableNeighbours;
        }

        private bool inBounds(Int2 coords)
        {
            return coords.x >= 0 && coords.x < gridSize.x && coords.y >= 0 && coords.y < gridSize.y;
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

                    grid[x, y] = new Node(worldPos2D, walkable, new Int2(x, y));
                }
            }
        }
    }
}
