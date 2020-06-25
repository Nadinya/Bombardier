using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadOut : MonoBehaviour
{
    public int sceneToLoad;
    [Space]
    public int players = 0;
    public GameObject[] playerPortraits;
    public Color[] colors = new Color[] { Color.red, Color.blue, Color.green, Color.cyan };
    public SpecialBombType[] availablePowers;

    [Header("Teams")]
    public bool isTeamGame = false;
    public int[] teams = new int[] { 1, 2 }; 

    [Header("Selected props")]
    public List<Color> selectedColors;
    public List<int> playerNumberOrder;
    public List<SpecialBombType> selectedPowers = new List<SpecialBombType>();
    public List<int> selectedTeams = new List<int>();

    [Header("UI Tools")]
    public Button playButton;
    public GameObject mainMenuCanvas;
    public GameObject loadOutCanvas;
    public GameObject pressToPlayText;

    private int playerControllerNumber;
    private int playersReady = 0;



    private bool ReadyToPlay = false;

    private bool player1 = false;
    private bool player2 = false;
    private bool player3 = false;
    private bool player4 = false;

    private void Update()
    {
        PlayerEnterGame(); 
        if(playersReady == players && playersReady > 1)
        {
            
            ReadyToPlay = true;
            pressToPlayText.SetActive(true);
        }
        else
        {
            ReadyToPlay = false;
            pressToPlayText.SetActive(false);
        }

        if (ReadyToPlay && Input.GetButtonDown("Standard" + playerControllerNumber))
        {
            for (int i = 0; i < players; i++)
            {
                PlayerSelect ps = playerPortraits[i].GetComponent<PlayerSelect>();
                selectedColors.Add(ps.chosenColor);
                selectedPowers.Add(ps.chosenBomb);
                if(isTeamGame)
                {
                    selectedTeams.Add(ps.chosenTeam);
                }
            }
            PlayGame();
        }

        if (players == 0)
        {
            SceneManager.LoadScene(0);
        }
    } 

    public void InitializeLoadOut(int playerNumber)
    {
        playerControllerNumber = playerNumber;
        switch (playerNumber)
        {
            case 1:
                player1 = true;
                PlayerJoined(1);
                break;
            case 2:
                player2 = true;
                PlayerJoined(2);
                break;
            case 3:
                player3 = true;
                PlayerJoined(3);
                break;
            case 4:
                player4 = true;
                PlayerJoined(4);
                break;
        }
    }  

    public void PlayerReady()
    {
        playersReady++;
    }
    public void PlayerBack(int playerNumber)
    {
        playersReady--;
        players--;
        if (playerNumber == 1)
        {
            player1 = false;
        }
        else if (playerNumber == 2)
        {
            player2 = false;
        }
        else if (playerNumber == 3)
        {
            player3 = false;
        }
        else if(playerNumber == 4)
        {
            player4 = false;
        }
        playerNumberOrder.Remove(playerNumber);

    }
    public bool CheckColorUniqueness(Color color)
    {
        List<Color> tempColors = new List<Color>();
        for (int i = 0; i < players; i++)
        {
            ChooseColor cc = playerPortraits[i].GetComponent<ChooseColor>();

            if(cc.colorChosen)
            {
                tempColors.Add(playerPortraits[i].GetComponent<PlayerSelect>().chosenColor);
            }
        }
        if(!tempColors.Contains(color))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckTeamAvailability(int team)
    {
        int t1 = 0;
        int t2 = 0;
        
        for (int i = 0; i < players; i++)
        {
            ChooseTeam ct = playerPortraits[i].GetComponent<ChooseTeam>();
            if(ct.teamChosen)
            {
                if(playerPortraits[i].GetComponent<PlayerSelect>().chosenTeam == 1)
                {
                    t1++;
                }
                else
                {
                    t2++;
                }
            }
        }

        if(team == 1)
        {
            if(t1 < 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (team == 2)
        {
            if (t2 < 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    // Check whose inputs has been noticed and assign this to player++
    private void PlayerEnterGame()
    {
        if (!player1 && Input.GetButtonDown("Standard1"))
        {
            player1 = true;
            PlayerJoined(1);
        }
        if (!player2 && Input.GetButtonDown("Standard2"))
        {
            player2 = true;
            PlayerJoined(2);
        }
        if (!player3 && Input.GetButtonDown("Standard3"))
        {
            player3 = true;
            PlayerJoined(3);
        }
        if (!player4 && Input.GetButtonDown("Standard4"))
        {
            player4 = true;
            PlayerJoined(4);
        }
    }

    private void PlayerJoined(int number)
    {

        players++;
        playerPortraits[players - 1].SetActive(true);
        playerPortraits[players - 1].GetComponent<PlayerSelect>().playerNumber = number;
        playerNumberOrder.Add(number);

    }

    public void ReturnToMenu()
    {
        mainMenuCanvas.SetActive(true);
        loadOutCanvas.SetActive(false);
    }

    public void PlayGame()
    {
        PlayerPrefs.DeleteAll();
        if(!isTeamGame)
        {
            for (int i = 0; i < players; i++)
            {
                if (i % 2 == 0)
                {
                    selectedTeams.Add(1);
                }
                else
                {
                    selectedTeams.Add(2);
                }                
            }
        }
        Data.instance.colors = selectedColors.ToArray();
        Data.instance.bombs = selectedPowers.ToArray();
        Data.instance.playerTeamOrder = selectedTeams.ToArray();
        Data.instance.playerNumberOrder = playerNumberOrder.ToArray();

        Data.instance.playersInGame = players;

        SceneManager.LoadScene(sceneToLoad);
    }

}
