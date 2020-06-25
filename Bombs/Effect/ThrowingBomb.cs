using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingBomb : MonoBehaviour
{
    private Transform targetDestination;
    public Bomb bombPrefab;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;

    public void StartThrow(Transform destination)
    {
        targetDestination = destination;
        StartCoroutine(SimulateProjectile());

    }


    IEnumerator SimulateProjectile()
    {
        

        // Move projectile to the position of throwing object + add some offset if needed.
        transform.position = transform.position + new Vector3(0, 0.0f, 0);

        // Calculate distance to target
        float target_Distance = Vector3.Distance(transform.position, targetDestination.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // Rotate projectile to face the target.
        transform.rotation = Quaternion.LookRotation(targetDestination.position - transform.position);

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }
        Bomb bomb = Instantiate(bombPrefab);
        bomb.transform.position = transform.position;
        yield return new WaitForEndOfFrame();
        if(targetDestination.CompareTag("Player") || targetDestination.CompareTag("Ghost"))
        {
            targetDestination = null;
        }
        else
        {
            Destroy(targetDestination.gameObject);

        }
        Destroy(this.gameObject);
    }
}
