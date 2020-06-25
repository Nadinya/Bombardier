using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Bomb : MonoBehaviour
{
    public float heightY = 0.3f;
    [Space]
    public float bombDistance = 2f;
    public float detonationDelay = 3f;
    public int damage = 1;
    public bool canTriggerShockwave = true;
    public bool canExplodeByDistance = true;
    public float distanceTrigger = 1;
    [Space]
    public GameObject[] players; // all players in game
    public GameObject placedBy; // player that placed this bomb
    public LayerMask layerMask;


    private Animator animator;

    bool hasDetonated = false;

    Rigidbody rb;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, heightY, transform.position.z);
        animator = GetComponent<Animator>();
        layerMask = LayerMask.GetMask("Default");
    }


    private void Update()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 2f, 0) + transform.forward, transform.forward);

        if (!hasDetonated)
        {
            detonationDelay -= Time.deltaTime;

            if(detonationDelay <= 0 )
            {
                StartExplosion();
                hasDetonated = true;
            }
            else if (canExplodeByDistance && DistanceCheck())
            {
                StartExplosion();
                hasDetonated = true;
                animator.SetTrigger("isExploding");
            }
        }
    }

    public void KickTheBomb(Vector3 direction)
    {
        rb = GetComponent<Rigidbody>();
        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.AddForce(direction * 15, ForceMode.Impulse);
    }

    public IEnumerator MoveToPosition(Vector3 destination, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, destination, t);
            yield return null;
        }
    }

    private bool DistanceCheck()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player != placedBy)
            {
                if (Vector3.Distance(transform.position, player.transform.position) < distanceTrigger)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public virtual void StartExplosion()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForEndOfFrame();
        Collider[] colliders = Physics.OverlapSphere(transform.position, bombDistance);
        foreach (Collider coll in colliders)
        {
            if(coll.gameObject != gameObject) // if the object is NOT this object
            {
                if (coll.GetComponent<Health>())// && !beenHit) // if object has a health component
                {
                    RaycastHit hit;
                    Vector3 rayDirection = coll.gameObject.transform.position - transform.position;
                    if (Physics.Raycast(transform.position, rayDirection, out hit))
                    {
                        if (hit.collider == coll)
                        {
                            coll.GetComponent<Health>().OnHit(damage);
                        }
                    }
                    //beenHit = true;
                }

                else if(coll.GetComponent<Breakable>())
                {
                    coll.GetComponent<Breakable>().OnHit(damage);
                }
                else if(coll.GetComponent<Bomb>() && canTriggerShockwave) // if object is another bomb
                {
                    coll.GetComponent<Bomb>().StartExplosion();
                    coll.GetComponent<Bomb>().TriggerExplosion();
                }
            }
        }
        Destroy(gameObject, 3);
    }

    public void TriggerExplosion()
    {
        animator.SetTrigger("isExploding");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, bombDistance);
    }
}
