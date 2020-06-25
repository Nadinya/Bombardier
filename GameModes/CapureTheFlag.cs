using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapureTheFlag : SceneManagement
{
    public Flag flagPrefab;
    [Header("Teams")]
    public Transform[] flagLocations1;
    public Transform[] flagLocations2;
    List<PlayerController> team1 = new List<PlayerController>();
    List<PlayerController> team2 = new List<PlayerController>();


    protected override void InitializePlayers()
    {
        base.InitializePlayers();


        for (int i = 0; i < activePlayers.Count; i++)
        {
            if (activePlayers[i].teamNumber == 1)
            {
                team1.Add(activePlayers[i]);
            }
            else if (activePlayers[i].teamNumber == 2)
            {
                team2.Add(activePlayers[i]);
            }
        }

        PlaceFlags();
    }
    

    public void PlaceFlags()
    {
        for (int i = 0; i < team1.Count; i++)
        {
            //team1[i].transform.position = flagLocations1[i].position + (flagLocations1[i].right * 1.5f);

            InstantiateFlag(flagLocations1[i].position, team1[i], 1);

        }

        for (int i = 0; i < team2.Count; i++)
        {
            //team2[i].transform.position = flagLocations2[i].position - flagLocations2[i].right;

            InstantiateFlag(flagLocations2[i].position, team2[i], 2);

        }

    }

    private void InstantiateFlag(Vector3 pos, PlayerController player, int team)
    {
        Flag flag = Instantiate(flagPrefab);
        flag.InitializeFlag(player.playerColor, player, team);
        if(team == 1)
        {
            flag.transform.position = pos ;
        }
        else
        {
            flag.transform.position = pos + transform.forward + transform.right;
            flag.transform.Rotate(0, 180, 0);
        }

    }
}
