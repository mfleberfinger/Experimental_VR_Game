using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
	[Tooltip("The unity \"Axis\" name for the capacitive touch sensor used to" +
		" decide whether the player is pointing.")]
	public string triggerTouchButtonName;
	[Tooltip("Gameobject to visibly indicate where the player is pointing.")]
	public GameObject pointer;

	private void Start()
	{
		pointer.SetActive(false);
	}

	private void Update()
	{
		Point();
	}

	/// <summary>
	/// Determine whether the player is pointing and display or hide pointer
	/// visual.
	/// </summary>
	private void Point()
	{
		if (Input.GetButton(triggerTouchButtonName))
			pointer.SetActive(false);
		else
			pointer.SetActive(true);
	}
}
