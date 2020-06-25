using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollect : MonoBehaviour
{
    public bool isFree = true;
    public GameObject point;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isFree)
        {
            other.GetComponent<BombPlacement>().AddStdBomb();
            GetComponentInParent<BombSpawner>().AddToSpawnpointList(this);
            Destroy(point);
            isFree = true;
        }
    }
}
