using UnityEngine;
using System.Collections;

/// <summary>
/// A script that causes the gameobject to which it is attached to move forward
/// and turn, following a gradually changing, procedurally generated path.
/// </summary>
public class PathFollower : MonoBehaviour
{
	[Tooltip("Speed at which the path follower moves.")]
	public float speed = 10f;
	[Tooltip("Random number generator seed for path generation.")]
	public string seed = "seed";
	[Tooltip ("If true, the rotation of the object is controlled by this script.")]
	public bool controlRotation = true;
	[Tooltip("Seconds to wait before movement starts.")]
	public float startingDelay = 0f;
	[Tooltip("Maximum slope (as in \"rise over run\") of climb or descent (on the" +
		" y-axis) with respect to distance traveled on the xz-plane.")]
	public float maxSlope = 0.5f;
	[Tooltip("By how much should the input to the PathGenerator vary with each" +
		" frame when getting the next location to move to? This controls the" +
		" resolution of the path (where smaller numbers result in higher resolution).")]
	public float wStepSize = 0.15f;

	// Object used to define and get points on the path.
	private PathGenerator pathGenerator;
	// Bool used to delay moving the follower.
	private bool started;
	// Imaginary fourth dimension used as a parameter to the path generator.
	// Can also be thought of as an arbitrary time based parameter.
	private float w;
	private Vector3 previousPosition, nextPosition;

	private void Start()
	{
		pathGenerator = new PathGenerator(seed);
		w = 0;
		// The follower is assumed to start at 0.
		transform.position = Vector3.zero;
		previousPosition = transform.position;
		nextPosition = transform.position;
	}

	
	private void Update()
	{
		if (started)
		{
			Move();
		}
		else
			started = Time.time > startingDelay;
	}

	/// <summary>
	/// Move the game object to which this is attached.
	/// </summary>
	/// <remarks>As written, this will not work correctly with Unity's physics
	/// engine.</remarks>
	private void Move()
	{
		// If nextPosition is reached, increment w and set new nextPosition.
		if (Vector3.Distance(transform.position, nextPosition) < 0.001f)
		{
			w += wStepSize;
			previousPosition = nextPosition;
			nextPosition =
				pathGenerator.GetPointWithLimitedSlope(w, previousPosition, maxSlope);
			if (controlRotation)
				transform.LookAt(nextPosition);
		}
		// Move towards nextPosition at constant speed.
		transform.position =
			Vector3.MoveTowards(transform.position, nextPosition, Time.deltaTime * speed);
		
	}
}
