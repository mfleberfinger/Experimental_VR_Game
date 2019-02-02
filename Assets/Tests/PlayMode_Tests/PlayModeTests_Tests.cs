using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
	/// <summary>
	/// Dummy test to verify that PlayMode tests are being recognized by Unity.
	/// </summary>
    public class PlayModeTests_Tests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void PlayModeTests_TestsSimplePasses()
        {
            // Use the Assert class to test conditions
			Debug.Log("The PlayMode test test ran.");
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator PlayModeTests_TestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
