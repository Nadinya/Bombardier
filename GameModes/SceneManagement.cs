using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    /// <summary>
    /// For easy starting the game without going through the menu
    /// </summary>
    private Color[] playerColors = new Color[] { Color.red, Color.green, Color.blue, Color.cyan };
    private SpecialBombType[] playerBombs = new SpecialBombType[] { SpecialBombType.Acid, SpecialBombType.Cluster, SpecialBombType.Fire, SpecialBombType.Ice };
    private int[] playerNumbers = new int[] { 1, 2, 3, 4 };
    private int[] teamNumbers = new int[] { 1, 2, 1, 2 };
    [Space]
    [SerializeField] private int amountOfPlayers = 4;
    [SerializeField] private GameObject playerParent = default;
    [SerializeField] private PlayerUI[] playerUI = default;
    [SerializeField] private Material[] playerMats = default;
    [SerializeField] private Transform[] startLocations = default;

    [Header("Start inventory")]
    public int amountOfStandardBombs;
    public int amountOfSpecialBombs;

    [Header("Prefabs")]
    public PlayerController playerPrefab;
    public FreezeBomb freezeBomb;
    public FireBomb fireBomb;
    public ClusterBomb clusterBomb;

    [Space]
    public List<PlayerController> activePlayers = new List<PlayerController>();

    //events
    public delegate void PauseAction();
    public static event PauseAction onPauseAction;

    // Only used in Editor for testing
    public bool startWithoutMenu = false;
    public bool isTeamGame = false;

    // Used to divide teams
    private int t1 = 0;
    private int t2 = 0;
    #region Singleton
    public static SceneManagement instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
            Destroy(this);

        SpecialBombSpawner.onBombChange += InitializeSpecialBombPrefabs;

    }
    #endregion

    private void Start()
    {
        if(!startWithoutMenu)
        {
            amountOfPlayers = Data.instance.playersInGame;
            playerColors = Data.instance.colors;
            playerBombs = Data.instance.bombs;
            playerNumbers = Data.instance.playerNumberOrder;
            teamNumbers = Data.instance.playerTeamOrder;
        }
        InitializePlayers();        
    }

    protected virtual void InitializePlayers()
    {
        for (int i = 0; i < amountOfPlayers; i++)
        {
            PlayerController player = Instantiate(playerPrefab, playerParent.transform);

            CreatePlayer(i, player);

            InitializeSpecialBombPrefabs(player);
        }
        CountDown.instance.StartCountDown();
    }

    private void CreatePlayer(int index, PlayerController player)
    {
        player.transform.name = "Player " + (index + 1);
        player.playerNumber = playerNumbers[index];
        player.teamNumber = teamNumbers[index];
        player.playerUIMat = playerMats[index];

        player.GetComponent<BombPlacement>().currentType = playerBombs[index];
        player.InstantiatePlayer(playerColors[index]);

        player.GetComponent<BombPlacement>().standardBombsAmount = amountOfStandardBombs;
        player.GetComponent<BombPlacement>().specialBombsAmount = amountOfSpecialBombs;

        if(!isTeamGame)
        {
            playerUI[index].gameObject.SetActive(true);
            playerUI[index].InitializeUI(player.gameObject);
            player.transform.position = startLocations[index].position;

        }
        else if(isTeamGame)
        {
            if(player.teamNumber == 1)
            {
                t1++;
                if(t1 == 1)
                {
                    playerUI[0].gameObject.SetActive(true);
                    playerUI[0].GetComponent<Image>().material = player.playerUIMat;
                    playerUI[0].InitializeUI(player.gameObject);
                    player.transform.position = startLocations[0].position;

                }
                else
                {
                    playerUI[2].gameObject.SetActive(true);
                    playerUI[2].GetComponent<Image>().material = player.playerUIMat;

                    playerUI[2].InitializeUI(player.gameObject); 
                    player.transform.position = startLocations[1].position;

                }
            }
            else if(player.teamNumber == 2)
            {
                t2++;
                if(t2 == 1)
                {
                    playerUI[1].gameObject.SetActive(true);
                    playerUI[1].GetComponent<Image>().material = player.playerUIMat;

                    playerUI[1].InitializeUI(player.gameObject);
                    player.transform.position = startLocations[2].position;

                }
                else
                {
                    playerUI[3].gameObject.SetActive(true);
                    playerUI[3].GetComponent<Image>().material = player.playerUIMat;

                    playerUI[3].InitializeUI(player.gameObject); 
                    player.transform.position = startLocations[3].position;
                }
            }
        }
        activePlayers.Add(player);
    }

    private void InitializeSpecialBombPrefabs(PlayerController pc)
    {
        BombPlacement player = pc.GetComponent<BombPlacement>();
        switch (player.currentType)
        {
            case SpecialBombType.Fire:
                player.specialBombPrefab = fireBomb;
                break;

            case SpecialBombType.Ice:
                player.specialBombPrefab = freezeBomb;
                break;

            case SpecialBombType.Cluster:
                player.specialBombPrefab = clusterBomb;
                break;

            case SpecialBombType.Acid:
                player.specialBombPrefab = fireBomb;
                break;

            default:
                break;
        }
    }

    public int GetFirstPlayer()
    {
        return playerNumbers[0];
    }



    public void StopGame()
    {
        onPauseAction();
    }

   
}
