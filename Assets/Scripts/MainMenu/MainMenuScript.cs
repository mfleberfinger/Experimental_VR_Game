using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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

    private string m_swapStr = "😂\n😂😂\n😂😂😂\nYEETle";
    private string m_buttonSwap = "S'COREs";
    private string m_titleSwap = "Achievements";
	public void toggleScore(GameObject scoreMenu)
	{
        var textobj = scoreMenu.GetComponentInChildren<TextMeshPro>();
        string tmp = textobj.text;
        textobj.text = m_swapStr;
        m_swapStr = tmp;

        var texts = scoreMenu.GetComponentsInChildren<Text>();
        Text titleText = texts[0];
        tmp = titleText.text;
        titleText.text = m_titleSwap;
        m_titleSwap = tmp;

        Text btnText = texts[1];
        tmp = btnText.text;
        btnText.text = m_buttonSwap;
        m_buttonSwap = tmp;
	}

    public void toggleButtonText(GameObject buttonText)
    {
        var text = buttonText.GetComponent<Text>();
    }
}
