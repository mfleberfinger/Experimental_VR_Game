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
	[Tooltip("Number of segments to create before beginning to delete segments.")]
	public int segmentMax = 10;
	[Tooltip("How many vertices to use for each circle when generating a mesh" +
		" segment.")]
	public int ringVertexCount = 12;
	[Tooltip("Radius of the tube.")]
	public float radius = 1f;
	[Tooltip("Material to apply to the tube's mesh.")]
	public Material tubeMaterial;

	// Starting position of the next segment to be generated.
	private Vector3 m_startOfSegment;
	// Direction in which the previous segment's end was facing (desecibes the
	// plane in which the circular cross section exists).
	private Vector3 m_previousFacing;
	// All active segments.
	private Queue<GameObject> m_segments;

	private void Start()
	{
		m_startOfSegment = transform.position;
		m_previousFacing = transform.forward;
		m_segments = new Queue<GameObject>();
	}

	private void Update()
	{
		if (Vector3.Distance(transform.position, m_startOfSegment) > segmentLength)
			PlaceSegment();
		if (m_segments.Count > segmentMax)
			Destroy(m_segments.Dequeue());
	}

	/// <summary>
	/// Create a segment of the tube, starting at m_startOfSegment and ending at
	/// the tube generator's current position. Enqueue the new segment. Set
	/// m_startOfSegment to the current position of the tube generator
	/// </summary>
	private void PlaceSegment()
	{
		Vector3[] verticesC0;
		Vector3[] verticesC1;
		Vector3[] normalsC0;
		Vector3[] normalsC1;
		Vector2[] uv;
		Vector3[] vertices;
		Vector3[] normals;
		Vector3 localPreviousFacing;
		Vector3 localFacing;
		Vector3 c0Center;
		Vector3 c1Center;
		int[] triangles;
		GameObject GO = new GameObject();
		MeshFilter MF = GO.AddComponent<MeshFilter>();
		MeshRenderer renderer = GO.AddComponent<MeshRenderer>();

		// Create and position the segment.
		GO.transform.position = m_startOfSegment;
		//GO.transform.LookAt(transform.position);
		// The mesh must be defined using the local coordinates of its parent gameObject.
		localPreviousFacing = GO.transform.InverseTransformDirection(m_previousFacing);
		localFacing = GO.transform.InverseTransformDirection(transform.forward);
		c0Center = GO.transform.InverseTransformPoint(m_startOfSegment);
		c1Center = GO.transform.InverseTransformPoint(transform.position);
		MeshUtilities.DefineCircleArrays(c0Center, localPreviousFacing, ringVertexCount,
			radius, out verticesC0, out normalsC0);
		MeshUtilities.DefineCircleArrays(c1Center, localFacing, ringVertexCount,
			radius, out verticesC1, out normalsC1);
		MeshUtilities.DefineCylinderArrays(verticesC0, verticesC1, normalsC0,
			normalsC1, out vertices, out normals, out triangles, out uv);
		MF.mesh = MeshUtilities.BuildMesh(vertices, triangles, normals, uv);
		renderer.material = tubeMaterial;
		// Enqueue segment and log the new facing and startOfSegment.
		m_segments.Enqueue(GO);
		m_startOfSegment = transform.position;
		m_previousFacing = transform.forward;
	}
}
