using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBombSpawner : MonoBehaviour
{
    public GameObject bombPointPrefab;
    private GameObject spawnedBombPoint;

    public float minTimeBetweenSpawns, maxTimeBetweenSpawns;

    [Header("If it spawns a specific bomb type")]
    public bool spawnsSpecificBomb = false;
    public SpecialBombType bombType;

    private bool hasSpawned = false; // item is there to show
    private bool isActive = false; // item can be picked up

    // determines time till next spawn
    private bool timeSet = false; 
    private float timeTillNextSpawn;
    private bool canSpawn = false;

    public delegate void OnBombChange(PlayerController player);
    public static event OnBombChange onBombChange;


    private void OnEnable()
    {
        SceneManagement.onPauseAction += GameStart;
    }
    private void Update()
    {
        if(!timeSet)
        {
            timeTillNextSpawn = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);

            timeSet = true;
        }
        if(!hasSpawned && timeSet)
        {
            StartCoroutine(SpawnPoint());
        }
    }

    private void GameStart()
    {
        canSpawn = !canSpawn;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            BombPlacement player = other.GetComponent<BombPlacement>();
            if(!spawnsSpecificBomb)
            {
                player.AddSpecial();
            }

            else if(spawnsSpecificBomb)
            {
                if(player.currentType == bombType)
                {
                    player.AddSpecial();
                }
                else if(player.currentType != bombType)
                {
                    player.specialBombsAmount = 0;
                    player.currentType = bombType;
                    player.AddSpecial();
                    onBombChange(player.GetComponent<PlayerController>());

                }
            }
            Destroy(spawnedBombPoint);
            spawnedBombPoint = null;
            hasSpawned = false;
            isActive = false;
        }
    }



    private IEnumerator SpawnPoint()
    {
        hasSpawned = true;
        yield return new WaitForSeconds(timeTillNextSpawn);
        spawnedBombPoint = Instantiate(bombPointPrefab, gameObject.transform);
        timeSet = false;

        yield return new WaitForEndOfFrame();
        isActive = true;
    }

    private void OnDisable()
    {
        SceneManagement.onPauseAction -= GameStart;
    }
}
