using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pathfinding : MonoBehaviour
{

    private int distanciaCelda = 1;
    public List<Tile> Aestrella(Tile inicio, Tile final)
    {
        //Debug.Log("Voy desde : " + inicio.name + "hacia : " + final.name);
        if (inicio == final) return new List<Tile>{inicio};

        List<Tile> definitivos = new List<Tile>();//Los visitados
        List<Tile> candidatos = new List<Tile>();

        InicializarTiles(final);

        candidatos.Add(inicio);

        Tile actual;
        float posibleMin = float.MaxValue;

        while (candidatos.Count > 0)
        {
            candidatos = candidatos.OrderBy(x => x.Fvalor).ToList();
            actual = candidatos[0];
            definitivos.Add(actual);

            if (actual == final) break;

            foreach(Tile vecino in actual.vecinos)
            {
                if (!vecino.isWalkable) continue;//Si el vecino no esta en las posibles casillas

                if (!definitivos.Contains(vecino))
                {
                    if (candidatos.Contains(vecino))//
                    {
                        posibleMin = actual.acumulada + distanciaCelda;

                        if(actual.Fvalor < vecino.Fvalor)
                        {
                            vecino.acumulada = posibleMin;
                            vecino.padre = actual;
                            posibleMin = float.MaxValue;
                        }
                    }
                    else
                    {
                        vecino.acumulada = actual.acumulada + 1;
                        vecino.padre = actual;
                        candidatos.Add(vecino);
                    }

                }
            }
            candidatos.Remove(actual);

        }

        return ObtenerListaFinal(inicio,final);

    }

    private void InicializarTiles(Tile final)
    {
        Tile[] tiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in tiles)
        {
            tile.padre = null;
            tile.Hvalor = Mathf.Sqrt(Mathf.Pow((final.transform.position.x-tile.transform.position.x), 2) + 
                                     Mathf.Pow((final.transform.position.y-tile.transform.position.y), 2));
            tile.acumulada = 0;
        }
    }

    private List<Tile> ObtenerListaFinal(Tile inicial,Tile final)
    {
        Tile actual = final;
        List<Tile> recorrido = new List<Tile>();

        while(actual != null)
        {
            recorrido.Add(actual);
            actual = actual.padre;
        }
        recorrido.Reverse();

        //Debug///
        string resultado = "El camino seleccionado es : [ ";
        foreach (Tile item in recorrido)
        {
            resultado += item.name + " , ";
        }
        resultado += " ]";
        //Debug.Log(resultado);
        ///////

        return recorrido;
    }
}
