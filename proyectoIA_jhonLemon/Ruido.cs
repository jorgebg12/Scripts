using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruido : MonoBehaviour
{
    public AudioSource noise;
    public int noise_range;
    GameObject[] ghosts;

    //la función makeNoise debería ser llamada cuando un limón choque contra la pared
    public void makeNoise(Vector3 origen_ruido){
        //Debug.Log("inserte ruido");

        noise.Play();
        ghosts = FantasmasCercanos.buscarFantasmasCercanos(this.transform.position, noise_range);
        
        if (ghosts.Length <= 0) Debug.LogWarning("Fantasmas cerca del ruido: " + ghosts.Length);
        
        GhostStates ghost;
        foreach (GameObject g in ghosts){
            ghost = g.GetComponent<GhostStates>();
            ghost.UpdateState(States.Investigar, origen_ruido);
        }
    }
}
