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
	/// of w.
	/// </summary>
	/// <param name="w">The "w" can be thought of as a fourth spatial dimension
	/// or as an arbitrary parameter to the Perlin noise functions.</param>
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
	/// Calculate the next point along the path, as in GetPoint(), but limit the
	/// maximum rate at which the vertical position changes.
	/// </summary>
	/// <param name="w">The "w" can be thought of as a fourth spatial dimension
	/// or as an arbitrary parameter to the Perlin noise functions.</param>
	/// <param name="lastPoint">The last point used by the thing following this
	/// path.</param>
	/// <param name="maxSlope">y = mx + b. Don't climb or descend more steeply
	/// than this.</param>
	/// <returns>The point along the procedurally generated path with the given
	/// w-coordinate, with its y-component modified to limit the slope of climb
	/// or descent based on the given previous position (lastPoint).</returns>
	public Vector3 GetPointWithLimitedSlope(float w, Vector3 lastPoint, float maxSlope)
	{
		Vector3 nextPoint = GetPoint(w);
		float newY = LimitSlope(lastPoint, nextPoint, maxSlope);
		nextPoint = new Vector3(nextPoint.x, newY, nextPoint.z);

		return nextPoint;
	}

	/// <summary>
	/// Limit the maximum angle that the path will climb or descend at (maximum
	/// (change in y) / (change in x-z plane)). This is intended to avoid making
	/// the player twist their neck to look straight up or down for extended
	/// periods of time. This function could be used similarly to limit the rate
	/// at which position on the other two axes changes if desired.
	/// </summary>
	/// <param name="previous">Previous point.</param>
	/// <param name="next">Next point.</param>
	/// <param name="maxSlope">Maximum slope.</param>
	/// <returns>If next.y results in a slope with magnitude less than or equal
	/// to maxSlope, then return next.y. Otherwise, return a y value that would
	/// result in the slope being equal in magnitude to maxSlope.</returns>
	private float LimitSlope(Vector3 previous, Vector3 next, float maxSlope)
	{
		float newDeltaY = 0f;
		float newY = next.y;
		Vector2 previousXZ = new Vector2(previous.x, previous.z);
		Vector2 nextXZ = new Vector2(next.x, next.z);
		float horizontalDistance = Vector2.Distance(previousXZ, nextXZ);
		float verticalDisplacement = next.y - previous.y;

		// If slope would be greater than the maximum, recalculate next.y such
		// that the slope will equal maxSlope.
		// (slope) = deltaY/deltaX => deltaY = (slope)(deltaX)
		if (Mathf.Abs(verticalDisplacement / horizontalDistance) > maxSlope)
		{
			newDeltaY =
				maxSlope * horizontalDistance * Mathf.Sign(verticalDisplacement);
			newY = previous.y + newDeltaY;
		}

		return newY;
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
