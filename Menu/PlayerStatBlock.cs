using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatBlock : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName = default;
    [SerializeField] private TMP_Text playerWins = default;
    [SerializeField] private TMP_Text playerDeaths = default;
    public int playerNumber;

    public void SetStats(PlayerController player)
    {
        playerName.text = player.transform.name;
        playerWins.text = PlayerPrefs.GetInt(player.winsPref + player.playerNumber).ToString();
        playerDeaths.text = PlayerPrefs.GetInt(player.deathsPref + player.playerNumber).ToString();
    }
}
