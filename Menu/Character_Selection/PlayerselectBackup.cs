using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerselectBackup : MonoBehaviour
{
    [Header("Currently Selected")]
    public int playerNumber;
    public Color chosenColor;
    public SpecialBombType chosenBomb;
    public int chosenTeam;
    public TMP_Text playerName;

    [Header("Color choices")]
    public ColorField[] colorFields;
    private Color[] colorsToChooseFrom;
    public bool colorChosen = false;

    [Header("Special bomb choices")]
    public SpecialBombType[] bombsToChooseFrom;
    public TMP_Text bombNameText;
    public bool specialChosen = false;

    [Header("Team choice")]
    public bool isTeamGame = false;
    public int[] teams;
    public TMP_Text teamText;
    public bool teamChosen = false;

    [Header("UI elements")]
    public GameObject powerBar;
    public GameObject colorBar;

    private LoadOut mainMenu;
    private int colorIndex = 0;
    public int bombIndex = 0;

    private bool horAxisInUse = false;
    private bool vertAxisInUse = false;

    private bool isChoosingColor = true;
    private void Start()
    {
        mainMenu = GetComponentInParent<LoadOut>();

        SetColors();
        SetBombs();
    }

    private void Update()
    {
        ScrollThroughIndex();

        if (!colorChosen && !specialChosen)
        {
            ChangeActiveBar();
        }

        if (isChoosingColor)
        {
            if (Input.GetButtonDown("Standard" + playerNumber) && !colorChosen)
            {
                if (mainMenu.CheckColorUniqueness(chosenColor))
                {
                    colorChosen = true;
                    colorBar.SetActive(true);
                    colorBar.GetComponent<Image>().color = Color.green;
                }
                if (!specialChosen)
                {
                    isChoosingColor = false;
                    powerBar.SetActive(true);
                }
            }
        }
        else if (!isChoosingColor)
        {
            if (Input.GetButtonDown("Standard" + playerNumber) && !specialChosen)
            {
                specialChosen = true;
                powerBar.SetActive(true);
                powerBar.GetComponent<Image>().color = Color.green;

                if (!colorChosen)
                {
                    isChoosingColor = true;
                    colorBar.SetActive(true);
                }
            }

        }
    }

    private void ChangeActiveBar()
    {
        float vertAxis = Input.GetAxis("UIVertical" + playerNumber);
        if (!vertAxisInUse && vertAxis != 0)
        {
            vertAxisInUse = true;
            if (vertAxis < 0 && !isChoosingColor)
            {
                isChoosingColor = true;
                colorBar.SetActive(true);
                powerBar.SetActive(false);
            }
            else if (vertAxis > 0 && isChoosingColor)
            {
                isChoosingColor = false;
                colorBar.SetActive(false);
                powerBar.SetActive(true);
            }
        }
        else if (vertAxis == 0)
        {
            vertAxisInUse = false;
        }
    }

    private void ScrollThroughIndex()
    {
        float UIAxis = Input.GetAxis("UIHorizontal" + playerNumber);

        if (UIAxis > 0 && !horAxisInUse)
        {
            horAxisInUse = true;

            //scroll through color
            if (!colorChosen && isChoosingColor)
            {
                colorFields[colorIndex].colorBorder.enabled = false;
                colorIndex++;
                if (colorIndex == colorsToChooseFrom.Length)
                {
                    colorIndex = 0;
                }
                colorFields[colorIndex].colorBorder.enabled = true;

                chosenColor = colorsToChooseFrom[colorIndex];
                playerName.color = chosenColor;
            }

            //scroll through bombs
            else if (!isChoosingColor && !specialChosen)
            {
                bombIndex++;
                if (bombIndex == bombsToChooseFrom.Length)
                {
                    bombIndex = 0;
                }
                chosenBomb = bombsToChooseFrom[bombIndex];
                bombNameText.text = chosenBomb.ToString();
            }
        }
        if (UIAxis < 0 && !horAxisInUse)
        {
            horAxisInUse = true;

            //scroll thorugh colors
            if (!colorChosen && isChoosingColor)
            {
                colorFields[colorIndex].colorBorder.enabled = false;

                if (colorIndex == 0)
                {
                    colorIndex = colorsToChooseFrom.Length - 1;
                }
                else
                {
                    colorIndex--;
                }

                colorFields[colorIndex].colorBorder.enabled = true;

                chosenColor = colorsToChooseFrom[colorIndex];
                playerName.color = chosenColor;
            }

            //scroll through bombs
            if (!specialChosen && !isChoosingColor)
            {
                if (bombIndex == 0)
                {
                    bombIndex = bombsToChooseFrom.Length - 1;
                }
                else
                {
                    bombIndex--;
                }

                chosenBomb = bombsToChooseFrom[bombIndex];
                bombNameText.text = chosenBomb.ToString();
            }
        }
        if (UIAxis == 0)
        {
            horAxisInUse = false;
        }
    }

    private void SetColors()
    {
        colorsToChooseFrom = mainMenu.colors;
        chosenColor = colorsToChooseFrom[colorIndex];
        playerName.color = chosenColor;

        for (int i = 0; i < colorsToChooseFrom.Length; i++)
        {
            colorFields[i].OnColorShow(colorsToChooseFrom[i]);
        }

        colorFields[colorIndex].colorBorder.enabled = true;
    }
    private void SetBombs()
    {
        bombIndex = 0;
        bombsToChooseFrom = mainMenu.availablePowers;
        chosenBomb = bombsToChooseFrom[bombIndex];

        bombNameText.text = chosenBomb.ToString();
    }

    private void SetTeams()
    {

    }
}
