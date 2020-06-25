using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterBomb : Bomb
{
    public Bomb standardBomb;
    public int clusterBombSpawns = 6;
    public float radius = 2;

    public override void StartExplosion()
    {
        base.StartExplosion();        

        float angle = 360f / clusterBombSpawns;
        for (int i = 0; i < clusterBombSpawns; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(i * angle, Vector3.up);
            Vector3 direction = rotation * Vector3.forward;

            Vector3 position = transform.position + (direction * radius);
            Instantiate(standardBomb, position, rotation);
        }

    }
}

