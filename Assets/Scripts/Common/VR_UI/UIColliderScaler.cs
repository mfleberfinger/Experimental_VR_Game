using UnityEngine;

public class UIColliderScaler : MonoBehaviour
{
	[Tooltip("Collider used by this UI element.")]
	public BoxCollider coll;
	[Tooltip("RectTransform to base collider size on.")]
	public RectTransform rectTransform;

	private void Start()
	{
		ScaleCollider();
	}

	/// <summary>
	/// Make the VR UI collider controlled by this script match the x and y
	/// size of the UI element to which it is attached.
	/// </summary>
	public void ScaleCollider()
	{
		Rect rect = rectTransform.rect;
		coll.size = new Vector3(rect.width, rect.height, coll.size.z);
	}
}
