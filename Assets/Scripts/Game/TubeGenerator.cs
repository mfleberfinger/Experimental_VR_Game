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

	// Array of points on a circle centered at the origin. This will be translated
	//	and rotated to decide where to generate vertices.
	private Vector3[] m_originalCircle;

	// Starting position of the next segment to be generated.
	private Vector3 m_startOfSegment;
	private Queue<GameObject> m_segments;

	private void Start()
	{
		m_startOfSegment = transform.position;
		m_segments = new Queue<GameObject>();
	}

	private void Update()
	{

	}


	/// <summary>
	/// Define a ring of vertices in a given plane, with the normals facing
	/// toward the center of the ring.
	/// </summary>
	/// <remarks>
	/// The UVs are not defined by this function because they will vary based on
	/// where the ring is used in the final mesh. The coordinate for the center
	/// will be in the mesh/gameobject's local space (because this is how Unity's
	/// meshes work).
	/// </remarks>
	/// <param name="center">Center of the ring/circle.</param>
	/// <param name="facing">Vector giving the direction in which the circle
	/// will face.</param>
	/// <param name="vertices">Array to store the calculated vertex positions.</param>
	/// <param name="normals">Array to store the calculated normals.</param>
	private void DefineVertexRing(Vector3 center, Vector3 facing,
		out Vector3[] vertices, out Vector3[] normals)
	{
		if (m_originalCircle.Length == 0)
			m_originalCircle = DefineCircle(ringVertexCount, radius);
		
		// Rotate and translate the original circle to get vertices.
		vertices = TranslatePoints(m_originalCircle, Vector3.zero, center);
		vertices = RotatePoints(vertices, Vector3.forward, facing);
		
		// Make normals point to the center.
		normals = new Vector3[m_originalCircle.Length];
		for (int i = 0; i < m_originalCircle.Length; i++)
			normals[i] = vertices[i] - center;

	}

	/// <summary>
	/// Given two rings of vertices, define the triangles needed to create a
	/// cylinder capped by the two rings (the cylinder can have sloped or
	/// variable-radius ends).
	/// </summary>
	/// <param name="c0Vertices">First circle. Must have the same number of vertices
	/// as circle1.</param>
	/// <param name="c1Vertices">Second circle. Must have the same number of vertices
	/// as circle0.</param>
	/// <param name="c0Normals">First circle's normals.</param>
	/// <param name="c1Normals">Second circle's normals.</param>
	/// <param name="cylinderVertices">Combined vertices arrays of the two circles.</param>
	/// <param name="cylinderNormals">Combined normals arrays of the two circles.</param>
	/// <param name="cylinderTriangles">The Cylinder's triangles.</param>
	/// <param name="uvs">The cylinder's UVs.</param>
	/// <returns></returns>
	private void DefineCylinder(Vector3[] c0Vertices, Vector3[] c1Vertices,
		Vector3[] c0Normals, Vector3[] c1Normals, out Vector3[] cylinderVertices,
		out Vector3[] cylinderNormals, out int[] cylinderTriangles, out Vector2[] uvs)
	{
		if (c0Vertices.Length != c1Vertices.Length)
			Debug.LogError("A cylinder requires each end cap to have the same" +
				" number of vertices.");
		
		// Length of circle vertex arrays.
		int lenC = c0Vertices.Length;

		// Copy the vertices and normals to the cylindar.
		cylinderVertices = new Vector3[lenC * 2];
		cylinderNormals = new Vector3[lenC * 2];
		c0Vertices.CopyTo(cylinderVertices, 0);
		c1Vertices.CopyTo(cylinderVertices, lenC);
		c0Normals.CopyTo(cylinderNormals, 0);
		c1Normals.CopyTo(cylinderNormals, lenC);

		// Define the triangles.
		cylinderTriangles = new int[lenC * 6];
		// Define most of the triangles. Keep in mind that a cylinder is a bunch
		// of rectangles stuck together (think of a wooden barrel).
		for (int i = 0; i < lenC; i++)
		{
			// TODO: Make sure the triangles are defined in the right order.
			// The vertices for each triangle must be numbered in the clockwise
			// direction when viewed, otherwise the face normals will point in
			// the wrong direction (away from the camera).
			
			// Top left triangle.
			cylinderTriangles[i + 6 * i] = i;
			cylinderTriangles[i + 1 + 6 * i] = i + lenC;
			cylinderTriangles[i + 2 + 6 * i] = i + 1;
			// Bottom right triangle.
			cylinderTriangles[i + 3 + 6 * i] = i + lenC;
			cylinderTriangles[i + 4 + 6 * i] = i + lenC + 1;
			cylinderTriangles[i + 5 + 6 * i] = i + 1;
		}
		// Define the last two triangles.
		// Top left triangle.
		cylinderTriangles[lenC + 6 * lenC] = lenC - 1;
		cylinderTriangles[lenC + 1 + 6 * lenC] = 2 * lenC - 1;
		cylinderTriangles[lenC + 2 + 6 * lenC] = 0;
		// Bottom right triangle.
		cylinderTriangles[lenC + 3 + 6 * lenC] = 2 * lenC - 1;
		cylinderTriangles[lenC + 4 + 6 * lenC] = lenC;
		cylinderTriangles[lenC + 5 + 6 * lenC] = 0;
		

		// TODO: Define cylinder UVs.
		// Define the UVs.
		uvs = new Vector2[cylinderVertices.Length];

	}

	/// <summary>
	/// Translate a cloud of points to a new position in 3D space.
	/// </summary>
	/// <param name="points">The current positions of all points to translate.</param>
	/// <param name="oldPosition">The orginal center position of the points.</param>
	/// <param name="newPosition">The new center position for the points.</param>
	/// <returns>The translated points.</returns>
	public static Vector3[] TranslatePoints(Vector3[] points, Vector3 oldPosition,
		Vector3 newPosition)
	{
		Vector3[] translatedPoints = new Vector3[points.Length];
		Vector3 translationVector = newPosition - oldPosition;

		for (int i = 0; i < points.Length; i++)
			translatedPoints[i] = points[i] + translationVector;

		return translatedPoints;
	}

	/// <summary>
	/// Rotate a cloud of points.
	/// </summary>
	/// <param name="points">The current positions of all points to rotate.</param>
	/// <param name="oldDirection">A vector giving the current "forward" direction
	/// of the point cloud (this is an arbitrary choice).</param>
	/// <param name="newDirection">A vector representing the new direction for
	/// the points to face. The cloud will rotate such that its "forward"
	/// facing side will now point in this direction.</param>
	/// <returns></returns>
	public static Vector3[] RotatePoints(Vector3[] points, Vector3 oldDirection,
		Vector3 newDirection)
	{
		Vector3[] rotatedPoints = new Vector3[points.Length];
		Quaternion rotation = new Quaternion();

		rotation.SetFromToRotation(oldDirection, newDirection);
		for (int i = 0; i < points.Length; i++)
			rotatedPoints[i] = rotation * points[i];

		return rotatedPoints;
	}

	/// <summary>
	/// Generate an array of points, evenly spaced around a circle centered on
	/// the origin in the x-y plane.
	/// </summary>
	/// <param name="numPoints">Number of points to generate.</param>
	/// <param name="radius">Radius of the circle.</param>
	/// <returns>An array of points on the defined circle.</returns>
	public static Vector3[] DefineCircle(int numPoints, float radius)
	{
		// Angle between any two of the points.
		float angularSpacing = 0;
		float x = 0, y = 0, theta = 0;
		Vector3[] circle = new Vector3[numPoints];

		if(numPoints < 1 || radius <= 0)
			Debug.LogError("A circle must have a positive radius and more than" +
				" one point.");

		angularSpacing = 2 * Mathf.PI / numPoints;
		for (int i = 0; i < numPoints; i++)
		{
			theta = i * angularSpacing;
			x = radius * Mathf.Cos(theta);
			y = radius * Mathf.Sin(theta);
			circle[i] = new Vector3(x, y, 0);
		}

		return circle;
	}
}
