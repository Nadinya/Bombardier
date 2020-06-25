using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseSpecial : MonoBehaviour
{
    public bool specialChosen = false;
    public bool isActive = false;
    [Space]
    public GameObject powerBar;
    public SpecialBombType[] bombsToChooseFrom;
    public TMP_Text bombNameText;

    int index = 0;
    int playerNumber;
    bool axisInUse = false;
    LoadOut lo;
    PlayerSelect ps;

    private void Start()
    {
        lo = GetComponentInParent<LoadOut>();
        ps = GetComponent<PlayerSelect>();

        bombsToChooseFrom = lo.availablePowers;
    }

    private void Update()
    {
        playerNumber = ps.playerNumber;

        if (isActive)
        {
            powerBar.SetActive(true);
            if(!specialChosen)
            {
                powerBar.SetActive(true);
                Scrolling();
                if(Input.GetButtonDown("Standard"+playerNumber))
                {
                    specialChosen = true;
                    powerBar.SetActive(true);
                    powerBar.GetComponent<Image>().color = Color.green;
                    ps.OptionChosen();
                }
            }
            if(Input.GetButtonDown("Special"+playerNumber))
            {
                powerBar.GetComponent<Image>().color = Color.white;
                specialChosen = false;
                ps.OptionDeselect();
            }
        }
        else if(!isActive && !specialChosen)
        {
            powerBar.SetActive(false);
        }
        else if(specialChosen)
        {
            powerBar.SetActive(true);
            powerBar.GetComponent<Image>().color = Color.green;
        }
    }

    private void Scrolling()
    {
        float UIAxis = Input.GetAxis("UIHorizontal" + playerNumber);

        if(UIAxis != 0)
        {

        }

        if (UIAxis > 0 && !axisInUse)
        {
            axisInUse = true;

            index++;
            if (index == bombsToChooseFrom.Length)
            {
                index = 0;
            }
            ps.chosenBomb = bombsToChooseFrom[index];
            bombNameText.text = ps.chosenBomb.ToString();
        }

        else if (UIAxis < 0 && !axisInUse)
        {
            axisInUse = true;

            if (index == 0)
            {
                index = bombsToChooseFrom.Length - 1;
            }
            else
            {
                index--;
            }

            ps.chosenBomb = bombsToChooseFrom[index];
            bombNameText.text = ps.chosenBomb.ToString();
        }
        else if (UIAxis == 0)
        {
            axisInUse = false;
        }
    }
}
