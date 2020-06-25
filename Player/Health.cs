using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, Idamagable
{
    public int maxHealth;
    public int health = 3;
    public float immuneTime = 3;
    public bool isImmune = false;
    public bool ghostMode = true;
    private bool isGhostAlready = false;
    [HideInInspector] public PlayerUI pui;

    PlayerController pc;
    private void Awake()
    {
        maxHealth = health;
    }
    private void Start()
    {
        pc = GetComponent<PlayerController>();
    }
    public void OnHit(int damage)
    {
       if(!isImmune)
        {
            health -= damage;
            pui.RefreshUI();

            if (health <= 0 && !isGhostAlready)
            {
                pc.PlayerDeath();

                health = 0;
                if(ghostMode)
                {
                    isGhostAlready = true;

                    GameOver.instance.GameOverScreen(pc);
                    transform.tag = "Ghost";

                    pc.currentSpeed = 2;
                    GetComponent<BombPlacement>().specialBombsAmount = GetComponent<BombPlacement>().standardBombsAmount = 0;

                    StartCoroutine(GetBombs());
                }
                else if (!ghostMode)
                {
                    Destroy(gameObject);
                }
            }
            else if(health > 0)
            {
                isImmune = true;
                StartCoroutine(Immunity());
            }
        }
    }

    public void GainHealth(int amount)
    {
        health += amount;
        if(health > 3)
        {
            health = 3;
        }
        if(isGhostAlready)
        {
            transform.tag = "Player";
            pc.currentSpeed = pc.maxSpeed;
            GetComponent<BombPlacement>().specialBombsAmount = GetComponent<BombPlacement>().standardBombsAmount = 0;
            pc.RevivePlayer();
            pui.RefreshUI();
            isGhostAlready = false;
            GameOver.instance.PlayerRevive(pc);
        }

    }

    IEnumerator Immunity()
    {
        yield return new WaitForSeconds(immuneTime);
        isImmune = false;
    }

    IEnumerator GetBombs()
    {
        while(isGhostAlready)
        {
            GetComponent<BombPlacement>().AddStdBomb();
            yield return  new WaitForSeconds(5);
        }
    }
}
