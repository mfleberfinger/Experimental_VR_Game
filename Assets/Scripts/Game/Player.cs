using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[Tooltip("The XR Rig's floor offset.")]
	public Transform floorOffset = null;
	[Tooltip("The player's head (the camera and head collider).")]
	public GameObject head = null;
	[Tooltip("The XRRig's top-level GameObject in the scene hierarchy.")]
	public GameObject XRRig = null;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			CenterHead();
	}

	/// <summary>
	/// Move the floor offset to center the player's head vertically within the
	/// tube (assumes that the XRRig GameObject is centered in the tube).
	/// </summary>
	private void CenterHead()
	{
		// Head position relative to the XRRig.
		Vector3 headRigPos = XRRig.transform.InverseTransformPoint(head.transform.position);
		// Floor position relative to the XRRig.
		Vector3 floorRigPos = floorOffset.localPosition;
		
		floorOffset.localPosition =
			new Vector3(floorRigPos.x, floorRigPos.y -headRigPos.y, floorRigPos.z);
	}
}
