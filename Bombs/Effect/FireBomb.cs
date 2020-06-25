using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBomb : Bomb
{
    public DOT firePatch;

    public override void StartExplosion()
    {
        base.StartExplosion();
        GameObject newFirePatch = Instantiate(firePatch.gameObject);
        newFirePatch.transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
    }
}
