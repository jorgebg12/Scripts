using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleEstatuas : MonoBehaviour
{

    public GameObject[] statues;
    public GameObject finaldoor;
    [HideInInspector]public bool resuelto = false;
    
    public void checkSolved(){

        Debug.Log("Chekea" );
        
        int count = 0;
        for(int i = 0; i < statues.Length; i++){

            GameObject estatua = statues[i];

            GameObject vela = estatua.transform.Find("vela").gameObject;
            GameObject luz = vela.transform.Find("Light").gameObject;

            if(luz.activeInHierarchy == true){

                count++;

            }
        }

        if(count==statues.LongLength){

            resuelto=true; 
            openFinalDoor();
            
        }

    }
    private void openFinalDoor(){

        GameObject izquierda = finaldoor.transform.Find("izquierda").gameObject;
        GameObject derecha = finaldoor.transform.Find("derecha").gameObject;
            
        izquierda.transform.Rotate(Vector3.forward, 100.0f );
        derecha.transform.Rotate(Vector3.forward, -100.0f );
    }
}
