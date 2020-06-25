using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    BombPlacement bombPlacement;

    [Header("Player info")]
    public float maxSpeed = 12;
    public float currentSpeed = 12;
    public int playerNumber = 1;
    public int teamNumber = 1;
    public Color playerColor;
    public Material playerdeadMat;
    public Material playerUIMat;
    private Material playerMat;

    [Header("Prefabs")]
    public GameObject dustCloudPrefab;
    public AreaDamage areaDamage;

    [Header("Score info")]
    public int totalWins = 0;
    public int totalDeaths = 0;
    public string winsPref = "WonGames";
    public string deathsPref = "TotalDeaths";

    //Private movement
    [HideInInspector] public bool canMoveAndRotate = true;
    Rigidbody rb;
    private float angle;
    private Vector3 movementInput;
    private Animator animator;

    private bool coroutineAllowed = true;
    private bool gameIsStarted = false;
    public bool canMove = true;

    private float afkTime = 0;

    private Vector3 direction;


    #region Unity magic methods
    private void OnEnable()
    {
        SceneManagement.onPauseAction += StartPauseGame;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        bombPlacement = GetComponent<BombPlacement>();

        currentSpeed = maxSpeed;

        GameOver.instance.AddToList(this, true);

        totalWins = PlayerPrefs.GetInt(winsPref + playerNumber);
        totalDeaths = PlayerPrefs.GetInt(deathsPref + playerNumber);

        areaDamage = FindObjectOfType<AreaDamage>();
    }


    private void Update()
    {
        if(gameIsStarted)
        {
            movementInput = new Vector3(Input.GetAxis("Horizontal" + playerNumber), 0, Input.GetAxis("Vertical" + playerNumber));

            if(movementInput == Vector3.zero)
            {
                afkTime += Time.deltaTime;
                if(afkTime > 30)
                {
                    areaDamage.AttackTarget(transform);
                    afkTime = 0;
                }
            }
            else
            {
                afkTime = 0;
            }
            SpawnDustClouds();
        }


        MoveAnimation();
    }

    private void FixedUpdate()
    {
        if(gameIsStarted)
        {
            if (canMoveAndRotate)
            {
                Turn();
                if(canMove)
                {
                    Move();
                }
            }
        }
    }
    private void OnDisable()
    {
        SceneManagement.onPauseAction -= StartPauseGame;
    }
    #endregion

    #region  Movement and rotation
    private void Move()
    {
        Vector3 movement = movementInput * currentSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void Turn()
    {
        if (movementInput.x != 0.0f || movementInput.z != 0.0f)
        {
            angle = Mathf.Atan2(movementInput.x, movementInput.z) * Mathf.Rad2Deg;
            if(!canMove)
            {
                angle = angle / 2;

            }
            direction = gameObject.transform.forward;
        }


        Quaternion turnRotation = Quaternion.Euler(0, angle, 0);
        rb.MoveRotation(turnRotation);
    }

    private void MoveAnimation()
    {
        if (canMoveAndRotate)
        {
            animator.SetBool("isMoving", movementInput != Vector3.zero);
        }
        if(!canMoveAndRotate)
        {
            animator.SetBool("isMoving", false);
        }
    }
    #endregion

    public Vector3 GetDirection()
    {
        return direction;
    }

    public void InstantiatePlayer(Color color)
    {
        playerColor = color;
        SetPlayerColor();
    }

    private void SetPlayerColor()
    {
        playerMat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        playerMat.SetColor("_PlayerColor", playerColor);
        playerUIMat.SetColor("_PlayerUIColor", playerColor);
    }

    private void StartPauseGame()
    {
        gameIsStarted = !gameIsStarted;
    }
    private void SpawnDustClouds()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), Vector3.down, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Floor") && coroutineAllowed && movementInput != new Vector3(0, 0, 0))
            {
                StartCoroutine(SpawnCloud());
                coroutineAllowed = false;
            }
            else
            {
                StopCoroutine(SpawnCloud());
                coroutineAllowed = true;
            }
        }
    }
    IEnumerator SpawnCloud()
    {
        Instantiate(dustCloudPrefab, transform.position, dustCloudPrefab.transform.rotation);
        yield return new WaitForSeconds(0.10f);
    }
    
    public void PlayerDeath()
    {
        totalDeaths++;
        PlayerPrefs.SetInt(deathsPref + playerNumber, totalDeaths);

        StartFreeze(2);
        animator.SetTrigger("isDying");

        StartCoroutine(MaterialSwap(1.5f, playerdeadMat));
    }
    public void RevivePlayer()
    {
        StartCoroutine(MaterialSwap(0.5f, playerMat));
    }

    public void PlayerWin()
    {
        totalWins++;
        PlayerPrefs.SetInt(winsPref + playerNumber, totalWins);
    }
    public void StartFreeze(float freezeTime)
    {
        StartCoroutine(Freeze(freezeTime));
       
    }
    private IEnumerator Freeze(float freezeTime)
    {
        canMoveAndRotate = false;
        yield return new WaitForSeconds(freezeTime);
        canMoveAndRotate = true;
    }
    private IEnumerator MaterialSwap(float delayTime, Material material)
    {
        yield return new WaitForSeconds(delayTime);
        GetComponentInChildren<SkinnedMeshRenderer>().material = material;
    }

}
