using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

	LineRenderer lineRenderer = null;

	EdgeCollider2D edgeCollider = null;

	public Material watermaterial = null;

	int vertexCount = 100;

	// Use this for initialization
	void Start () {
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.SetVertexCount(vertexCount);
		lineRenderer.material = watermaterial;

		edgeCollider = gameObject.AddComponent<EdgeCollider2D> ();
		//edgeCollider.pointCount = vertexCount;
	}

	// Update is called once per frame
	void Update () {
	

		Vector2[] positionsArray = new Vector2[vertexCount];

		for (int i = 0; i < vertexCount; i++) {
			float xpos = 38f / vertexCount * i;
			float ypos = Mathf.Sin (xpos/2 + Time.time) * 1.5f + 10;

			lineRenderer.SetPosition (i, new Vector3(xpos, ypos, 1) + transform.position);

			positionsArray [i] = new Vector2 (xpos, ypos);
		}

		edgeCollider.points = positionsArray;
	}


}
