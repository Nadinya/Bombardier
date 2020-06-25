using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public float timeBetweenSpawns = 4;
    [Space]
    public Transform spawnPointsParent;
    public GameObject standardPrefab;

    Transform spawnPoint;
    bool spawnPointChosen = false;
    bool gameHasStarted = false;

    public List<BombCollect> spawnPoints = new List<BombCollect>();
    private void OnEnable()
    {
        SceneManagement.onPauseAction += GameStart;

    }
    private void Start()
    {

        foreach (Transform spawnPoint in spawnPointsParent)
        {
            if(spawnPoint.GetComponent<BombCollect>())
            {
                AddToSpawnpointList(spawnPoint.GetComponent<BombCollect>());
            }
        }
    }

    private void Update()
    {
        if(gameHasStarted)
        {
            if (!spawnPointChosen && spawnPoints.Count > 0)
            {
                StartCoroutine(InstantiateBombPoint());
                spawnPointChosen = true;
            }
        }
    }

    private void GameStart()
    {
        gameHasStarted = !gameHasStarted;
    }

    public void AddToSpawnpointList(BombCollect spawnPoint)
    {
        spawnPoints.Add(spawnPoint);
    }

    private IEnumerator InstantiateBombPoint() //Do NOT change name. String referenced in Start.
    {
        yield return new WaitForSeconds(timeBetweenSpawns);

        int i = Random.Range(0, spawnPoints.Count - 1);

        spawnPoint = spawnPoints[i].transform;
        spawnPoints[i].GetComponent<BombCollect>().isFree = false;

        GameObject point = Instantiate(standardPrefab, spawnPoint);
        point.transform.position = spawnPoint.position;
        spawnPoint.GetComponent<BombCollect>().point = point;

        spawnPoints.RemoveAt(i);

        spawnPoint = null;
        spawnPointChosen = false;
    }

    private void OnDisable()
    {
        SceneManagement.onPauseAction -= GameStart;

    }
}
