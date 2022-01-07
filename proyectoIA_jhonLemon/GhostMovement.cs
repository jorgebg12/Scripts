
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public float speed;
    private GhostStates estado;
    Rigidbody m_Rigidbody;
    //public Transform[] w;
    private int index;
    private bool recta;
    //public float longitud;
    private float[] pasos1 = {0.02f, 0.04f, 0.09f, 0.17f, 0.27f, 0.37f, 0.50f, 0.62f, 0.77f};
    //private float[] pasos1;
    private float[] pasos2 = {0.15f, 0.30f, 0.43f, 0.56f, 0.68f, 0.77f, 0.87f, 0.94f, 0.98f};
    //private float[] pasos2;
    private Vector3[] puntos = new Vector3[9];

    private Vector3 origen, inicio, final;

    private int m_CurrentWaypointIndex;

    void Start()
    {
        //pasos1 = new float[]{longitud/256, longitud/128, longitud/64, longitud/32, longitud/16, longitud/8, longitud/4, longitud/2, longitud/1};
        //pasos2 = new float[]{longitud/6.66f, longitud/3.33f, longitud/2.33f, longitud/1.79f, longitud/1.47f, longitud/1.30f, longitud/1.15f, longitud/1.06f, longitud/1.02f};
        m_Rigidbody = GetComponent<Rigidbody>();
        m_CurrentWaypointIndex = 0;
        //index = 0;
        //origen = transform.position;
    }

    //void FixedUpdate()
    //{
    //    if (moveTo(w[index].position, w[index + 1].position, origen))
    //    {
    //        origen = w[index].position;
    //        index++;
    //        m_CurrentWaypointIndex = 0;
    //    }
    //}

    //public bool moveTo(Vector3 Objective)
    //{
    //    Vector3 camino = Objective - transform.position;
    //    camino *= Time.deltaTime;
    //    camino *= speed;
    //    m_Rigidbody.AddForce(camino);
    //    transform.LookAt(Objective);
    //    float step = speed * Time.deltaTime;
    //    transform.position = Vector3.MoveTowards(transform.position, Objective, step);
    //    if (Mathf.Floor(transform.position.x) == Mathf.Floor(Objective.x) && Mathf.Floor(transform.position.z) == Mathf.Floor(Objective.z))
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        ////Debug.Log(transform.position - Objective);
    //        return false;
    //    }
    //}

    public bool moveTo(Vector3 Objective, Vector3 NextObjective, Vector3 PreviousObjetive, bool enFormacion)
    {
        float t = 0.9f;
        //Debug.Log("Objetivo = "+Objective + " Siguiente = " + NextObjective + "Anterior = " + PreviousObjetive);

        if (Objective.z > PreviousObjetive.z)
        {
            if (NextObjective.x < Objective.x)
            {
                //Debug.Log("giro arriba izq");
                recta = false;
                inicio = new Vector3(Objective.x, 0, Objective.z - t);
                final = new Vector3(Objective.x - t, 0, Objective.z);
                for(int i = 0; i < puntos.Length; i++)
                {
                    puntos[i] = new Vector3(inicio.x - pasos1[i], 0, inicio.z + pasos2[i]);
                }
            }
            else if (NextObjective.x > Objective.x)
            {
                //Debug.Log("giro arriba der");
                recta = false;
                inicio = new Vector3(Objective.x, 0, Objective.z - t);
                final = new Vector3(Objective.x + t, 0, Objective.z);
                for(int i = 0; i < puntos.Length; i++)
                {
                    puntos[i] = new Vector3(inicio.x + pasos1[i], 0, inicio.z + pasos2[i]);
                }
            }
            else
            {
                //Debug.Log("recta arriba");
                recta = true;
                inicio = new Vector3(Objective.x, 0, Objective.z);
                final = new Vector3(Objective.x, 0, Objective.z);
                for(int i = 0; i < puntos.Length; i++)
                {
                    puntos[i] = new Vector3(Objective.x, 0, Objective.z);
                }
            }
        }
        else if (Objective.z < PreviousObjetive.z)
        {
            if (NextObjective.x < Objective.x)
            {
                //Debug.Log("giro abajo izq");
                recta = false;
                inicio = new Vector3(Objective.x, 0, Objective.z + t);
                final = new Vector3(Objective.x - t, 0, Objective.z);
                for(int i = 0; i < puntos.Length; i++)
                {
                    puntos[i] = new Vector3(inicio.x - pasos1[i], 0, inicio.z - pasos2[i]);
                }
            }
            else if (NextObjective.x > Objective.x)
            {
                //Debug.Log("giro abajo der");
                recta = false;
                inicio = new Vector3(Objective.x, 0, Objective.z + t);
                final = new Vector3(Objective.x + t, 0, Objective.z);
                for(int i = 0; i < puntos.Length; i++)
                {
                    puntos[i] = new Vector3(inicio.x + pasos1[i], 0, inicio.z - pasos2[i]);
                }
            }
            else
            {
                //Debug.Log("recta abajo");
                recta = true;
                inicio = new Vector3(Objective.x, 0, Objective.z);
                final = new Vector3(Objective.x, 0, Objective.z);
                for(int i = 0; i < puntos.Length; i++)
                {
                    puntos[i] = new Vector3(Objective.x, 0, Objective.z);
                }
            }
        }
        else
        {
            if (Objective.x < PreviousObjetive.x)
            {
                if (NextObjective.z > Objective.z)
                {
                    //Debug.Log("giro izq arriba");
                    recta = false;
                    inicio = new Vector3(Objective.x + t, 0, Objective.z);
                    final = new Vector3(Objective.x, 0, Objective.z + t);
                    for(int i = 0; i < puntos.Length; i++)
                    {
                        puntos[i] = new Vector3(inicio.x - pasos2[i], 0, inicio.z + pasos1[i]);
                    }
                }
                else if (NextObjective.z < Objective.z)
                {
                    //Debug.Log("giro izq abajo");
                    recta = false;
                    inicio = new Vector3(Objective.x + t, 0, Objective.z);
                    final = new Vector3(Objective.x, 0, Objective.z - t);
                    for(int i = 0; i < puntos.Length; i++)
                    {
                        puntos[i] = new Vector3(inicio.x - pasos2[i], 0, inicio.z - pasos1[i]);
                    }
                }
                else
                {
                    //Debug.Log("recta izq");
                    recta = true;
                    inicio = new Vector3(Objective.x, 0, Objective.z);
                    final = new Vector3(Objective.x, 0, Objective.z);
                    for(int i = 0; i < puntos.Length; i++)
                    {
                        puntos[i] = new Vector3(Objective.x, 0, Objective.z);
                    }
                }

            }
            else if (Objective.x > PreviousObjetive.x)
            {
                if (NextObjective.z > Objective.z)
                {
                    //Debug.Log("giro der arriba");
                    recta = false;
                    inicio = new Vector3(Objective.x - t, 0, Objective.z);
                    final = new Vector3(Objective.x, 0, Objective.z + t);
                    for(int i = 0; i < puntos.Length; i++)
                    {
                        puntos[i] = new Vector3(inicio.x + pasos2[i], 0, inicio.z + pasos1[i]);
                    }
                }
                else if(NextObjective.z < Objective.z)
                {
                    //Debug.Log("giro der abajo");
                    recta = false;
                    inicio = new Vector3(Objective.x - t, 0, Objective.z);
                    final = new Vector3(Objective.x, 0, Objective.z - t);
                    for(int i = 0; i < puntos.Length; i++)
                    {
                        puntos[i] = new Vector3(inicio.x + pasos2[i], 0, inicio.z - pasos1[i]);
                    }
                }
                else
                {
                    //Debug.Log("recta der");
                    recta = true;
                    inicio = new Vector3(Objective.x, 0, Objective.z);
                    final = new Vector3(Objective.x, 0, Objective.z);
                    for(int i = 0; i < puntos.Length; i++)
                    {
                        puntos[i] = new Vector3(Objective.x, 0, inicio.z);
                    }
                }
            }
            else
            {
                //Debug.Log("Quieto");
                recta = false;
                inicio = new Vector3(Objective.x, 0, Objective.z);
                    final = new Vector3(Objective.x, 0, Objective.z);
                    for(int i = 0; i < puntos.Length; i++)
                    {
                        puntos[i] = new Vector3(Objective.x, 0, Objective.z);
                    }
            }
        }

        Vector3[] waypoints;
        if(recta)
        {
            waypoints = new Vector3[3];
        }
        else
        {
            waypoints = new Vector3[12];
        }
        waypoints[0] = inicio;
        waypoints[waypoints.Length-2] = final;
        waypoints[waypoints.Length-1] = NextObjective;
        if(!recta)
        {
            for(int i = 0; i < puntos.Length; i++)
            {
                waypoints[i+1] = puntos[i];
            }
        }
        
        Vector3 target = waypoints[m_CurrentWaypointIndex];
        float step = speed * Time.deltaTime;
        float step2 = 40 * Time.deltaTime;

        //rotacion
        if (!enFormacion) {
            Quaternion rotTarget;

            if (target != inicio)
            {
                Vector3 giro = NextObjective - transform.position;
                if(giro != Vector3.zero)
                    rotTarget = Quaternion.LookRotation(giro);
                else
                    rotTarget = Quaternion.identity;
            }
            else
            {
                Vector3 giro = inicio - transform.position;
                if(giro != Vector3.zero)
                    rotTarget = Quaternion.LookRotation(giro);
                else
                    rotTarget = Quaternion.identity;
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, step2);
        }        

        //movimiento
        if (transform.position != target)
        {
            //Vector3 camino = Objective - transform.position;
            //camino *= Time.deltaTime;
            //camino *= speed;
            //m_Rigidbody.AddForce(camino);
            
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            //Debug.Log("Posicion = " + transform.position);
            return false;
        }
        else
        {
            m_CurrentWaypointIndex++;
            if(m_CurrentWaypointIndex >= (waypoints.Length-1))
            {
                m_CurrentWaypointIndex = 0;
                return true;
            }
            else return false;
        }
    }
}