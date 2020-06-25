using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBomb : Bomb
{
    public float freezeTime = 3;
    public GameObject freezeEffectPrefab;

    public override void StartExplosion()
    {
        StartCoroutine(ExplodeIce());
    }

    IEnumerator ExplodeIce()
    {
        yield return new WaitForEndOfFrame();

        Collider[] colliders = Physics.OverlapSphere(transform.position, bombDistance);
        foreach (Collider coll in colliders)
        {
            if (coll.gameObject != gameObject) // if the object is NOT this object
            {
                if (coll.GetComponent<Health>()) // if object has a health component
                {
                    RaycastHit hit;
                    Vector3 rayDirection = coll.gameObject.transform.position - transform.position;

                    if (Physics.Raycast(transform.position, rayDirection, out hit))
                    {
                        if (hit.collider == coll)
                        {
                            coll.GetComponent<PlayerController>().StartFreeze(freezeTime);
                        }
                    }
                }
                else if (coll.GetComponent<Bomb>() && canTriggerShockwave) // if object is another bomb
                {
                    coll.GetComponent<Bomb>().StartExplosion();

                }
            }
        }
        Destroy(gameObject, 3);

    }
}
