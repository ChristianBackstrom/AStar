using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
	public bool isWalkable;

	public Vector3 worldPosition;

	public int gridX;
	public int gridY;

	public int gCost;
	public int hCost;

	public Node parent;

	public int fCost
	{
		get
		{
			return gCost + hCost;
		}
	}

	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
	{
		isWalkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	}
}
