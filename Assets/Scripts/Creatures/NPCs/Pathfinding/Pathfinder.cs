using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Creatures.NPCs.Pathfinding
{
    [RequireComponent(typeof(NodeGrid))]
    public class Pathfinder : MonoBehaviour
    {
        private NodeGrid grid;

        public void Awake()
        {
            grid = GetComponent<NodeGrid>();
        }

        public List<Node> FindPath(Vector3 from, Vector3 to)
        {
            Node start = grid.WorldPosToClosestWalkableNode(new Vector2(from.x, from.z));
            Node target = grid.WorldPosToClosestWalkableNode(new Vector2(to.x, to.z));

            //List<Node> openSet = new List<Node>(); // TODO: make it a heap
            Heap<Node>.compareDel compare = (Node a, Node b) =>
            {
                int res = a.fCost.CompareTo(b.fCost);
                if(res == 0)
                {
                    res = a.hCost.CompareTo(b.hCost);
                }

                return -res;
            };
            Heap<Node> openSet = new Heap<Node>(grid.Size.x * grid.Size.y, compare);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(start);

            while(openSet.Count > 0)
            {
                Node current = openSet.Pop();
                closedSet.Add(current);

                if(current == target)
                {
                    return retracePath(start, target);
                }

                foreach(Node neighbour in grid.GetNeighbours(current))
                {
                    if(!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newNeighbourGCost = current.gCost + getDistance(current, neighbour);

                    if (newNeighbourGCost < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newNeighbourGCost;
                        neighbour.hCost = getDistance(neighbour, target);
                        neighbour.parent = current;
                        openSet.Update(neighbour);

                        if(!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }

            return null;
        }

        private int getDistance(Node a, Node b)
        {
            int deltaX = Mathf.Abs(a.coords.x - b.coords.x);
            int deltaY = Mathf.Abs(a.coords.y - b.coords.y);

            if(deltaX >= deltaY)
            {
                return 14 * deltaY + 10 * (deltaX - deltaY);
            }
            else
            {
                return 14 * deltaX + 10 * (deltaY - deltaX);
            }
        }

        private List<Node> retracePath(Node start, Node end)
        {
            List<Node> path = new List<Node>();
            Node current = end;
            while(current != start)
            {
                path.Add(current);
                current = current.parent;
            }
            path.Reverse();

            return path;
        }
    }
}
