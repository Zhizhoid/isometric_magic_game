using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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

        public Vector3[] GetWaypoints(Vector3 from, Vector3 to)
        {
            List<Node> path = FindPath(from, to);

            if(path == null)
            {
                return null;
            }
            return pathToWaypoints(path);
        }

        public List<Node> FindPath(Vector3 from, Vector3 to)
        {
            Node start = grid.WorldPosToClosestWalkableNode(new Vector2(from.x, from.z));
            Node target = grid.WorldPosToClosestWalkableNode(new Vector2(to.x, to.z));

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

                //foreach(Node neighbour in grid.GetNeighbours(current))
                //{
                //    if(!neighbour.walkable || closedSet.Contains(neighbour))
                //    {
                //        continue;
                //    }

                //    int newNeighbourGCost = current.gCost + getDistance(current, neighbour);

                //    if (newNeighbourGCost < neighbour.gCost || !openSet.Contains(neighbour))
                //    {
                //        neighbour.gCost = newNeighbourGCost;
                //        neighbour.hCost = getDistance(neighbour, target);
                //        neighbour.parent = current;

                //        if(!openSet.Contains(neighbour))
                //        {
                //            openSet.Add(neighbour);
                //        } else
                //        {
                //            openSet.Update(neighbour);
                //        }
                //    }
                //}
                foreach (Node neighbour in grid.GetWalkableNeighbours(current))
                {
                    if (closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newNeighbourGCost = current.gCost + getDistance(current, neighbour);

                    if (newNeighbourGCost < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newNeighbourGCost;
                        neighbour.hCost = getDistance(neighbour, target);
                        neighbour.parent = current;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.Update(neighbour);
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

        private Vector3[] pathToWaypoints(List<Node> path)
        {
            if(path.Count == 0)
            {
                return new Vector3[0];
            }

            List<Vector3> waypoints = new List<Vector3>();

            Int2 prevDir = new Int2(0, 0);
            Node prevNode = path.First();
            foreach (Node node in path.Skip(1))
            {
                Int2 currDir = node.coords - prevNode.coords;

                if (currDir != prevDir)
                {
                    waypoints.Add( new Vector3(prevNode.worldPosition.x, 0, prevNode.worldPosition.y) );
                }

                prevNode = node;
                prevDir = currDir;
            }

            Vector2 lastNodeWorldPos = path.Last().worldPosition;
            waypoints.Add(new Vector3(lastNodeWorldPos.x, 0f, lastNodeWorldPos.y));

            return waypoints.ToArray();
        }
    }
}
