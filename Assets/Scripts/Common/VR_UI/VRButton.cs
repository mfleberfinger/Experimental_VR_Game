using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VRButton : MonoBehaviour
{
	[Tooltip("Functions to call when the button is pressed.")]
	public UnityEvent callbacks;
	[Tooltip("Trigger used to make this button interactable.")]
	public Collider trigger;
	[Tooltip("Color to use when the button is not being interacted with.")]
	public Color defaultColor;
	[Tooltip("Color to use when the button is hovered over.")]
	public Color hoveredColor;
	[Tooltip("The image used to represent the button.")]
	public Image image;

	/// <summary>
	/// Tell the button that it is being hovered over.
	/// </summary>
	public void HoverStart()
	{
		image.color = hoveredColor;
	}

	/// <summary>
	/// Tell the button that it is no longer being hovered over.
	/// </summary>
	public void HoverEnd()
	{
		image.color = defaultColor;
	}

	/// <summary>
	/// Press the button, causing it to call its callback functions.
	/// </summary>
	public void Press()
	{
		callbacks.Invoke();
	}
}
