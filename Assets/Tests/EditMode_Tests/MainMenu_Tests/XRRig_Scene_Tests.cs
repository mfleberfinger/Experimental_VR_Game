using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SpatialTracking;

namespace Tests
{
	/// <summary>
	/// Test whether the XR Rig is in the scene and properly configured.
	/// </summary>
	public class XRRig_Scene_Tests
	{
		/// <summary>
		/// SetUp method to open the proper scene for testing.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
		}
        
		/// <summary>
		/// Test for the XR Rig's existence in the scene.
		/// </summary>
		[Test]
		public void XRRig_Found_Test()
		{
			GameObject rig = null;
			rig = GameObject.Find("XRRig");
			Assert.That(rig, Is.Not.Null, "GameObject named \"XRRig\" not in" +
				" scene or not enabled.");
		}

		/// <summary>
		/// Test for a camera on the XR Rig.
		/// </summary>
		[Test]
		public void XRRig_Has_Camera_Test()
		{
			GameObject rig = null;
			Camera cam = null;

			// Assumptions
			rig = GameObject.Find("XRRig");
			Assert.That(rig, Is.Not.Null, "GameObject named \"XRRig\" not in" +
				" scene or not enabled.");

			// Assertions
			cam = rig.GetComponentInChildren<Camera>();
			Assert.That(cam, Is.Not.Null, "Camera not found on XR Rig.");
		}

		/// <summary>
		/// Make sure the XR Rig has a tracked pose driver for the camera and
		/// each hand.
		/// </summary>
		[Test]
		public void XRRig_Has_TrackedPoseDrivers()
		{
			GameObject rig = null;
			TrackedPoseDriver[] trackedPoseDrivers = null;

			// Assumptions
			rig = GameObject.Find("XRRig");
			Assert.That(rig, Is.Not.Null, "GameObject named \"XRRig\" not in" +
				" scene or not enabled.");

			// Assertions
			trackedPoseDrivers = rig.GetComponentsInChildren<TrackedPoseDriver>();
			Assert.That(trackedPoseDrivers.Length, Is.EqualTo(3), "There should" +
				" be exactly 3 tracked pose drivers on the XR Rig.");
		}
	}
}
