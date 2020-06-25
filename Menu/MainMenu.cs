using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject[] interactableBars;
    public GameObject loadOut;
    public GameObject teamsLoadOut;
    public GameObject options;    

    private int index = 0;
    private GameObject activeBar;
    private int playerControlNumber;
    private bool hasController = false;
    private bool UIAxisUsed = false;
    private float timeUI;
    private void Start()
    {
        activeBar = interactableBars[index];
    }

    private void Update()
    {
        DetermineFirstController();

        if (hasController)
        {
            ScrollThroughMenu();
            SelectMenuOption();
        }
    }

    private void SelectMenuOption()
    {
        if (Input.GetButtonDown("Standard" + playerControlNumber))
        {
            if (activeBar == interactableBars[0]) // play game
            {
                loadOut.SetActive(true);
                loadOut.GetComponent<LoadOut>().InitializeLoadOut(playerControlNumber);

                activeBar.GetComponent<MainMenuBar>().border.SetActive(false);
                index = 0;
                SetActiveBar();
                gameObject.SetActive(false);
            }
            else if (activeBar == interactableBars[1]) // teams
            {
                teamsLoadOut.SetActive(true);
                teamsLoadOut.GetComponent<LoadOut>().InitializeLoadOut(playerControlNumber);

                activeBar.GetComponent<MainMenuBar>().border.SetActive(false);
                index = 0;
                SetActiveBar();
                gameObject.SetActive(false);

            }
            else if (activeBar == interactableBars[2]) // options
            {
                options.SetActive(true);
                options.GetComponent<MenuOptions>().playerControllerNumber = playerControlNumber;
                options.GetComponent<MenuOptions>().mainMenu = gameObject;

                activeBar.GetComponent<MainMenuBar>().border.SetActive(false);
                index = 0;
                SetActiveBar();
                gameObject.SetActive(false);
            }
            else if (activeBar == interactableBars[3]) // quit game
            {
                Debug.Log("Game over");
                Application.Quit();
            }

        }
    }

    private void ScrollThroughMenu()
    {
        float UIAxis = Input.GetAxis("UIVertical" + playerControlNumber);

        if(UIAxis != 0)
        {
            timeUI += Time.deltaTime;
            if(timeUI > 0.25f)
            {
                activeBar.GetComponent<MainMenuBar>().border.SetActive(false);

                if (UIAxis > 0)
                {
                    index++;
                    if (index == interactableBars.Length)
                    {
                        index = 0;
                    }
                }
                else if(UIAxis < 0)
                {
                    if (index == 0)
                    {
                        index = interactableBars.Length - 1;
                    }
                    else
                    {
                        index--;
                    }
                }
                SetActiveBar();
                timeUI = 0;
            }
        }
        if (UIAxis > 0 && !UIAxisUsed)
        {
            activeBar.GetComponent<MainMenuBar>().border.SetActive(false);
            index++;
            if (index == interactableBars.Length)
            {
                index = 0;
            }
            SetActiveBar();

            UIAxisUsed = true;
        }

        else if (UIAxis < 0 && !UIAxisUsed)
        {
            activeBar.GetComponent<MainMenuBar>().border.SetActive(false);
            if (index == 0)
            {
                index = interactableBars.Length - 1;
            }
            else
            {
                index--;
            }
            SetActiveBar();

            UIAxisUsed = true;
        }

        else if (UIAxis == 0)
        {
            timeUI = 0;

            UIAxisUsed = false;
        }
    }

    private void SetActiveBar()
    {
        activeBar = interactableBars[index];
        activeBar.GetComponent<MainMenuBar>().border.SetActive(true);
    }

    private void DetermineFirstController()
    {
        if (!hasController)
        {
            if (Input.GetButtonDown("Standard1") || Input.GetAxis("UIVertical1") != 0)
            {
                playerControlNumber = 1;
                hasController = true;
            }
            else if (Input.GetButtonDown("Standard2") || Input.GetAxis("UIVertical2") != 0)
            {
                playerControlNumber = 2;
                hasController = true;

            }
            else if (Input.GetButtonDown("Standard3") || Input.GetAxis("UIVertical3") != 0)
            {
                playerControlNumber = 3;
                hasController = true;

            }
            else if (Input.GetButtonDown("Standard4") || Input.GetAxis("UIVertical4") != 0)
            {
                playerControlNumber = 4;
                hasController = true;

            }
            Debug.Log(playerControlNumber);
        }
    }
}
