using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Grid : MonoBehaviour
{
	[SerializeField] private LayerMask unwalkableMask;
	[SerializeField] private Vector2Int gridWorldSize;
	[SerializeField] private bool avoidObstacles;
	[SerializeField] private float nodeRadius;
	private Node[,] grid = null;

	private float nodeDiameter;

	public List<Node> finalPath;


	private void Awake()
	{
		GenerateGrid();
	}

	private void GenerateGrid()
	{
		Vector3 gridBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 + Vector3.right * nodeRadius - Vector3.forward * gridWorldSize.y / 2 + Vector3.forward * nodeRadius;




		grid = new Node[gridWorldSize.x, gridWorldSize.y];
		nodeDiameter = nodeRadius * 2;

		for (int x = 0; x < gridWorldSize.x; x++)
		{
			for (int y = 0; y < gridWorldSize.y; y++)
			{
				Vector3 worldPoint = gridBottomLeft + new Vector3(x, 0, y) * nodeDiameter;
				bool walkable = avoidObstacles == !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
				grid[x, y] = new Node(walkable, worldPoint, x, y);
			}
		}
	}

	public Node GetNodeFromWorldPosition(Vector3 worldPosition)
	{
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridWorldSize.x - 1) * percentX);
		int y = Mathf.RoundToInt((gridWorldSize.y - 1) * percentY);

		return grid[x, y];
	}

	public List<Node> GetNeighbours(Node node)
	{
		List<Node> neighbours = new List<Node>();
		int gridX = node.gridX;
		int gridY = node.gridY;

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{

				if (x == 0 && y == 0) continue;

				if (gridX + x >= 0 && gridX + x < gridWorldSize.x && gridY + y >= 0 && gridY + y < gridWorldSize.y)
					neighbours.Add(grid[node.gridX + x, node.gridY + y]);
			}
		}

		return neighbours;
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

		if (grid == null) return;

		foreach (Node node in grid)
		{

			Gizmos.color = (node.isWalkable) ? Color.white : Color.red;
			if (finalPath != null)
			{
				if (finalPath.Contains(node)) Gizmos.color = Color.cyan;
			}
			Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - .1f));
		}

		if (finalPath != null)
		{
			Gizmos.color = Color.gray;
		}
	}
}
