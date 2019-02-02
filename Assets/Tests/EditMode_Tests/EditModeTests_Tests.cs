using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
	/// <summary>
	/// Dummy test to verify that EditMode tests are being recognized by Unity.
	/// </summary>
    public class EditModeTests_Tests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void EditModeTests_TestsSimplePasses()
        {
            // Use the Assert class to test conditions
			Debug.Log("The EditMode test test ran.");
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator EditModeTests_TestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
