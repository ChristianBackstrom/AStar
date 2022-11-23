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


	private void Awake()
	{
		StartCoroutine(UpdatePath());
	}
	private void Update()
	{
		if (!follow) return;
		if (grid.finalPath == null) return;

		if (index > 0)
			UpdatePosition();
	}

	IEnumerator UpdatePath()
	{
		while (true)
		{
			yield return new WaitForSeconds(1);
			if (!follow) continue;
			if (!grid.finalPath.Equals(path))
			{
				path = grid.finalPath;
				index = path.Count - 1;
			}
		}
	}


	private void UpdatePosition()
	{
		if (transform.position == path[index].worldPosition)
		{
			index--;
		}

		transform.position = Vector3.Lerp(transform.position, path[index].worldPosition, Time.deltaTime * speed);
	}

}
