using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// As the object to which the TubeGenerator script is attached moves through space,
/// segments of tube mesh are generated behind the object up to some maximum
/// number. Once the maximum number of segments has been generated, the oldest
/// segments will start being deleted.
/// </summary>
public class TubeGenerator : MonoBehaviour
{
	[Tooltip("Length of a single segment of the tube.")]
	public float segmentLength = 0.1f;
	[Tooltip("How many vertices to use for each circle when generating a mesh" +
		" segment.")]
	public int ringVertexCount = 12;
	[Tooltip("Radius of the tube.")]
	public float radius = 1f;

	// Starting position of the next segment to be generated.
	private Vector3 m_startOfSegment;
	private Queue<GameObject> m_segments;

	private void Start()
	{
		m_startOfSegment = transform.position;
		m_segments = new Queue<GameObject>();

		//-----------------TODO: delete test code-----------------
		Vector3[] verticesC0;
		Vector3[] verticesC1;
		Vector3[] normalsC0;
		Vector3[] normalsC1;
		Vector2[] uv;
		Vector3[] vertices;
		Vector3[] normals;
		int[] triangles;
		GameObject GO = new GameObject();
		MeshFilter MF = GO.AddComponent<MeshFilter>();
		GO.AddComponent<MeshRenderer>();
		
		MeshUtilities.DefineCircleArrays(Vector3.zero, Vector3.up, ringVertexCount,
			radius, out verticesC0, out normalsC0);
		MeshUtilities.DefineCircleArrays(Vector3.forward * segmentLength, Vector3.up, ringVertexCount,
			radius, out verticesC1, out normalsC1);
		MeshUtilities.DefineCylinderArrays(verticesC0, verticesC1, normalsC0,
			normalsC1, out vertices, out normals, out triangles, out uv);
		MF.mesh = MeshUtilities.BuildMesh(vertices, triangles, normals, uv);
		//-----------------TODO: delete test code-----------------
	}
}
