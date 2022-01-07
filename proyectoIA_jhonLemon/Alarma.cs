using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarma : MonoBehaviour
{
    public float duracion;
    public AudioSource alarma;

    private bool activa;
    private float tiempoTranscurrido;
    private bool contando;
    private GameObject[] fantasmas;

    // Start is called before the first frame update
    void Awake()
    {
        activa = false;
        contando = false;
        tiempoTranscurrido = 0f;
    }

    void FixedUpdate() {
        if(contando){
            tiempoTranscurrido += Time.deltaTime;
            desactivar(fantasmas);
        }
        else{
            tiempoTranscurrido = 0f;
        }
    }

    //Activa el estado alarma de todos los fantasmas
    public void activar(GameObject[] ghosts, Vector3 origenAlarma){
        activa = true;
        alarma.Play();
        GhostStates ghost;
        Vector3 objetivo;

        foreach (GameObject g in ghosts){
            ghost = g.GetComponent<GhostStates>();
            if (ghost.puntoVigilancia != null)
                objetivo = ghost.puntoVigilancia.position;
            else
                objetivo = origenAlarma;
            
            ghost.UpdateState(States.Alarma, objetivo);
        }
    }

    //Desactiva el estado de alarma de todos los fantasmas
    public void desactivar(GameObject[] ghosts){
        
        if(tiempoTranscurrido >= duracion){
            activa = false;
            contando = false;
            alarma.Stop();
            GhostStates ghost;
            //Vector3 objetivo;

            foreach (GameObject g in ghosts){
                ghost = g.GetComponent<GhostStates>();

                //objetivo = ghost.siguienteObjetivoEnPatrulla();
                
                ghost.UpdateStateAlarma(States.Patrulla);
            }
            Debug.LogWarning("Se ha desactivado la alarma");
        }
        else if(!contando) {
            contando = true;
            fantasmas = ghosts;
        }
    }

    public bool isActive(){
        return activa;
    }
}
