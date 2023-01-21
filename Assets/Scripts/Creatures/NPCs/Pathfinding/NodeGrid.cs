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
        private Vector2Int gridSize;
        public Vector2Int Size
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
        [SerializeField] CharacterController seekerCC; // TEST
        private List<Node> path; // TEST
        public enum GridDisplayMode
        {
            isWalkable,
            distanceFromClosestUnwalkable,
            ifSeekerCanGoThere
        }

        [SerializeField] private GridDisplayMode gridDisplayMode;


        private void OnDrawGizmos()
        {
            if (grid != null)
            {
                path = pathfinder.FindPath(seeker.position, target.position, seekerCC.radius);
                for (int x = 0; x < gridSize.x; x++)
                {
                    for (int y = 0; y < gridSize.y; y++)
                    {
                        Node node = grid[x, y];

                        //Gizmos.color = node.walkable ? Color.green : Color.red;
                        Gizmos.color = Color.Lerp(Color.red, Color.green, node.distanceToClosestUnwalkableNode/seekerCC.radius);

                        Gizmos.color = gridDisplayMode switch
                        {
                            GridDisplayMode.isWalkable => node.walkable ? Color.green : Color.red,
                            GridDisplayMode.distanceFromClosestUnwalkable => Color.Lerp(Color.red, Color.green, node.distanceToClosestUnwalkableNode / seekerCC.radius),
                            GridDisplayMode.ifSeekerCanGoThere => node.walkable ? (seekerCC.radius <= node.distanceToClosestUnwalkableNode ? Color.green : Color.Lerp(Color.green, Color.red, 0.5f)) : Color.red,
                            GridDisplayMode => Color.black
                        };

                        if (node == WorldPosToNode(seeker.position))
                        {
                            Gizmos.color = Color.cyan;
                        }
                        else if (path != null && path.Contains(node))
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

        public Node WorldPosToNode(Vector3 worldPos)
        {
            int x = Mathf.Clamp(Mathf.FloorToInt((worldPos.x - worldBottomLeft.x + nodeSize/2) / nodeSize), 0, gridSize.x - 1);
            int y = Mathf.Clamp(Mathf.FloorToInt((worldPos.z - worldBottomLeft.y + nodeSize/2) / nodeSize), 0, gridSize.y - 1);

            return grid[x, y];
        }

        public Node WorldPosToClosestWalkableNode(Vector3 worldPos, float seekerRadius)
        {
            return ClosestWalkableNode(WorldPosToNode(worldPos), seekerRadius);
        }

        public Node ClosestWalkableNode(Node node, float seekerRadius)
        {
            Queue<Node> queue = new Queue<Node>();
            HashSet<Node> enqued = new HashSet<Node>();

            queue.Enqueue(node);
            enqued.Add(node);

            return closestWalkableNodeRec(queue, enqued, seekerRadius, node);
        }

        private Node closestWalkableNodeRec(Queue<Node> queue, HashSet<Node> enqued, float seekerRadius, Node start)
        {
            Node node = queue.Dequeue();

            if(node.walkable && node.distanceToClosestUnwalkableNode >= seekerRadius)
            {
                Debug.DrawLine(new Vector3(start.worldPosition.x, 0f, start.worldPosition.y), new Vector3(node.worldPosition.x, 0f, node.worldPosition.y), Color.magenta);
                return node;
            }

            Debug.DrawLine(new Vector3(start.worldPosition.x, 0f, start.worldPosition.y), new Vector3(node.worldPosition.x, 0f, node.worldPosition.y), Color.black);

            foreach (Node neighbour in GetNeighbours(node))
            {
                if (enqued.Contains(neighbour))
                {
                    continue;
                }
                queue.Enqueue(neighbour);
                enqued.Add(node);
            }

            return closestWalkableNodeRec(queue, enqued, seekerRadius, node);
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
            neighbours[0] = inBounds(node.coords + new Vector2Int(-1, 1)) ? grid[node.coords.x - 1, node.coords.y + 1] : null;
            neighbours[1] = inBounds(node.coords + new Vector2Int(0, 1)) ? grid[node.coords.x, node.coords.y + 1] : null;
            neighbours[2] = inBounds(node.coords + new Vector2Int(1, 1)) ? grid[node.coords.x + 1, node.coords.y + 1] : null;
            neighbours[3] = inBounds(node.coords + new Vector2Int(1, 0)) ? grid[node.coords.x + 1, node.coords.y] : null;
            neighbours[4] = inBounds(node.coords + new Vector2Int(1, -1)) ? grid[node.coords.x + 1, node.coords.y - 1] : null;
            neighbours[5] = inBounds(node.coords + new Vector2Int(0, -1)) ? grid[node.coords.x, node.coords.y - 1] : null;
            neighbours[6] = inBounds(node.coords + new Vector2Int(-1, -1)) ? grid[node.coords.x - 1, node.coords.y - 1] : null;
            neighbours[7] = inBounds(node.coords + new Vector2Int(-1, 0)) ? grid[node.coords.x - 1, node.coords.y] : null;

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

        private bool inBounds(Vector2Int coords)
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

                    grid[x, y] = new Node(worldPos2D, walkable, new Vector2Int(x, y));
                }
            }

            foreach(Node n1 in grid) {
                n1.distanceToClosestUnwalkableNode = int.MaxValue;
                foreach(Node n2 in grid)
                {
                    if(!n2.walkable)
                    {
                        float newPossibleDistance = nodesTouchRadius(n1, n2);
                        if(newPossibleDistance < n1.distanceToClosestUnwalkableNode)
                        {
                            n1.distanceToClosestUnwalkableNode = newPossibleDistance;
                        }
                    }
                }
            }
        }

        private float nodesTouchRadius(Node a, Node b) // distance from the center of node A to the closest point or edge of B (or its center, if the nodes are the same)
        {
            int coordDeltaX = a.coords.x - b.coords.x;
            int coordDeltaY = a.coords.y - b.coords.y;

            Vector2 closestPoint = new Vector2(
                b.worldPosition.x + (nodeSize / 2) * Mathf.Clamp(coordDeltaX, -1, 1),
                b.worldPosition.y + (nodeSize / 2) * Mathf.Clamp(coordDeltaY, -1, 1)
                );

            return Vector2.Distance(a.worldPosition, closestPoint);
        }
    }
}
