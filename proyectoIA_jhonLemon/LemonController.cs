using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum lemonStates
{
    Idle,
    Grabbed,
    onWall,
    onFloor
}

public class LemonController : MonoBehaviour
{
    AudioSource audioData;

    public lemonStates state;
    public GhostStates[] fantasmasCercanos;
    public BoxCollider myCollider;
    public Ruido ruido;
    private GameObject caughtGhost;
    private GhostStates ghostStates;

    void Awake()
    {
        state = lemonStates.Idle;
        audioData = GetComponent<AudioSource>();
    }

    void Update()
    {
        switch (state)
        {
            case lemonStates.onWall:
                //Debug.Log("Me he estrellado con la PARED!");
                //Asignar los fantasmasCercanos
                //Atrae fantasmasCercanos
            break;
            case lemonStates.onFloor:
                //Debug.Log("Me has estampado contra el SUELO!");
            break;
            default:
                this.gameObject.transform.Rotate(0,0,60*Time.deltaTime);
            break;
        }
    }

    public void grab () 
    {
        state = lemonStates.Grabbed;
    }

    public void thrownToWall (Vector3 normal) 
    {
        Debug.Log("Me he estrellado con la PARED!");
        Debug.Log("NORMAL: " + normal);

        Vector3 factoresIncremento;
        factoresIncremento.x = - Mathf.Abs(normal.x) + 1;
        factoresIncremento.y = - Mathf.Abs(normal.y) + 1;
        factoresIncremento.z = - Mathf.Abs(normal.z) + 1;

        transform.rotation = Quaternion.Euler(new Vector3 (normal.x * 90, normal.y * 90, normal.z * 90));

        float x = transform.localScale.x;
        float y = transform.localScale.y;
        float z = transform.localScale.z;
        float incremento = 20.0f;

        transform.localScale = new Vector3(
        x + (incremento * factoresIncremento.x),
        y + (incremento * factoresIncremento.y),
        z + (incremento * factoresIncremento.z)
        );

        ruido.makeNoise(this.transform.position);

        state = lemonStates.onWall;
    }

    public void thrownToFloor () 
    {
        audioData.Play(0);
        Debug.Log("Me has estampado contra el SUELO!");
        transform.rotation = Quaternion.Euler(0,0,0);

        float x = transform.localScale.x;
        float y = transform.localScale.y;
        float z = transform.localScale.z;
        float incremento = 20.0f;

        transform.localScale = new Vector3(
        x+incremento,
        y,
        z+incremento
        );

        state = lemonStates.onFloor;
    }

    private void OnTriggerEnter(Collider other) {
        
        if(state == lemonStates.onFloor){
            Debug.Log("Algo me ha pisado!");
        }

        if(other.gameObject.tag == "ghosts" && state == lemonStates.onFloor){
            caughtGhost = other.gameObject;

            ghostStates = caughtGhost.GetComponent<GhostStates>();
            ghostStates.Caer();
            myCollider.enabled = false;
            Debug.Log("Un fantasma se ha caido!");
        }

    }
}
