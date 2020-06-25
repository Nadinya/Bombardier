using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    [Header("Specific player info")]
    public int playersInGame;
    public Color[] colors = new Color[4];
    public SpecialBombType[] bombs = new SpecialBombType[4];
    public int[] playerNumberOrder = new int[4];
    public int[] playerTeamOrder = new int[4];

    public static Data instance;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
