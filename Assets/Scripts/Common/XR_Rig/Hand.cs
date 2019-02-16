﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
	[Tooltip("The Unity \"Axis\" name for the capacitive touch sensor used to" +
		" decide whether the player is pointing.")]
	public string triggerTouchButtonName;
	[Tooltip("The Unity \"Axis\" name for the grip button on the controller.")]
	public string gripAxisName;
	[Tooltip("Gameobject to visibly indicate where the player is pointing.")]
	public GameObject pointer;
	[Tooltip("Animation Controller Object")]
	public GameObject animControlObj;
	[Tooltip("Default value for the animator parameter controlling finger curl.")]
	public float relaxedThreshold = 0.2f;
	[Tooltip("The minimum grip axis value (how tight the fist must be) required" +
		"to point at UI elements.")]
	public float pointThreshold = 0.9f;

    private Animator m_animator;

	/// <summary>
	/// <c>true</c> if the player is touching the trigger on the Rift controller.
	/// </summary>
	public bool m_triggerTouched
	{
		get
		{
			return Input.GetButton(triggerTouchButtonName);
		}
	}
	
	/// <summary>
	/// <c>true</c> if the grip button is pressed.
	/// </summary>
	public float m_gripValue
	{
		get
		{
			return Input.GetAxisRaw(gripAxisName);
		}
	}

	private void Start()
	{
		pointer.SetActive(false);
        m_animator = animControlObj.GetComponent<Animator>();
	}

	private void Update()
	{
        Animate();
		AttemptToPoint();
	}

	/// <summary>
	/// Do blend tree animation.
	/// </summary>
	private void Animate()
	{
		float gripValue = relaxedThreshold;

		if (m_gripValue > relaxedThreshold)
			gripValue = m_gripValue;
		
		// Set thumb, middle, ring, and pinky fingers.
		m_animator.SetFloat("GripCurl", gripValue);
		
		// If the trigger is touched, the index finger follows the other fingers.
		// Otherwise, it is straightened as if pointing.
		if (m_triggerTouched)
			m_animator.SetFloat("IndexCurl", gripValue);
		else
			m_animator.SetFloat("IndexCurl", 0f);
	}

	/// <summary>
	/// Decide whether the player is pointing, then display the pointer visual
	/// and raycast to UI elements if required.
	/// </summary>
	private void AttemptToPoint()
	{
		if (!m_triggerTouched && m_gripValue > pointThreshold)
			pointer.SetActive(true);
		else
			pointer.SetActive(false);
	}
}
