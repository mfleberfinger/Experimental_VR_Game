using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tests
{
    public class VRButton_Tests
    {
		bool m_callbackCalled;

		[SetUp]
		public void SetUp()
		{
			m_callbackCalled = false;
		}

		/// <summary>
		/// Call HoverStart() and make sure it changes the button color to
		/// hoveredColor.
		/// </summary>
        [Test]
        public void HoverStart_Color_Test()
        {
			GameObject GO = CreateVRButton();
			VRButton button = GO.GetComponent<VRButton>();
			Image image = GO.GetComponent<Image>();

			// Assumptions
			Assume.That(image.color, Is.Not.EqualTo(button.hoveredColor));

			// Function call and Assertions
			button.HoverStart();
			Assert.That(image.color, Is.EqualTo(button.hoveredColor));
        }

		/// <summary>
		/// Set the button's color to something other than the default color,
		/// then call HoverEnd() and make sure the color changes to the default.
		/// </summary>
        [Test]
        public void HoverEnd_Color_Test()
        {
			GameObject GO = CreateVRButton();
			VRButton button = GO.GetComponent<VRButton>();
			Image image = GO.GetComponent<Image>();

			// Assumptions
			Assume.That(image.color, Is.Not.EqualTo(button.defaultColor));

			// Function call and Assertions
			button.HoverEnd();
			Assert.That(image.color, Is.EqualTo(button.defaultColor));
        }

		/// <summary>
		/// Call Press() and make sure the callback function is called.
		/// </summary>
		[Test]
        public void Press_Callback_Test()
        {
			GameObject GO = CreateVRButton();
			VRButton button = GO.GetComponent<VRButton>();

			// Assumptions
			Assume.That(m_callbackCalled, Is.False);

			// Function call and Assertions
			button.Press();
			Assert.That(m_callbackCalled, Is.True, "Button did not call callback.");
        }

		/// <summary>
		/// Create a GameObject with a VRButton component attached to allow
		/// testing.
		/// </summary>
		/// <returns></returns>
		private GameObject CreateVRButton()
		{
			GameObject parentObject = new GameObject();
			VRButton buttonScript = null;
			UnityEvent callbacks = new UnityEvent();
			Image image = null;

			// Add and set up the button script.
			buttonScript = parentObject.AddComponent<VRButton>();
			callbacks.AddListener(CallbackFunction);
			buttonScript.callbacks = callbacks;
			buttonScript.defaultColor = Color.blue;
			buttonScript.hoveredColor = Color.red;
			image = parentObject.AddComponent<Image>();
			buttonScript.image = image;
			
			// Set the image so that it doesn't match hovered or default color.
			// This is a requirement for at least two of the tests in this class.
			image.color = Color.white;

			return parentObject;
		}

		/// <summary>
		/// A dummy function for testing callbacks. Will set a variable to true
		/// in this class. That variable can then be used in an assert.
		/// </summary>
		private void CallbackFunction()
		{
			m_callbackCalled = true;
		}
    }
}
