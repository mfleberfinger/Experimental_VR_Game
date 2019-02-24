using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Tests
{
    public class UIColliderScaler_Tests
    {
		/// <summary>
		/// Create a UI element with a box collider, call ScaleCollider(), and
		/// verify that the collider's x and y size match the UI element's x and
		/// y size.
		/// </summary>
		[Test]
		public void ScaleCollider_Test()
		{
			GameObject mockUI = CreateMockUI();
			UIColliderScaler scaler = mockUI.GetComponentInChildren<UIColliderScaler>();
			BoxCollider collider = mockUI.GetComponentInChildren<BoxCollider>();
			GameObject testElement = collider.gameObject;
			RectTransform rectTransform = testElement.GetComponent<RectTransform>();
			Vector3 originalCenter = collider.center;
			float originalZSize = collider.size.z;

			// Assumptions
			Assume.That(collider.size.x, Is.Not.EqualTo(rectTransform.rect.width));
			Assume.That(collider.size.y, Is.Not.EqualTo(rectTransform.rect.height));
			
			// Test and Assertions
			scaler.ScaleCollider();
			Assert.That(collider.size.x, Is.EqualTo(rectTransform.rect.width));
			Assert.That(collider.size.y, Is.EqualTo(rectTransform.rect.height));
			Assert.That(collider.center, Is.EqualTo(originalCenter));
			Assert.That(collider.size.z, Is.EqualTo(originalZSize));
		}

		/// <summary>
		/// Create a simple UI with a UI element with a collider and a
		/// UIColliderScaler component.
		/// </summary>
		/// <returns></returns>
		private GameObject CreateMockUI()
		{
			GameObject mockUI = new GameObject();
			GameObject testElement = new GameObject();
			BoxCollider collider = null;
			RectTransform rectTransform = null;
			Vector3 colliderSize = Vector3.zero;
			UIColliderScaler scaler = null;

			// Set up the UI canvas.
			mockUI.AddComponent<Canvas>();
			mockUI.AddComponent<CanvasScaler>();

			// Add the test UI element.
			testElement.transform.SetParent(mockUI.transform);
			testElement.AddComponent<Image>();
			collider = testElement.AddComponent<BoxCollider>();
			// For testing, the size of the collider should be different from
			// the element's size initially.
			rectTransform = testElement.GetComponent<RectTransform>();
			colliderSize = new Vector3(rectTransform.rect.size.x * 0.5f, 
				rectTransform.rect.size.y * 0.5f, 1f);
			collider.size = colliderSize;

			// Add the UIColliderScaler.
			scaler = testElement.AddComponent<UIColliderScaler>();
			scaler.coll = collider;
			scaler.rectTransform = rectTransform;

			return mockUI;
		}
    }
}
