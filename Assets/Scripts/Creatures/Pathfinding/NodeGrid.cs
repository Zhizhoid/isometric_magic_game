using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Pathfinding {
    public class NodeGrid : MonoBehaviour
    {
        [SerializeField] private LayerMask unwalkable;
        [SerializeField] private Vector2 gridWorldSize;
        [SerializeField][Range(0.1f, 1f)] private float nodeSize;

        private Node[,] grid;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1f, gridWorldSize.y));

            if(grid != null)
            {
                foreach(Node node in grid)
                {
                    Gizmos.color = node.walkable ? Color.green : Color.red;
                    Gizmos.DrawCube(new Vector3(node.worldPosition.x, transform.position.y, node.worldPosition.y), Vector3.one * nodeSize);
                }
            }
        }

        private void Start()
        {
            createGrid();
        }

        private void createGrid()
        {
            int gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeSize);
            int gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeSize);

            grid = new Node[gridSizeX, gridSizeY];

            Vector2 worldBottomLeft = new Vector2(transform.position.x, transform.position.z) - gridWorldSize/2;

            for(int x = 0; x < gridSizeX; x++)
            {
                for(int y = 0; y < gridSizeY; y++)
                {
                    Vector2 worldPos = worldBottomLeft + new Vector2(x * nodeSize, y * nodeSize);

                    grid[x, y].worldPosition = worldPos;
                    grid[x, y].walkable = !Physics.CheckBox(
                        new Vector3(worldPos.x, transform.position.y, worldPos.y),
                        Vector3.one * (nodeSize / 2),
                        Quaternion.identity,
                        unwalkable
                    );
                }
            }
        }
    }
}
