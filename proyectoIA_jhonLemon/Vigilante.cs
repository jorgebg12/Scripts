using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vigilante : MonoBehaviour
{
    public Transform player;
    public GameObject[] ghosts;
    public Alarma alarma;


    //Si se detecta al jugador, se activa la alarma
    void OnTriggerEnter (Collider other)
    {
        if (other.transform == player)
        {
            if (!alarma.isActive()){
                alarma.activar(ghosts, this.transform.position);
            }

        }
    }

    void OnTriggerExit(Collider other) {
        if (other.transform == player){
            if(alarma.isActive()){
                alarma.desactivar(ghosts);
            }
        }
    }
}
