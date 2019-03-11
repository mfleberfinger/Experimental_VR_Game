using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the UI 
/// </summary>
public class MainMenuScript : MonoBehaviour
{
	[Tooltip("Path to the scene in which the actual gameplay occurs.")]
	public string scenePath;

	/// <summary>
	/// Start the game.
	/// </summary>
	public void Play()
	{
		SceneManager.LoadScene(scenePath);
	}
}
