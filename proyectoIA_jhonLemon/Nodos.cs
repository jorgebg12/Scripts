 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Nodos : MonoBehaviour
{

    //Distancias[A] es la distancia de este nodo a Vecinos[A]
    public double[] Distancias;

    //
    public double acumulado; //Distancia al proximo nodo(acumulada)
    //Para  a*
    [HideInInspector]public double hValor; //Distancia del nodo al final
    [HideInInspector]public double FValor { get{ return acumulado + hValor;} } //resultado de la formula F=g+h
    ////
    public Nodos[] Vecinos = new Nodos[0];

    [HideInInspector]public Nodos padre = null;
    public Vector3 coordenadas { get; private set; }

    private float xCercana = float.MaxValue;
    private float zCercana = float.MaxValue;

    private List<Vector3> respuestas = new List<Vector3>();

    //Awake utilizado para asignar variables porque si no daba errores de concurrencia
    private void Awake()
    {
        coordenadas = transform.position;
    }

    public void CalcularDistancias()
    {
        Distancias = new double[Vecinos.Length];
        for(int i = 0; i < Vecinos.Length; i++)
        {
            Distancias[i] = Mathf.Sqrt(Mathf.Pow((Vecinos[i].coordenadas.x - coordenadas.x), 2) + Mathf.Pow((Vecinos[i].coordenadas.z - coordenadas.z), 2));
        }
    }

    public void calcularCerca()
    {
        for(int i = 0; i < Vecinos.Length; i++)
        {
            xCercana = xCercana >= (Vecinos[i].coordenadas.x - coordenadas.x) ? Mathf.Abs(Vecinos[i].coordenadas.x - coordenadas.x) : xCercana;
            zCercana = zCercana >= (Vecinos[i].coordenadas.z - coordenadas.z) ? Mathf.Abs(Vecinos[i].coordenadas.z - coordenadas.z) : zCercana;
            //Distancias[i] = Mathf.Sqrt(Mathf.Pow((Vecinos[i].coordenadas.x - coordenadas.x), 2) + Mathf.Pow((Vecinos[i].coordenadas.z - coordenadas.z), 2));
        }
    }

    private void Start()
    {
        
    }

    public void addNeigbor(GameObject vecino)
    {
        if(Vecinos[0] == null)
        {
            Vecinos = new Nodos[]{vecino.GetComponent<Nodos>()};
        }
        else
        {
            Nodos[] nuevosVecinos = new Nodos[Vecinos.Length+1];
            for ( int i = 0; i < Vecinos.Length; i++)
                nuevosVecinos[i] = Vecinos[i];
            nuevosVecinos[nuevosVecinos.Length-1] = vecino.GetComponent<Nodos>();
            Vecinos = nuevosVecinos;
        }
    }

    /*public Nodos[] Buscar(Vector3 objetivo)
    {
        //Si ya estamos buscando el objetivo, cancelamos; si no, buscamos el objetivo
        if(!respuestas.Contains(objetivo))
        {
            respuestas.Add(objetivo);
            return Objetivos(objetivo);
        }
        else
            return null;
    }

    public Nodos[] Objetivos(Vector3 objetivo)
    {
        //El Objetivo está mas cerca que un nodo, con lo que corresponde a este nodo
        if(objetivo.x - coordenadas.x < xCercana && objetivo.z - coordenadas.z < zCercana)
        {
           Nodos[] devolver = new Nodos[1];
           devolver[0] = this;
           //Debug.Log("Objetivo = " + objetivo + "Camino = " + devolver[0].coordenadas);
           respuestas.Remove(objetivo);
           return devolver;
        }
        //El objetivo no está mas cerca que un nodo, con lo que preguntamos a los nodos vecinos
        //Llamamos al algoritmo de búsqueda
        Nodos[] camino = BusquedaAnchura(objetivo);

        Nodos[] nextCamino = new Nodos[camino.Length + 1];
        nextCamino[0] = this;
        for(int i = 0; i < camino.Length; i++)
            nextCamino[i+1] = camino[i];
        respuestas.Remove(objetivo);
        return nextCamino;
    }

    private Nodos[] BusquedaAnchura(Vector3 objetivo)
    {
        Nodos[] camino = new Nodos[0];
        Nodos[] posibleCamino;
        for(int i = 0; i < Vecinos.Length; i++)
        {
            posibleCamino = Vecinos[i].Buscar(objetivo);
            if(posibleCamino != null)
            {
                //Primer camino
                if(camino.Length == 0)
                    camino = posibleCamino;
                //Camino mas corto
                else if(posibleCamino.Length < camino.Length)
                    camino = posibleCamino;
                //Si el camino es igual, hacer cosas (Por ahora elegir aleatoriamente)
                else if(posibleCamino.Length == camino.Length)
                {
                    int numero = Random.Range(0,2);
                    camino = numero < 1 ? posibleCamino : camino;
                }
            }
        }
        return camino;
    }*/
}
