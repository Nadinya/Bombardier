using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseTeam : MonoBehaviour
{
    public bool teamChosen = false;
    public bool isActive = false;
    [Space]
    public GameObject teamBar;
    public int[] teamsToChooseFrom;
    public TMP_Text teamNameText;

    int index = 0;
    int playerNumber;
    bool axisInUse = false;
    LoadOut lo;
    PlayerSelect ps;
    private void Start()
    {
        lo = GetComponentInParent<LoadOut>();
        ps = GetComponent<PlayerSelect>();

        teamsToChooseFrom = lo.teams;

        ps.chosenTeam = teamsToChooseFrom[index];
        string teamText = "Team " + ps.chosenTeam;
        teamNameText.text = teamText;
    }
    private void Update()
    {
        playerNumber = ps.playerNumber;

        if (isActive)
        {
            teamBar.SetActive(true);
            if (!teamChosen)
            {
                Scrolling();
                if (Input.GetButtonDown("Standard" + playerNumber))
                {
                    if(lo.CheckTeamAvailability(ps.chosenTeam))
                    {
                        teamChosen = true;
                        teamBar.SetActive(true);
                        teamBar.GetComponent<Image>().color = Color.green;
                        ps.OptionChosen();
                    }
                }
            }
            if (Input.GetButtonDown("Special" + playerNumber))
            {
                teamBar.GetComponent<Image>().color = Color.white;
                teamChosen = false;
                ps.OptionDeselect();
            }
        }
        else if (!isActive && !teamChosen)
        {
            teamBar.SetActive(false);
        }
        else if (teamChosen)
        {
            teamBar.SetActive(true);
            teamBar.GetComponent<Image>().color = Color.green;
        }
    }

    private void Scrolling()
    {
        float UIAxis = Input.GetAxis("UIHorizontal" + playerNumber);

        if (UIAxis > 0 && !axisInUse)
        {
            axisInUse = true;

            index++;
            if (index == teamsToChooseFrom.Length)
            {
                index = 0;
            }
            ps.chosenTeam = teamsToChooseFrom[index];
            string teamText = "Team " + ps.chosenTeam;
            teamNameText.text = teamText;
        }

        else if (UIAxis < 0 && !axisInUse)
        {
            axisInUse = true;

            if (index == 0)
            {
                index = teamsToChooseFrom.Length - 1;
            }
            else
            {
                index--;
            }

            ps.chosenTeam = teamsToChooseFrom[index];
            string teamText = "Team " + ps.chosenTeam;
            teamNameText.text = teamText;
        }
        else if (UIAxis == 0)
        {
            axisInUse = false;
        }
    }
}
