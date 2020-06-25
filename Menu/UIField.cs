using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIField : MonoBehaviour
{
    public Image image;
    public GameObject border;
    public TMP_Text nameText;



    public void OnColorShow(Color color)
    {
        nameText.gameObject.SetActive(false);
        image.gameObject.SetActive(true);
        image.color = color;
    }
    public void ShowText(SpecialBombType bomb)
    {
        nameText.gameObject.SetActive(true);
        image.gameObject.SetActive(false);
        nameText.text = bomb.ToString();
    }

}

