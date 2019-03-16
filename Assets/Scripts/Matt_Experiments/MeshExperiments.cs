using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://docs.unity3d.com/Manual/GeneratingMeshGeometryProcedurally.html
public class MeshExperiments : MonoBehaviour
{
	public MeshFilter meshFilter;

	private void Start()
	{
		CreateQuad(1f, 1f);
	}

	/// <summary>
	/// Turns the meshFilter's mesh into a quad.
	/// </summary>
	private void CreateQuad(float height, float width)
	{
		Vector3[] vertices;
		int[] triangles;
		Vector3[] normals;
		Vector2[] uv;

		// Delete the original mesh data.
		meshFilter.mesh.Clear();

		// Set up the vertices.
		vertices = new Vector3[4];

		// bottom left
		vertices[0] = new Vector3(0, 0, 0);
		// bottom right
		vertices[1] = new Vector3(width, 0, 0);
		// top left
		vertices[2] = new Vector3(0, height, 0);
		// top right
		vertices[3] = new Vector3(width, height, 0);


		// Since the Mesh data properties execute code behind the scenes, it is
		// much more efficient to set up the data in your own array and then
		// assign this to a property rather than access the property array
		// element by element.
		meshFilter.mesh.vertices = vertices;

		// Set up the triangles.
		triangles = new int[6];

		// Define the lower left triangle by vertices 0, 2, and 1.
		// From the documentation:
		//		"An important detail of the triangles is the ordering of the corner
		//		vertices. They should be arranged so that the corners go around
		//		clockwise as you look down on the visible outer surface of the
		//		triangle, although it doesn’t matter which corner you start with."
		triangles[0] = 0;
		triangles[1] = 2;
		triangles[2] = 1;

		// Define the upper right triangle.
		triangles[3] = 2;
		triangles[4] = 3;
		triangles[5] = 1;

		meshFilter.mesh.triangles = triangles;


		// Orient the normals of the vertices so that the quad is rendered correctly.
		// In this case they all face in the same direction (towards the front
		// of the quad).
		normals = new Vector3[4];

		normals[0] = -Vector3.forward;
		normals[1] = -Vector3.forward;
		normals[2] = -Vector3.forward;
		normals[3] = -Vector3.forward;

		meshFilter.mesh.normals = normals;

		// Set up the texture coordinates (UVs) to display the entire image
		// on the surface of the quad. UVs are normalized between 0 and 1.
		uv = new Vector2[4];

		uv[0] = new Vector2(0, 0);
		uv[1] = new Vector2(1, 0);
		uv[2] = new Vector2(0, 1);
		uv[3] = new Vector2(1, 1);

		meshFilter.mesh.uv = uv;
	}
}
