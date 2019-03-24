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
	public float radius;

	// Starting position of the next segment to be generated.
	private Vector3 m_startOfSegment;
	private Queue<GameObject> m_segments;

	// Arrays to define a ring of connected vertices to be copied as the ends of
	// each segment.
	private Vector3[] m_ringVertices;
	private Vector3[] m_ringNormals;
	private Vector2[] m_ringUVs;
	
	private void Start()
	{
		m_startOfSegment = transform.position;
		m_segments = new Queue<GameObject>();
	}

	/// <summary>
	/// Create a ring of vertices in the given plane, connected by edges with
	/// the normals facing toward the center of the ring.
	/// </summary>
	/// <param name="numberOfVertices">Number of vertices to use in the ring.</param>
	/// <param name="radius">Radius of the circle on which the vertices will be placed.</param>
	/// <param name="center">Center of the ring/circle.</param>
	/// <param name="plane">A unit vector defining the plane that the circle will
	/// be generated within.</param>
	/// <param name="vertices">Array to store the calculated vertex positions.</param>
	/// <param name="normals">Array to store the calculated normals.</param>
	/// <param name="uv">Array to store the calculated UVs.</param>
	private void DefineRing(int numberOfVertices, float radius, Vector3 center,
		Vector3 plane, out Vector3[] vertices, out Vector3[] normals, out Vector2[] uv)
	{
		vertices = new Vector3[numberOfVertices];
		normals = new Vector3[numberOfVertices];
		uv = new Vector2[numberOfVertices];

		// Divide 360 degrees/2pi radians by numberOfVertices to get angle
		// between vertices.
	}
}
