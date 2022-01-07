using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player;
    public GameEnding gameEnding;

    bool mm_IsPlayerInRange;

    void OnTriggerEnter (Collider other)
    {
        if (other.transform == player)
        {
            mm_IsPlayerInRange = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.transform == player)
        {
            mm_IsPlayerInRange = false;
        }
    }

    void Update ()
    {
        if (mm_IsPlayerInRange)
        {
            Debug.Log("Update");
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;
            
            if (Physics.Raycast (ray, out raycastHit))
            {
                
                if (raycastHit.collider.transform == player)
                {
                    Debug.Log("Update2");
                    gameEnding.CaughtPlayer ();
                }
            }
        }
    }
}
