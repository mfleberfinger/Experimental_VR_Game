using UnityEngine;

/// <summary>
/// Class to define a random path and return points along that path in Unity
/// world coordinates.
/// </summary>
public class PathGenerator
{
	// Initial PerlinNoise coordinates for controlling the x position of the follower.
	float m_xpx, m_ypx;
	// Initial PerlinNoise coordinates for controlling the y position of the follower.
	float m_xpy, m_ypy;
	// Initial PerlinNoise coordinates for controlling the z position of the follower.
	float m_xpz, m_ypz;
	// Initial PerlinNoise() return values, used to ensure that the patch will
	// always start at Unity's scene origin.
	float initialXPerlin, initialYPerlin, initialZPerlin;

	/// <summary>
	/// Constructor. Takes a seed and initializes values needed for use with
	/// the PerlinNoise() function.
	/// </summary>
	/// <param name="seed"></param>
	public PathGenerator(string seed)
	{
		InitPathGenerator(seed);
	}

	/// <summary>
	/// Seed the random number generator and get the starting coordinates on
	/// the Perlin noise plane.
	/// </summary>
	/// <param name="seed">Seed value for procedural path generation.</param>
	public void InitPathGenerator(string seed)
	{
		Random.InitState(seed.GetHashCode());
		m_xpx = Random.value;
		m_ypx = Random.value;
		m_xpy = Random.value;
		m_ypy = Random.value;
		m_xpz = Random.value;
		m_ypz = Random.value;
		initialXPerlin = PerlinFunction(m_xpx, m_ypx);
		initialYPerlin = PerlinFunction(m_xpy, m_ypy);
		initialZPerlin = PerlinFunction(m_xpz, m_ypz);
	}

	/// <summary>
	/// Generates the x, y, and z coordinates along the path for a given value
	/// of w. The "w" can be thought of as a fourth spatial dimension or as an
	/// arbitrary parameter to the Perlin noise functions.
	/// </summary>
	/// <param name="z">Some world space z-coordinate.</param>
	/// <returns>The point along the procedurally generated path with the given
	/// w-coordinate.</returns>
	public Vector3 GetPoint(float w)
	{
		float x = PerlinFunction(w + m_xpx, m_ypx) - initialXPerlin;
		float y = PerlinFunction(w + m_xpy, m_ypy) - initialYPerlin;
		float z = PerlinFunction(w + m_xpz, m_ypz) - initialZPerlin;
		return new Vector3(x, y, z);
	}

	/// <summary>
	/// Internal Perlin noise based function used to control various qualities
	/// of the noise being generated and thereby control the path.
	/// </summary>
	/// <param name="z"></param>
	private float PerlinFunction(float x, float y)
	{
		return 1000f * Mathf.PerlinNoise(x, y);
	}
}
