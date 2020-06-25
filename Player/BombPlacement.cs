using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPlacement : MonoBehaviour
{
    int playerNumber;
    public PlayerUI pui;
    public float timeBeforeThrow = 0.5f;
    public float throwSpeed = 3f;
    private PlayerController pc;
    [Header("Inventory")]
    public int standardBombsAmount;
    public int specialBombsAmount;

    [Header("Prefabs")]
    public Bomb bombPrefab;
    public Bomb specialBombPrefab;
    public ThrowingBomb throwPrefab;
    public SpecialBombType currentType = SpecialBombType.Fire; // will be set in load-out screen
    
    public Transform targetRosePrefab;
    private Transform targetTransform;
    private Vector3 endLocation;
    float buttonDownTime = 0;
    Vector3 direction;
    bool gameHasStarted = false;

    private void Awake()
    {
        SceneManagement.onPauseAction += PauseGame;
    }
    private void Start()
    {
        pc = GetComponent<PlayerController>();
        playerNumber = pc.playerNumber;
    }

    private void PauseGame()
    {
        gameHasStarted = !gameHasStarted;
    }
    private void Update()
    {
        if(gameHasStarted)
        {
            if (Input.GetButton("Standard" + playerNumber))
            {
                //InitializeThrowingBomb();
            }
            if (Input.GetButtonUp("Standard" + playerNumber))
            {
                if(buttonDownTime < timeBeforeThrow)
                {
                    PlaceBomb();
                }
                else if (buttonDownTime > timeBeforeThrow)
                {
                    PlaceThrowingBomb();
                }               
            }
            if (Input.GetButtonDown("Special"+playerNumber))
            {
                PlaceSpecialBomb();
            }
        }
    }

    private void InitializeThrowingBomb()
    {
        pc.canMove = false;
        buttonDownTime += Time.deltaTime;

        direction = pc.GetDirection();

        if (buttonDownTime > timeBeforeThrow)
        {
            if (targetTransform == null) // create target and reset its position based on char
            {
                targetTransform = Instantiate(targetRosePrefab);
                targetTransform.position = gameObject.transform.position + direction;
                targetTransform.rotation = gameObject.transform.rotation;
            }
            else if (targetTransform != null) // move test forward
            {
                targetTransform.position += direction * Time.deltaTime * throwSpeed;
            }
        }
    }
    private void PlaceBomb()
    {
        pc.canMoveAndRotate = true;
        pc.canMove = true;
        buttonDownTime = 0;

        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, out RaycastHit hit, 1))
        {
            //if raycast hits a crate, take 1 step back and place bomb
            if (standardBombsAmount > 0 && hit.transform.GetComponent<Breakable>())
            {
                Bomb bomb = Instantiate(bombPrefab);
                bomb.GetComponent<Bomb>().placedBy = gameObject;

                bomb.transform.position = transform.position;
                transform.position -= transform.forward;

                standardBombsAmount--;
                pui.RefreshUI();
            }
            //if raycast hits a bomb, kick it
            else if (hit.transform.GetComponent<Bomb>())
            {
                Vector3 dir = pc.GetDirection();
                hit.transform.GetComponent<Bomb>().KickTheBomb(dir);
            }
        }
        // if raycast hits nothing, just place a bomb
        else if (standardBombsAmount > 0)
        {
            pc.canMoveAndRotate = true;
            pc.canMove = true;
            Bomb bomb = Instantiate(bombPrefab);
            bomb.GetComponent<Bomb>().placedBy = gameObject;

            bomb.transform.position = transform.position + transform.forward;
            standardBombsAmount--;
            pui.RefreshUI();
        }
    }
    private void PlaceThrowingBomb()
    {
        pc.canMoveAndRotate = true;
        pc.canMove = true;
        buttonDownTime = 0;
        endLocation = targetTransform.position;

        ThrowingBomb bomb = Instantiate(throwPrefab);
        bomb.transform.position = gameObject.transform.position;
        bomb.StartThrow(targetTransform.transform);
        targetTransform = null;


        standardBombsAmount--;
        pui.RefreshUI();
    }
    private void PlaceSpecialBomb()
    {
        if (specialBombsAmount > 0)
        {
            Bomb bomb = Instantiate(specialBombPrefab);


            bomb.GetComponent<Bomb>().placedBy = gameObject;

            bomb.transform.position = transform.position + transform.forward;
            specialBombsAmount--;
            pui.RefreshUI();
        }
    }

    public void AddStdBomb()
    {
        if(standardBombsAmount == 99)
        {
            standardBombsAmount = 99;
        }
        else
        {
            standardBombsAmount++;
        }
        pui.RefreshUI();
    }
    public void AddSpecial()
    {
        specialBombsAmount++;

        pui.RefreshUI();
    }

    private void OnDisable()
    {
        SceneManagement.onPauseAction -= PauseGame;
    }
}
