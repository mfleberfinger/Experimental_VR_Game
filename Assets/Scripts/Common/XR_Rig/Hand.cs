using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
	// Default value for the animator parameters controlling finger curl.
	private const float RELAXED = 0.2f;

	[Tooltip("The Unity \"Axis\" name for the capacitive touch sensor used to" +
		" decide whether the player is pointing.")]
	public string triggerTouchButtonName;
	[Tooltip("The Unity \"Axis\" name for the grip button on the controller.")]
	public string gripAxisName;
	[Tooltip("Gameobject to visibly indicate where the player is pointing.")]
	public GameObject pointer;
	[Tooltip("Animation Controller Object")]
	public GameObject animControlObj;

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
		DebugMessenger.instance.SetDebugText(Input.GetAxisRaw("Grip - Left").ToString());
        Animate();
	}

	/// <summary>
	/// Do blend tree animation.
	/// </summary>
	private void Animate()
	{
		float gripValue = RELAXED;

		if (m_gripValue > 0.05)
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
}
