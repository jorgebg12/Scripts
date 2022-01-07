using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiarCanvas : MonoBehaviour
{
    
    private Canvas canvasActual;
    public Canvas canvasGrande;
    public GameObject generarJuego;

    //Menu de compra
    public GameObject[] menus;
    public GameObject[] unidades;
    public float[] precios;
    public Jugador jugador;
    private bool abierto;
    public Generador generador;

    //sonido
    public AudioSource generateMicrogameSound;

    private float spawnTime;

    private void Awake()
    {
        abierto = false;
    }

    void Start()
    {
        spawnTime = Random.Range(10f, 15f);
    }
    void Update()
    {
        spawnTime -= Time.deltaTime;

        if (spawnTime <= 0f)
        {
            generateMicrogameSound.Play(0);
            Instantiate(generarJuego, canvasGrande.gameObject.transform);
            spawnTime = Random.Range(10f, 15f);
        }
    }

    public void ToggleCompra()
    {
        if (abierto)
        {
            abierto = false;
            foreach(GameObject menu in menus)
                menu.SetActive(false);
        }

        else
        {
            abierto = true;
            foreach (GameObject menu in menus)
                menu.SetActive(true);
        }
    }

    public void Comprar(int tipo)
    {
        GameObject unit = unidades[tipo];
        float precio = precios[tipo];
        ToggleCompra();
        if(jugador.comprarUnidad(unit, precio))
        {
            Instantiate(unit);
        }
    }

}
