using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public MeshRenderer flagMat;
    public PlayerController flagOwner;
    public int myTeam;


    public bool isBeingCarried = false;

    public bool isAtHomeBase = true;
    public bool isAtEnemyBase = false;
    // pair player to flag, only happens in the beginning of the game
    public void InitializeFlag(Color color, PlayerController player, int team)
    {
        flagMat.material.color = color;
        flagOwner = player;
        myTeam = team;
    }

    // allow players to pick up flags
    private void OnTriggerStay(Collider other)
    {

        if(Input.GetKeyDown(KeyCode.G))
        {
            MoveFlag(other.GetComponent<PlayerController>());
        }
    }
    private void MoveFlag(PlayerController player)
    {
        // todo make distance check?
        if(isBeingCarried)
        {
            // collider sphere cast
            Collider[] colls = Physics.OverlapSphere(transform.position, 1);
            foreach (Collider collider in colls)
            {
                FlagDrop flagDropPoint = collider.GetComponent<FlagDrop>();
                if(flagDropPoint != null)
                {
                    PutOnBase(flagDropPoint);
                    // if back at homebase
                    if (flagDropPoint.myTeam == myTeam)
                    {
                        isAtHomeBase = true;
                        isAtEnemyBase = false;

                        // revive owner or reset health                        
                        flagOwner.GetComponent<Health>().GainHealth(3);

                    }
                    // if at enemy base
                    else if(flagDropPoint.myTeam != myTeam)
                    {
                        isAtEnemyBase = true;
                        isAtHomeBase = false;
                        // kill owner
                        if(flagOwner.CompareTag("Player"))
                        {
                            flagOwner.GetComponent<Health>().OnHit(3);
                        }
                    }
                    StartCoroutine(ChangeStatus());
                    return;
                }
            }
            // place flag in front of player ?? Shoot away?
            gameObject.transform.parent = null;
            isAtEnemyBase = false;
            isAtHomeBase = false;
            StartCoroutine(ChangeStatus());
            return;
        }

        else if(!isBeingCarried)
        {
            if (isAtHomeBase)
            {
                // enemy can pick up flag
                if(player.teamNumber != myTeam)
                {
                    CarryFlag(player);
                }
            }
            else if (isAtEnemyBase)
            {
                // team can pick up flag
                if(player.teamNumber == myTeam)
                {
                    CarryFlag(player);
                }
            }
            else if (!isAtHomeBase && !isAtEnemyBase)
            {
                // enemy AND team can pick up flag
                CarryFlag(player);
                // owner does not change state
            }
        }
    }

    private void CarryFlag(PlayerController player)
    {
        gameObject.transform.parent = player.transform;
        transform.position += player.transform.forward;
        StartCoroutine(ChangeStatus());
    }
    private void PutOnBase(FlagDrop flagDrop)
    {
        gameObject.transform.parent = flagDrop.transform; // put flag on pedestal
        gameObject.transform.position = new Vector3(flagDrop.transform.position.x, transform.position.y, flagDrop.transform.position.z);
        gameObject.transform.rotation = flagDrop.transform.rotation;
        flagDrop.holdsFlag = true;
    }
   
    private IEnumerator ChangeStatus()
    {
        yield return new WaitForSeconds(1);
        isBeingCarried = !isBeingCarried;
    }
}
