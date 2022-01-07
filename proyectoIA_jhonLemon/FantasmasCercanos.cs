using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FantasmasCercanos : MonoBehaviour
{
    public static GameObject[] buscarFantasmasCercanos(Vector3 origen, float radio)
    {
        Collider[] hitColliders = Physics.OverlapSphere(origen, radio);
        GameObject[] enemigos = new GameObject[0];
        for(int i = 0; i < hitColliders.Length; i++)
        {
            if(hitColliders[i].gameObject.tag == "ghosts")
            {
                GameObject[] enemies = new GameObject[enemigos.Length + 1];
                for(int j = 0; j < enemigos.Length; j++)
                {
                    enemies[j] = enemigos[j];
                }
                enemies[enemies.Length-1] = hitColliders[i].gameObject;
                enemigos = enemies;
            }
        }
        return enemigos;
    }
}
