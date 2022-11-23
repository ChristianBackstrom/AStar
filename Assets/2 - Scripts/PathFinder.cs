using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
	[SerializeField] private Transform player, target;

	private Grid grid;

	private void Awake()
	{
		grid = GetComponent<Grid>();

	}

	private void Update()
	{
		FindPath(player.position, target.position);
	}


	private void FindPath(Vector3 startPosition, Vector3 target)
	{
		List<Node> openNodes = new List<Node>();
		HashSet<Node> closedNodes = new HashSet<Node>();

		Node startNode = grid.GetNodeFromWorldPosition(startPosition);
		Node endNode = grid.GetNodeFromWorldPosition(target);
		openNodes.Add(startNode);

		while (openNodes.Count > 0)
		{
			Node currentNode = openNodes[0];

			openNodes.Remove(currentNode);
			closedNodes.Add(currentNode);

			if (currentNode == endNode)
			{
				grid.finalPath = GetFinalPath(startNode, endNode);
				return;
			}

			foreach (Node neigbour in grid.GetNeighbours(currentNode))
			{
				if (closedNodes.Contains(neigbour) || !neigbour.isWalkable) continue;

				int movementCost = currentNode.gCost + GetDistance(currentNode, neigbour);
				if (!openNodes.Contains(neigbour) || movementCost < neigbour.gCost)
				{
					neigbour.gCost = movementCost;
					neigbour.hCost = GetDistance(neigbour, endNode);
					neigbour.parent = currentNode;

					if (!openNodes.Contains(neigbour)) openNodes.Add(neigbour);
				}

			}
		}
	}

	private List<Node> GetFinalPath(Node start, Node end)
	{
		List<Node> finalPath = new List<Node>();

		Node currentNode = end;

		while (currentNode != start)
		{
			finalPath.Add(currentNode);
			currentNode = currentNode.parent;
		}

		return finalPath;
	}

	private int GetDistance(Node node, Node endNode)
	{
		int dstX = Mathf.Abs(node.gridX - endNode.gridX);
		int dstY = Mathf.Abs(node.gridY - endNode.gridY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}

	private void MovePlayer(Node nextPositon, float speed)
	{
		player.position = Vector3.Lerp(player.position, nextPositon.worldPosition, Time.deltaTime * speed);
	}


	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		if (player)
			Gizmos.DrawCube(player.position, Vector3.one * .9f);

		Gizmos.color = Color.yellow;

		if (target)
			Gizmos.DrawCube(target.position, Vector3.one * .9f);
	}
}
