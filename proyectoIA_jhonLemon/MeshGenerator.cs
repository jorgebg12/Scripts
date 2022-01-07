using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeshGenerator : MonoBehaviour
{
    private Pathfinding pathfinder;
    public GameObject nodo;
    public GameObject resto;
    private Grid tablero;
    private Queue<Vector3Int> abiertas = new Queue<Vector3Int>();
    private List<Vector3Int> cerradas = new List<Vector3Int>();
    private List<GameObject> puntos = new List<GameObject>();
    private ulong numero;
    void Start()
    {
        pathfinder = gameObject.GetComponent<Pathfinding>();
        tablero = gameObject.GetComponent<Grid>();
        if(generarMatriz())
        resto.active = true;
        numero = 0;
    }

    private bool generarMatriz()
    {
        Vector3Int punto = new Vector3Int(0,0,0);
        GameObject hijo = CrearNodo(punto);
        hijo.name = numero++.ToString();
        AddSides(punto, WallsOpen(hijo));
        cerradas.Add(punto);
        puntos.Add(hijo);
        pathfinder.AddElements(hijo);
        while(abiertas.Count > 0 && Mathf.Abs(punto.x) < 50 && Mathf.Abs(punto.y) < 50)
        {
            punto = abiertas.Dequeue();
            if(!ExisteHijo(punto))
            {
                GameObject son = CrearNodo(punto);
                son.name = numero++.ToString();
                bool[] caminos = WallsOpen(son);
                AddSides(punto, caminos);
                addNeigbor(son, caminos, punto);
                cerradas.Add(punto);
                puntos.Add(son);
                pathfinder.AddElements(son);
            }
        }
        foreach(GameObject go in puntos)
        {
            go.GetComponent<Nodos>().CalcularDistancias();
        }
        foreach(GameObject go in puntos)
        {
            go.GetComponent<Nodos>().calcularCerca();
        }
        return true;
    }

    private bool[] WallsOpen(GameObject nodo)
    {
        RaycastHit golpe;
        bool[] aperturas = new bool[4];
        aperturas[0] = !Physics.Raycast(nodo.transform.position, Vector3.right, out golpe, tablero.cellSize.x);
        aperturas[1] = !Physics.Raycast(nodo.transform.position, Vector3.left, out golpe, tablero.cellSize.x);
        aperturas[2] = !Physics.Raycast(nodo.transform.position, Vector3.forward, out golpe, tablero.cellSize.z);
        aperturas[3] = !Physics.Raycast(nodo.transform.position, Vector3.back, out golpe, tablero.cellSize.z);
        return aperturas;
    }

    //R L U D
    private void AddSides(Vector3Int punto, bool[] aperturas)
    {
        Vector3Int dot;
        dot = new Vector3Int(punto.x + 1, punto.y, punto.z);
        if(aperturas[0] && !ExisteHijo(dot))
            abiertas.Enqueue(dot);
        dot = new Vector3Int(punto.x - 1, punto.y, punto.z);
        if(aperturas[1] && !ExisteHijo(dot))
            abiertas.Enqueue(dot);
        dot = new Vector3Int(punto.x, punto.y + 1 , punto.z);
        if(aperturas[2] && !ExisteHijo(dot))
            abiertas.Enqueue(dot);
        dot = new Vector3Int(punto.x, punto.y - 1, punto.z);
        if(aperturas[3] && !ExisteHijo(dot))
            abiertas.Enqueue(dot);        
    }

    private void addNeigbor(GameObject nodo, bool[] aperturas, Vector3Int punto)
    {
        bool[] muros = WallsOpen(nodo);
        Vector3Int dot;
        dot = new Vector3Int(punto.x +1, punto.y, punto.z);
        if(cerradas.Contains(dot) && muros[0])
        {
            puntos[cerradas.FindIndex(a => a == dot)].GetComponent<Nodos>().addNeigbor(nodo);
            nodo.GetComponent<Nodos>().addNeigbor(puntos[cerradas.FindIndex(a => a == dot)]);
        }
        dot = new Vector3Int(punto.x -1, punto.y, punto.z);
        if(cerradas.Contains(dot) && muros[1])
        {
            puntos[cerradas.FindIndex(a => a == dot)].GetComponent<Nodos>().addNeigbor(nodo);
            nodo.GetComponent<Nodos>().addNeigbor(puntos[cerradas.FindIndex(a => a == dot)]);
        }
        dot = new Vector3Int(punto.x, punto.y + 1, punto.z);
        if(cerradas.Contains(dot) && muros[2])
        {
            puntos[cerradas.FindIndex(a => a == dot)].GetComponent<Nodos>().addNeigbor(nodo);
            nodo.GetComponent<Nodos>().addNeigbor(puntos[cerradas.FindIndex(a => a == dot)]);
        }
        dot = new Vector3Int(punto.x, punto.y - 1, punto.z);
        if(cerradas.Contains(dot) && muros[3])
        {
            puntos[cerradas.FindIndex(a => a == dot)].GetComponent<Nodos>().addNeigbor(nodo);
            nodo.GetComponent<Nodos>().addNeigbor(puntos[cerradas.FindIndex(a => a == dot)]);
        }
    }

    private static bool igual(Vector3Int uno, Vector3Int dos)
    {
        return uno == dos;
    }

    private GameObject CrearNodo(Vector3Int punto)
    {
        GameObject nodus = Instantiate(nodo, tablero.CellToWorld(punto), Quaternion.identity, transform);
        return nodus;
    }

    private bool ExisteHijo(Vector3Int punto)
    {
        return cerradas.Contains(punto);
    }
}