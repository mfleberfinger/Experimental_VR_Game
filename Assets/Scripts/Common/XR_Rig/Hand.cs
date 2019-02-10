using System.Collections;
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
	public bool m_gripPressed
	{
		get
		{
			return Input.GetAxisRaw(gripAxisName) >= 0.5f;
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
		Point();
	}

	/// <summary>
	/// Determine whether the player is pointing and display or hide pointer
	/// visual.
	/// </summary>
	private void Point()
	{
		pointer.SetActive(!m_triggerTouched);
	}

	/// <summary>
	/// Determine whether the hand should play the grip animation.
	/// </summary>
	private void Animate()
	{
		if (m_gripPressed && !m_animator.GetBool("IsGrabbing"))
			m_animator.SetBool("IsGrabbing", true);
		else if (!m_gripPressed && m_animator.GetBool("IsGrabbing"))
			m_animator.SetBool("IsGrabbing", false);
		
		if (!m_triggerTouched && !m_animator.GetBool("IsPointing"))
			m_animator.SetBool("IsPointing", true);
		else if (m_triggerTouched && m_animator.GetBool("IsPointing"))
			m_animator.SetBool("IsPointing", false);
	}
}
