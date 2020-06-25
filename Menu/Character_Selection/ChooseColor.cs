using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseColor : MonoBehaviour
{
    public bool colorChosen = false;
    public bool isActive = false;
    [Space]
    public GameObject colorBar;
    public ColorField[] colorFields;
    public Image colorImage;
    public Color[] colorsToChooseFrom;

    int index = 0;
    int playerNumber;
    bool axisInUse = false;
    LoadOut lo;
    PlayerSelect ps;

    float timeUI = 0;
    private void Start()
    {
        lo = GetComponentInParent<LoadOut>();
        ps = GetComponent<PlayerSelect>();

        colorsToChooseFrom = lo.colors;
        ps.chosenColor = colorsToChooseFrom[index];


        for (int i = 0; i < colorFields.Length; i++)
        {
            colorFields[i].OnColorShow(colorsToChooseFrom[i]);
        }
        colorImage.color = colorsToChooseFrom[index];

    }
    private void Update()
    {
        playerNumber = ps.playerNumber;

        if (isActive)
        {
            colorBar.SetActive(true);
            if(!colorChosen)
            {
                Scrolling();
                SetColor();
            }            
            if(Input.GetButtonDown("Special"+playerNumber))
            {
                colorBar.GetComponent<Image>().color = Color.white;
                colorChosen = false;
                ps.OptionDeselect();
            }
        }  
        else if (!isActive && !colorChosen)
        {
            colorBar.SetActive(false);
        }
        else if (colorChosen)
        {
            colorBar.SetActive(true);
            colorBar.GetComponent<Image>().color = Color.green;
        }
    }

    private void SetColor()
    {
        if(Input.GetButtonDown("Standard"+playerNumber))
        {
            if(lo.CheckColorUniqueness(ps.chosenColor))
            {
                colorChosen = true; 
                colorBar.SetActive(true);
                colorBar.GetComponent<Image>().color = Color.green;
                ps.OptionChosen();
            }
        }
    }

    private void Scrolling()
    {
        float UIAxis = Input.GetAxis("UIHorizontal" + playerNumber);
        
        // scroll through options by holding input
        if(UIAxis != 0)
        {
            timeUI += Time.deltaTime;
            if (timeUI > .25f)
            {
                //colorFields[index].colorBorder.enabled = false;
                if(UIAxis > 0)
                {
                    index++;
                    if (index == colorsToChooseFrom.Length)
                    {
                        index = 0;
                    }
                }
                else if(UIAxis < 0)
                {
                    if (index == 0)
                        index = colorsToChooseFrom.Length - 1;
                    else
                        index--;
                }
                //colorFields[index].colorBorder.enabled = true;
                colorImage.color = colorsToChooseFrom[index];
                ps.chosenColor = colorsToChooseFrom[index];
                timeUI = 0;
            }

        }

        // scroll by pressing input
        if(UIAxis > 0 && !axisInUse)
        {
            axisInUse = true;
            //colorFields[index].colorBorder.enabled = false;
            index++;
            if (index == colorsToChooseFrom.Length)
            {
                index = 0;
            }
            //colorFields[index].colorBorder.enabled = true;
            colorImage.color = colorsToChooseFrom[index];

            ps.chosenColor = colorsToChooseFrom[index];
        }

        else if (UIAxis < 0 && !axisInUse)
        {
            axisInUse = true;
            //colorFields[index].colorBorder.enabled = false;

            if (index == 0)            
                index = colorsToChooseFrom.Length - 1;            
            else           
                index--;

            //colorFields[index].colorBorder.enabled = true;
            colorImage.color = colorsToChooseFrom[index];

            ps.chosenColor = colorsToChooseFrom[index];
        }
        else if (UIAxis == 0)
        {
            timeUI = 0;
            axisInUse = false;
        }
    }
}
