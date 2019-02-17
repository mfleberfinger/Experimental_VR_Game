using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
	public Color color;
	public Image image;

	public void SetColor()
	{
		image.color = color;
	}
}
