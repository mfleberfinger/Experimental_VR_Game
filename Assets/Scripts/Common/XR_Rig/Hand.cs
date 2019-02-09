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
	[Tooltip("Animation Controller Object")]
	public GameObject animControlObj;
    [Tooltip("Grip Axis ID")]
    public string gripAxis;

    private Animator m_animator;

	private void Start()
	{
		pointer.SetActive(false);
        m_animator = animControlObj.GetComponent<Animator>();
	}

	private void Update()
	{
        float gripInput = Input.GetAxisRaw(gripAxis);
        m_animator.SetFloat("Grab", gripInput);
        DebugMessenger.instance.SetDebugText(string.Format("input: {0}", gripInput));
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
