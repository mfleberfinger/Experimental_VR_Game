using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshUtilities
{
	// Array of points on a circle centered at the origin. This will be translated
	//	and rotated to decide where to generate vertices for circles.
	static private Vector3[] m_originalCircle;
	static private float m_originalCircleRadius;

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
	public static Vector3[] DefineCirclePoints(int numPoints, float radius)
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
	/// <param name="vertexCount">Number of vertices to use in the circle.</param>
	/// <param name="radius">Radius of the circle.</param>
	public static void DefineCircleArrays(Vector3 center, Vector3 facing, int vertexCount,
		float radius, out Vector3[] vertices, out Vector3[] normals)
	{
		// Since this works by translating and rotating a circle of vertices,
		// with a predefined position and rotation, we will cache such a circle
		// and only regenerate it if the next circle will differ.
		if (m_originalCircle == null || m_originalCircle.Length != vertexCount
			|| m_originalCircleRadius != radius)
		{
			m_originalCircle = DefineCirclePoints(vertexCount, radius);
			m_originalCircleRadius = radius;
		}
		
		// Rotate and translate the original circle to get vertices.
		vertices = TranslatePoints(m_originalCircle, Vector3.zero, center);
		vertices = RotatePoints(vertices, Vector3.forward, facing);
		
		// Make normals point to the center.
		normals = new Vector3[m_originalCircle.Length];
		for (int i = 0; i < m_originalCircle.Length; i++)
			normals[i] = vertices[i] - center;

	}

	/// <summary>
	/// Given two rings of vertices, return the arrays needed to create a
	/// cylinder capped by the two rings (the cylinder can have sloped or
	/// variable-radius ends).
	/// </summary>
	/// <param name="c0Vertices">First circle. Must have the same number of vertices
	/// as circle1.</param>
	/// <param name="c1Vertices">Second circle. Must have the same number of vertices
	/// as circle0.</param>
	/// <param name="c0Normals">First circle's normals.</param>
	/// <param name="c1Normals">Second circle's normals.</param>
	/// <param name="cylinderVertices">Combined "vertices" arrays of the two circles.</param>
	/// <param name="cylinderNormals">Combined "normals" arrays of the two circles.</param>
	/// <param name="cylinderTriangles">The Cylinder's triangles.</param>
	/// <param name="uv">The cylinder's UVs.</param>
	/// <returns></returns>
	public static void DefineCylinderArrays(Vector3[] c0Vertices, Vector3[] c1Vertices,
		Vector3[] c0Normals, Vector3[] c1Normals, out Vector3[] cylinderVertices,
		out Vector3[] cylinderNormals, out int[] cylinderTriangles, out Vector2[] uv)
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
			cylinderTriangles[0 + 6 * i] = i;
			cylinderTriangles[1 + 6 * i] = i + lenC;
			cylinderTriangles[2 + 6 * i] = i + 1;
			// Bottom right triangle.
			cylinderTriangles[3 + 6 * i] = i + 1;
			cylinderTriangles[4 + 6 * i] = i + lenC;
			cylinderTriangles[5 + 6 * i] = i + lenC +1;
		}
		// Define the last two triangles (where the last quad mates with the first).
		// Top left triangle.
		cylinderTriangles[6 * lenC - 6] = lenC - 1;
		cylinderTriangles[6 * lenC - 5] = 2 * lenC - 1;
		cylinderTriangles[6 * lenC - 4] = 0;
		// Bottom right triangle.
		cylinderTriangles[6 * lenC - 3] = 2 * lenC - 1;
		cylinderTriangles[6 * lenC - 2] = lenC;
		cylinderTriangles[6 * lenC - 1] = 0;
		

		// TODO: Define cylinder UVs.
		// Define the UVs.
		uv = new Vector2[cylinderVertices.Length];
	}

	/// <summary>
	/// Given all of the arrays needed to define a mesh, create and return that
	/// mesh.
	/// </summary>
	/// <param name="vertices">The vertices.</param>
	/// <param name="triangles">The triangles.</param>
	/// <param name="normals">The normals.</param>
	/// <param name="uv">The UVs.</param>
	/// <returns></returns>
	public static Mesh BuildMesh(Vector3[] vertices, int[] triangles,
		Vector3[] normals, Vector2[] uv)
		{
			Mesh mesh = new Mesh();
			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.normals = normals;
			mesh.uv = uv;
			return mesh;
		}
}
