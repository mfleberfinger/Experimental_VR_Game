using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class experimentUIcolorChangeThing : MonoBehaviour
{

    public static bool toggle = true;
    public void callBackButton(GameObject caller)
    {
        var img = caller.GetComponent<Image>();
        if (toggle)
            img.color = Color.cyan;
        else
            img.color = Color.green;

        toggle = !toggle;
    }
}
