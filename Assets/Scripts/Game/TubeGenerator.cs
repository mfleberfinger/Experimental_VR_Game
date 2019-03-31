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

	//TODO: Delete test code.
	Vector3 lastPlacement;

	private void Start()
	{
		m_startOfSegment = transform.position;
		m_segments = new Queue<GameObject>();
		m_originalCircle = DefineCircle(ringVertexCount, radius);
		//TODO: Delete test code.
		lastPlacement = transform.position;
	}

	private void Update()
	{
		//TODO: Delete test code.
		//---------------Test Code----------------
		if (Vector3.Distance(lastPlacement, transform.position) > 0.01f)
		{
			GameObject s;
			Vector3[] vees = RotatePoints(m_originalCircle, Vector3.forward, transform.forward);
			vees = TranslatePoints(vees, Vector3.zero, transform.position);
			foreach(Vector3 v in vees)
			{
				s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				s.transform.localScale = s.transform.localScale * 0.01f;
				s.transform.position = v;
			}
			lastPlacement = transform.position;
		}
		//---------------Test Code----------------
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
	private void CreateVertexRing(int numberOfVertices, float radius, Vector3 center,
		Vector3 plane, out Vector3[] vertices, out Vector3[] normals, out Vector2[] uv)
	{
		vertices = new Vector3[numberOfVertices];
		normals = new Vector3[numberOfVertices];
		uv = new Vector2[numberOfVertices];
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
