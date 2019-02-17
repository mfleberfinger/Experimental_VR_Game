using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayPointer : MonoBehaviour
{
	// Buttons pointed to by all instances of RayPointer.
	// Keyed by pressed buttons, value is the number of pointers on the button.
	private static Dictionary<VRButton, int> hoveredButtons;

	[Tooltip("Transform from which to raycast. Transform.Forward will be used" +
		" to set direction.")]
	public Transform raycastOrigin;
	[Tooltip("Controller button used to press a UI button.")]
	public string pressButton;

	/// <summary>
	/// The button being pointed at.
	/// </summary>
	public VRButton m_hoveredButton { get; private set; }
	
	private void Start()
	{
		if (hoveredButtons == null)
			hoveredButtons = new Dictionary<VRButton, int>();

		m_hoveredButton = null;
	}

	private void OnDisable()
	{
		HoverEnd();
	}

	private void Update()
	{
		if (Input.GetButtonDown(pressButton))
			Press();
	}

	private void FixedUpdate()
	{
		VRButton button = null;
		RaycastHit hit;
		bool hitOcurred = Physics.Raycast(raycastOrigin.position,
			raycastOrigin.forward, out hit);

		// If something is hit, try to find a button script.
		if (hitOcurred)
			button = hit.collider.gameObject.GetComponent<VRButton>();
			
		// If we're hovering over a button we weren't already hovering over,
		// inform the new and old buttons of the change and remember what we're
		// hovering over.
		if (button != m_hoveredButton && button != null)
		{
			HoverEnd();
			HoverStart(button);
		}
		else if (button == null)
			HoverEnd();
	}

	/// <summary>
	/// Called when the player stops hovering the pointer over a button.
	/// </summary>
	private void HoverEnd()
	{
		if (m_hoveredButton != null)
		{
			// Keep track of hover end in static dictionary.
			// Only end the Hover on the button if the button is not being
			//	hovered over by another pointer.
			if (hoveredButtons.ContainsKey(m_hoveredButton))
			{
				hoveredButtons[m_hoveredButton]--;
				if (hoveredButtons[m_hoveredButton] <= 0)
				{
					hoveredButtons.Remove(m_hoveredButton);
					m_hoveredButton.HoverEnd();
				}
			}
			else
				m_hoveredButton.HoverEnd();

			m_hoveredButton = null;
		}
	}

	/// <summary>
	/// Called when the player points at a button.
	/// </summary>
	/// <param name="button">The <c>VRButton</c> being pointed at.</param>
	private void HoverStart(VRButton button)
	{
		m_hoveredButton = button;
		m_hoveredButton.HoverStart();

		// Keep track of hover started in dictionary.
		if (hoveredButtons.ContainsKey(button))
			hoveredButtons[button]++;
		else
			hoveredButtons.Add(button, 1);
	}

	/// <summary>
	/// Press the hovered button, if there is one.
	/// </summary>
	private void Press()
	{
		if (m_hoveredButton != null)
			m_hoveredButton.Press();
	}
}
