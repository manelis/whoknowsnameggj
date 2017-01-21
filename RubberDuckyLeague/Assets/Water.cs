using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

	LineRenderer lineRenderer = null;

	EdgeCollider2D edgeCollider = null;

	public Material watermaterial = null;

	int vertexCount = 250;
	int vertical_offset = 7;

	//Our meshes and colliders
	GameObject[] meshobjects;
	Mesh[] meshes;
	public GameObject watermesh;

	// Use this for initialization
	void Start () {
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.SetVertexCount(vertexCount);
		lineRenderer.material = watermaterial;
		lineRenderer.SetWidth(1.0f, 1.0f);
		 
		edgeCollider = gameObject.AddComponent<EdgeCollider2D> ();

		//edgeCollider.pointCount = vertexCount;
		CreateMeshes();
	}

	public void CreateMeshes()
	{
		

		//Declare our mesh arrays
		meshobjects = new GameObject[vertexCount];
		meshes = new Mesh[vertexCount];


		//Setting the meshes now:
		for (int i = 0; i < vertexCount; i++)
		{
			//Make the mesh
			meshes[i] = new Mesh();

			//Create the corners of the mesh
			float xpos1 = 38f / vertexCount * i;
			float ypos1 = Mathf.Sin (xpos1/2 + Time.time) * Mathf.Sin(Time.time) * 1.5f + vertical_offset;

			float xpos2 = 38f / vertexCount * (i+1);
			float ypos2 = Mathf.Sin (xpos2/2 + Time.time) * Mathf.Sin(Time.time) * 1.5f + vertical_offset;


			Vector3[] Vertices = new Vector3[4];
			Vertices[0] = new Vector3(xpos1, ypos1, 0);
			Vertices[1] = new Vector3(xpos2, ypos2, 0);
			Vertices[2] = new Vector3(xpos1, -50, 0);
			Vertices[3] = new Vector3(xpos2, -50, 0);

			//Set the UVs of the texture
			Vector2[] UVs = new Vector2[4];
			UVs[0] = new Vector2(0, 1);
			UVs[1] = new Vector2(1, 1);
			UVs[2] = new Vector2(0, 0);
			UVs[3] = new Vector2(1, 0);

			//Set where the triangles should be.
			int[] tris = new int[6] { 0, 1, 3, 3, 2, 0};

			//Add all this data to the mesh.
			meshes[i].vertices = Vertices;
			meshes[i].uv = UVs;
			meshes[i].triangles = tris;

			//Create a holder for the mesh, set it to be the manager's child
			meshobjects[i] = Instantiate(watermesh,Vector3.zero,Quaternion.identity) as GameObject;
			meshobjects[i].GetComponent<MeshFilter>().mesh = meshes[i];
			meshobjects[i].transform.parent = transform;



		}
	}

	//Same as the code from in the meshes before, set the new mesh positions
	void UpdateMeshes()
	{
		for (int i = 0; i < meshes.Length; i++)
		{

			float xpos1 = 38f / vertexCount * i;
			float ypos1 = Mathf.Sin (xpos1/2 + Time.time) * Mathf.Sin(Time.time) * 1.5f + vertical_offset;

			float xpos2 = 38f / vertexCount * (i+1);
			float ypos2 = Mathf.Sin (xpos2/2 + Time.time) * Mathf.Sin(Time.time) * 1.5f + vertical_offset;

			Vector3[] Vertices = new Vector3[4];
			Vertices[0] = new Vector3(xpos1, ypos1, 0);
			Vertices[1] = new Vector3(xpos2, ypos2, 0);
			Vertices[2] = new Vector3(xpos1, -50, 0);
			Vertices[3] = new Vector3(xpos2, -50, 0);

			meshes[i].vertices = Vertices;
		}
	}


	// Update is called once per frame
	void Update () {
	

		Vector2[] positionsArray = new Vector2[vertexCount];

		for (int i = 0; i < vertexCount; i++) {
			float xpos = 38f / vertexCount * i;
			float ypos = Mathf.Sin (xpos/2 + Time.time) * Mathf.Sin(Time.time) * 1.5f + vertical_offset;

			lineRenderer.SetPosition (i, new Vector3(xpos, ypos, 1) + transform.position);

			positionsArray [i] = new Vector2 (xpos, ypos);
		}

		edgeCollider.points = positionsArray;
		UpdateMeshes ();
	}


}
