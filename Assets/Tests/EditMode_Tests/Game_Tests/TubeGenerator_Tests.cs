using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{	
	public class TubeGenerator_Tests
	{
		/// <summary>
		/// Call DefineCircle and verify that all returned points are at the
		/// correct distance from the origin and evenly spaced. Verify that
		/// the correct number of points is returned. Verify that there are no
		/// duplicate points.
		/// </summary>
		[Test]
		public void DefineCircle_Test()
		{
			Vector3[] circle;
			HashSet<Vector3> existingPoints;
			int nextK;
			float radius, expectDistance, distance;

			// For several numbers of points...
			for (int i = 2; i < 5; i++)
			{
				// For several radii...
				for (int j = 1; j <= 5; j++)
				{
					// Create a circle.
					circle = TubeGenerator.DefineCircle(i, j);
					Assert.That(circle.Length, Is.EqualTo(i), "Wrong number of " +
						"points returned.");
					existingPoints = new HashSet<Vector3>();
					expectDistance = Vector3.Distance(circle[0], circle[1]);
					// For each point...
					for (int k = 0; k < circle.Length; k++)
					{
						// Verify that the point is at the correct radius.
						radius = Vector3.Distance(circle[k], Vector3.zero);
						Assert.That(Mathf.Approximately(radius, j), Is.True,
							"Point found at wrong radius.");

						// Verify that no other point exists at this position.
						Assert.That(existingPoints.Contains(circle[k]), Is.False,
							"Overlapping points found.");
						existingPoints.Add(circle[k]);

						// Verify that the point is the same distance from its
						//	neighbor as all of the other points.
						nextK = k + 1 < circle.Length ? k + 1 : 0;
						distance = Vector3.Distance(circle[k], circle[nextK]);
						Assert.That(Mathf.Approximately(expectDistance, distance),
							Is.True, "Inconsistent point spacing found.");
					}
				}
			}
		}
	}
}
