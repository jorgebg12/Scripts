using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pathfinding : MonoBehaviour
{

    public Nodos[] grafo;

    public void AddElements(GameObject elemento)
    {
        if(grafo == null)
        {
            grafo = new Nodos[]{elemento.GetComponent<Nodos>()};
        }
        else
        {
            Nodos[] nuevoGrafo = new Nodos[grafo.Length+1];
            for ( int i = 0; i < grafo.Length; i++)
                nuevoGrafo[i] = grafo[i];
            nuevoGrafo[nuevoGrafo.Length-1] = elemento.GetComponent<Nodos>();
            grafo = nuevoGrafo;
        }
    }

    public Nodos[] Dijkstra(Nodos inicio, Nodos final)
    {
        if(inicio == final) return new Nodos[]{inicio};

        inicializarNodos();

        List<Nodos> definitivos = new List<Nodos>();
        List<Nodos> candidatos = new List<Nodos>();

        candidatos.Add(inicio);

        Nodos actual;
        double posibleMin=double.MaxValue;

        while(candidatos.Count > 0){//Mientras exista un nodo que no es definitivo

            candidatos=candidatos.OrderBy(x=>x.acumulado).ToList();//Se ordena para escoger el que tiene una distancia acumulada menor
            actual = candidatos[0];
            definitivos.Add(actual);//Lo tomamaos como definitivo

            if(actual == final) break;

            for(int n = 0; n<actual.Vecinos.Length ; n++){// Recorremos todos los vecinos del nodo escogido

                if(!definitivos.Contains(actual.Vecinos[n])){//Si ese vecino ya se ha considerado definitivo lo ignoramos

                    if(candidatos.Contains(actual.Vecinos[n])){//Si no es definitivo, pero ya se habia calculado una distancia acumulada para el, lo volvemos a evaluar por si se puede mejorar
                
                        posibleMin=actual.acumulado + actual.Distancias[n];

                        if(posibleMin < actual.Vecinos[n].acumulado){//Si se mejora la distancia, cambiamos la acumulada y su padre

                            actual.Vecinos[n].acumulado=posibleMin;
                            actual.padre=actual;
                            posibleMin=double.MaxValue;
                        }
                    }
                    else{//Si aun no se ha accedido, lo añadimos a posibles candidatos, cambiandole la distancia acumulada y su padre

                        actual.Vecinos[n].acumulado=actual.acumulado + actual.Distancias[n];
                        actual.Vecinos[n].padre=actual;
                        candidatos.Add(actual.Vecinos[n]);

                    }
                }
            }

            candidatos.Remove(actual);//Se elimina el nodo actual

    
        }
        /*Para ver los padres de cada nodo Visitado
        foreach (Nodos item in definitivos)
        {
            Debug.Log("El nodo : " + item.name + " Su padre es : " + item.padre);
        }*/


        return ObtenerListaFinal(inicio,final);

    }
    public Nodos[] Aestrella(Nodos inicio, Nodos final)
    {
        if(inicio == final) return new Nodos[]{inicio};

        inicializarNodos();

        List<Nodos> definitivos = new List<Nodos>();
        List<Nodos> candidatos = new List<Nodos>();

        candidatos.Add(inicio);

        Nodos actual;
        double posibleMin=double.MaxValue;

        while(candidatos.Count > 0){//Mientras exista un nodo que no es definitivo

            candidatos=candidatos.OrderBy(x=>x.FValor).ToList();//Se ordena para escoger el que tiene una distancia acumulada menor
            actual = candidatos[0];
            definitivos.Add(actual);//Lo tomamaos como definitivo

            if(actual == final) break;

            for(int n = 0; n<actual.Vecinos.Length ; n++){// Recorremos todos los vecinos del nodo escogido

                if(!definitivos.Contains(actual.Vecinos[n])){//Si ese vecino ya se ha considerado definitivo lo ignoramos

                    if(candidatos.Contains(actual.Vecinos[n])){//Si no es definitivo, pero ya se habia calculado una distancia acumulada para el, lo volvemos a evaluar por si se puede mejorar
                
                        posibleMin=actual.acumulado + actual.Distancias[n];

                        if(actual.FValor < actual.Vecinos[n].FValor){//Si se mejora la distancia, cambiamos la acumulada y su padre

                            actual.Vecinos[n].acumulado=posibleMin;
                            actual.padre=actual;
                            posibleMin=double.MaxValue;
                        }
                    }
                    else{//Si aun no se ha accedido, lo añadimos a posibles candidatos, cambiandole la distancia acumulada y su padre

                        actual.Vecinos[n].acumulado=actual.acumulado + actual.Distancias[n];
                        actual.Vecinos[n].padre=actual;
						actual.Vecinos[n].hValor= Mathf.Sqrt(Mathf.Pow((final.coordenadas.x - actual.Vecinos[n].coordenadas.x), 2) + Mathf.Pow((final.coordenadas.z - actual.Vecinos[n].coordenadas.z), 2));
                        candidatos.Add(actual.Vecinos[n]);

                    }
                }
            }

            candidatos.Remove(actual);//Se elimina el nodo actual

    
        }
        /*Para ver los padres de cada nodo Visitado
        foreach (Nodos item in definitivos)
        {
            Debug.Log("El nodo : " + item.name + " Su padre es : " + item.padre);
        }*/


        return ObtenerListaFinal(inicio,final);

    }

    private Nodos[] ObtenerListaFinal(Nodos inicial,Nodos fin)
    {
        Nodos actual = fin;
        List<Nodos> final = new List<Nodos>();
        
        while(actual != null){//Se recorre de final a inicio para obtener la lista de nodos definitiva 
            final.Add(actual);
            actual = actual.padre;
        }
        final.Reverse();
        Nodos[] finalArray = final.ToArray();

        //Debug///
        string resultado = "El camino seleccionado es : [ ";
        foreach (Nodos item in finalArray)
        {
            resultado+= item.name + " , ";
        }
        resultado += " ]";
        Debug.Log(resultado);
        ///////

        return finalArray;
    }

    private void inicializarNodos(){

        foreach (Nodos nodo in grafo)
        {
            nodo.acumulado=0;
            nodo.padre=null;
            nodo.hValor=0;
        }
    }

    public Nodos buscarNodoCercano(Vector3 posicion){

        Nodos actual= grafo[0];

        double distanciaMinima=double.MaxValue;
        double posibleMinima;

        foreach (Nodos nodo in grafo)
        {
            posibleMinima=Mathf.Sqrt(Mathf.Pow((posicion.x - nodo.coordenadas.x), 2) + Mathf.Pow((posicion.z - nodo.coordenadas.z), 2));

            if(posibleMinima < distanciaMinima){

                distanciaMinima=posibleMinima;
                actual=nodo;
            }
        }
        return actual;
    }
}
