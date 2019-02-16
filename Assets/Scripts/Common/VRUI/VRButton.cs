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

    void Start()
    {
    }

    void Update()
    {
        
    }
}
