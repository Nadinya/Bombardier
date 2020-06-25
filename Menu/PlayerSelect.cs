using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSelect : MonoBehaviour
{
    public bool isTeamGame;
    public bool playerReady = false;
    [Header("Currently Selected")]
    public int playerNumber;
    public Color chosenColor;
    public SpecialBombType chosenBomb;
    public int chosenTeam;
    [Space]
    public TMP_Text playerName;
    public ChooseColor color;
    public ChooseSpecial special;
    public ChooseTeam team;
    public GameObject[] selectionBars;


    private int optionsSelected = 0;
    private LoadOut loadOut;
    private int index = 0;

    private bool axisInUse = false;

    private void Start()
    {
        loadOut = GetComponentInParent<LoadOut>();
    }

    private void Update()
    {
        ChangeActiveBar();
        playerName.color = chosenColor;

        if (Input.GetButtonDown("Special" + playerNumber))
        {
            if(optionsSelected == 0)
            {
                loadOut.PlayerBack(playerNumber);
                gameObject.SetActive(false);
            }
        }
    }

    public void OptionChosen()
    {
        optionsSelected++;
        if(optionsSelected == selectionBars.Length)
        {
            playerReady = true;
            loadOut.PlayerReady();
        }
    }
    public void OptionDeselect()
    {
        optionsSelected--;
    }

    private void ChangeActiveBar()
    {
        float vertAxis = Input.GetAxis("UIVertical" + playerNumber);
        if(!axisInUse && vertAxis !=0)
        {
            axisInUse = true;
            if (vertAxis > 0 )
            {
                index++;
                if(index == selectionBars.Length)
                {
                    index = 0;
                }

            }
            else if (vertAxis < 0)
            {
                if(index  == 0)
                {
                    index = selectionBars.Length - 1;
                }
                else 
                    index--;

            }
            SetActivebar();
        }
        else if (vertAxis == 0)
        {
            axisInUse = false;
        }
    }

    private void SetActivebar()
    {
        if(isTeamGame)
            team.isActive = false;        
        color.isActive = false;
        special.isActive = false;

        switch (index)
        {
            case 0:
                color.isActive = true;
                break;
            case 1:
                special.isActive = true;
                break;
            case 2:
                team.isActive = true;
                break;
            default:
                Debug.LogError("undefined index in Player Select");
                break;
        }
    }

}
