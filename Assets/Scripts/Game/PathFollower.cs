﻿using UnityEngine;
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

	// Object used to define and get points on the path.
	private PathGenerator pathGenerator;
	// Bool used to delay moving the follower.
	private bool started;
	// Imaginary forth dimension used as a parameter to the path generator.
	// Can also be thought of as an arbitrary time based parameter.
	private float w;

	private void Start()
	{
		pathGenerator = new PathGenerator(seed);
		w = 0;
		// The follower is assumed to start at 0.
		transform.position = Vector3.zero;
	}

	
	private void Update()
	{
		if (started)
		{
			w = w + speed * Time.deltaTime;
			Move(w);
		}
		else
			started = Time.time > startingDelay;
	}

	/// <summary>
	/// Move the game object to which this is attached. Make sure it faces in
	/// the direction of travel.
	/// </summary>
	/// <param name="z">The w-coordinate to move to.</param>
	private void Move(float w)
	{
		// TODO: If using the physics engine, this will need to change.
		Vector3 nextPosition = pathGenerator.GetPoint(w);
		
		if (controlRotation)
			transform.LookAt(nextPosition);
		transform.position = nextPosition;
	}
}
