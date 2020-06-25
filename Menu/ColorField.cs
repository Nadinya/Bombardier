using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorField : MonoBehaviour
{
    public Image image;
    public Image colorBorder;

    public void OnColorShow(Color color)
    {
        image.color = color;
    }
}
