using UnityEngine;

/// <summary>
/// A simple experiment in procedural mesh generation. This will generate quads
/// in the wake of a moving gameobject.
/// </summary>
public class QuadGenerator : MonoBehaviour
{
	[Tooltip("Distance between generated quads.")]
	public float spacing = 1;
	[Tooltip("Width of the quads.")]
	public float width = 1;
	[Tooltip("Height of the quads.")]
	public float height = 1;
	[Tooltip("Render material to apply to the quads.")]
	public Material meshMaterial;

	// Last location at which a quad was placed.
	private Vector3 lastPlacement;

	// Arrays that define a quad.
	Vector3[] m_vertices;
	int[] m_triangles;
	Vector3[] m_normals;
	Vector2[] m_uv;

	private void Start()
	{
		lastPlacement = transform.position;
		PopulateQuadArrays();
	}

	private void Update()
	{
		if (Vector3.Distance(lastPlacement, transform.position) > spacing)
		{
			PlaceQuad(transform.position);
			lastPlacement = transform.position;
		}
	}
	
	private void PlaceQuad(Vector3 location)
	{
		GameObject gameObject = new GameObject();
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshRenderer.material = meshMaterial;

		meshFilter.mesh.vertices = m_vertices;
		meshFilter.mesh.triangles = m_triangles;
		meshFilter.mesh.normals = m_normals;
		meshFilter.mesh.uv = m_uv;

		gameObject.transform.position = location;
	}

	/// <summary>
	/// Save the information used to make a quad in some array data members.
	/// </summary>
	private void PopulateQuadArrays()
	{
		// Set up the vertices.
		m_vertices = new Vector3[4];
		m_vertices[0] = new Vector3(0, 0, 0);				// bottom left
		m_vertices[1] = new Vector3(width, 0, 0);			// bottom right
		m_vertices[2] = new Vector3(0, height, 0);			// top left
		m_vertices[3] = new Vector3(width, height, 0);		// top right

		// Set up the triangles.
		m_triangles = new int[6];
		// Define the lower left triangle by vertices 0, 2, and 1.
		m_triangles[0] = 0;
		m_triangles[1] = 2;
		m_triangles[2] = 1;
		// Define the upper right triangle.
		m_triangles[3] = 2;
		m_triangles[4] = 3;
		m_triangles[5] = 1;

		// Orient the normals of the vertices so that the quad is rendered correctly.
		// In this case they all face in the same direction (towards the front
		// of the quad).
		m_normals = new Vector3[4];
		m_normals[0] = -Vector3.forward;
		m_normals[1] = -Vector3.forward;
		m_normals[2] = -Vector3.forward;
		m_normals[3] = -Vector3.forward;

		// Set up the texture coordinates (UVs) to display the entire image
		// on the surface of the quad. UVs are normalized between 0 and 1.
		m_uv = new Vector2[4];
		m_uv[0] = new Vector2(0, 0);
		m_uv[1] = new Vector2(1, 0);
		m_uv[2] = new Vector2(0, 1);
		m_uv[3] = new Vector2(1, 1);
	}
}