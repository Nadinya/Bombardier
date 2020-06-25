using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOT : MonoBehaviour
{
    public bool slowsPlayer = false;
    public float timeBeforeDespawn = 5;
    public int damagePerSecond = 1;
    private float time;
    private float timeBeforeHurts = 1;

    private List<PlayerController> players = new List<PlayerController>();

    private void Start()
    {
        Invoke("DespawnPool", timeBeforeDespawn);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if(!players.Contains(player))
            {
                players.Add(player);

            }

            if(slowsPlayer)
            {
                player.currentSpeed = player.maxSpeed / 2;
            }            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        time += Time.deltaTime;
        if(time > timeBeforeHurts)
        {
            foreach (PlayerController player in players)
            {
                player.GetComponent<Health>().OnHit(damagePerSecond);
            }
            time = 0;
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            players.Remove(other.GetComponent<PlayerController>());
        }
        if (slowsPlayer)
        {
            other.GetComponent<PlayerController>().currentSpeed = other.GetComponent<PlayerController>().maxSpeed;
        }
    }

    private void DespawnPool()
    {
        if(slowsPlayer)
        {
            foreach (PlayerController player in players)
            {
                player.currentSpeed = player.maxSpeed;
            }
        }
        Destroy(gameObject);

    }

}
