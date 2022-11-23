using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private bool follow;
	[SerializeField] private Grid grid;
	[SerializeField] private float speed;
	private Vector3 nextTarget;

	private List<Node> path = new List<Node>();
	private int index = 0;


	private void Update()
	{
		if (!follow) return;
		if (grid.finalPath == null) return;

		UpdatePath();

		if (index > 0)
			UpdatePosition();
	}

	private void UpdatePath()
	{

		if (!follow) return;
		if (!grid.finalPath.Equals(path))
		{
			path = grid.finalPath;
			path.Reverse();
			index = path.Count - 1;
		}
	}


	private void UpdatePosition()
	{
		if (!follow) return;

		if (path.Count > 0)
		{
			transform.position = Vector3.MoveTowards(transform.position, path[0].worldPosition, speed * Time.deltaTime);

			if (transform.position == path[0].worldPosition)
			{
				path.RemoveAt(0);
			}
		}
	}

}
