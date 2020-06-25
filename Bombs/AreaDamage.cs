using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    public ThrowingBomb itemToSpawn;
    public bool canAttack = true;
    public float timeBetweenAttacks = 3;


    public void AttackTarget(Transform position)
    {
        if(canAttack)
        {
            canAttack = false;
            ThrowingBomb bomb = Instantiate(itemToSpawn);
            bomb.gravity = 9.8f;
            bomb.transform.position = gameObject.transform.position;
            bomb.StartThrow(position);
            StartCoroutine(AttackCooldown());
            
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        canAttack = true;
    }
}
