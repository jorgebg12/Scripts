using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectPlayer : MonoBehaviour
{


    private GhostStates estado;
    public Transform player;

    void Start(){

        estado=transform.parent.gameObject.GetComponent<GhostStates>();

    }

    void OnTriggerExit (Collider other)
    {
        if(other.tag == "Player")
        {
            estado.UpdateState(States.Patrulla, other.transform.position);
        }
    }

    void OnTriggerStay(Collider other)
    {
       if(other.tag == "Player")
        {
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;
          
            if (Physics.Raycast (ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player)
                {
                    estado.UpdateState(States.Perseguir, other.transform.position);
                }
            }
        }
    }
}
