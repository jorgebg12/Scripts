using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generarMicrojuego : MonoBehaviour
{

    public GameObject[] minijuegos;
    private GameObject microActual;
    public Canvas canvasMicrojuego;

    public GameObject crono;
    private UITimeBar scriptCrono;
    public float tiempo = 10;

    //El boton de abajo a la izquierda
    public GameObject botonMinimizado;
    public Animator animMinimizado;

    private bool activado = false;

    void Start()
    {
        scriptCrono = crono.GetComponent<UITimeBar>();
        scriptCrono.maxTime = tiempo;
        botonMinimizado.transform.position = new Vector2(Random.Range(botonMinimizado.GetComponent<RectTransform>().rect.width,
                                                         Screen.width- botonMinimizado.GetComponent<RectTransform>().rect.width), botonMinimizado.transform.position.y);
        microActual = Instantiate(minijuegos[Random.Range(0, minijuegos.Length)], canvasMicrojuego.gameObject.transform);
        botonMinimizado.SetActive(true);
        canvasMicrojuego.gameObject.SetActive(false);
        resizeCanvas();
        resizeCrono();
    }

    public void resizeCanvas()
    {
        if (microActual.gameObject.tag == "MicroPequeno")
        {
            canvasMicrojuego.transform.position = new Vector2(Random.Range(canvasMicrojuego.GetComponent<RectTransform>().rect.width / 2, Screen.width - (canvasMicrojuego.GetComponent<RectTransform>().rect.width / 2)), canvasMicrojuego.transform.position.y);
            canvasMicrojuego.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, microActual.GetComponent<RectTransform>().rect.width);
            canvasMicrojuego.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, microActual.GetComponent<RectTransform>().rect.height);
        }
        else
        {
            canvasMicrojuego.transform.position = new Vector2(Screen.width / 2, canvasMicrojuego.transform.position.y);
            canvasMicrojuego.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, microActual.GetComponent<RectTransform>().rect.width);
            canvasMicrojuego.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, microActual.GetComponent<RectTransform>().rect.height);
        }

    }

    public void resizeCrono()
    {
        crono.GetComponent<RectTransform>().SetAsLastSibling();
        //crono.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, crono.GetComponent<RectTransform>().rect.width);
        //crono.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, crono.GetComponent<RectTransform>().rect.height);

    }

    void Update()
    {
        tiempo -= Time.deltaTime;

        if (tiempo <= 0f)
        {
            Generador generador = FindObjectOfType<Generador>();
            generador.GenerarBarco(2);
            Destroy(this.gameObject);
        }
    }
    private void OnDestroy()
    {
        animMinimizado.SetBool("death",true);  
    }
    
    public void cerrarVentana()
    {
        Destroy(this.gameObject);
    }

    public void alternarVentana()
    {
        activado = !activado;

        if (activado)
        {
            this.gameObject.GetComponent<RectTransform>().SetAsLastSibling();
            canvasMicrojuego.gameObject.SetActive(activado);
            scriptCrono.timeLeft = tiempo;
            botonMinimizado.SetActive(false);
        }
        else
        {
            canvasMicrojuego.gameObject.SetActive(activado);
            botonMinimizado.SetActive(true);
        }

    }
}
