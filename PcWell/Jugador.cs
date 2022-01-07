using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Jugador : MonoBehaviour
{
    public TextMeshProUGUI dineroUI;
    private float dineros;

    public TextMeshProUGUI vidaUI;
    public float vida;

    public GameObject escenario;
    public Canvas mainCanvas;
    public Canvas gameOverCanvas;

    //sonido
    public AudioSource winMoney;

    private void Awake()
    {
        dineros = 0;
        gameOverCanvas.gameObject.SetActive(false);
    }

    private void Start()
    {
        dineroUI.text = dineros.ToString();
        foreach(Transform objeto in escenario.transform)
        {
            if(objeto.gameObject.layer == 6)
            {
                UpdateHP(objeto.gameObject.GetComponent<Objetivo>().HP);
            }
        }
    }


    public float pagar(float precio)
    {
        float cantidad = 0;
        cantidad = Mathf.Clamp(precio, 0f, dineros);
        return cantidad;
    }

    public bool comprarUnidad(GameObject unidad, float precio)
    {
        if (pagar(precio) < precio)
        {
            //Se puede enviar un mensaje al hud de poco dinero
            return false;
        }
        else
        {
            dineros -= precio;
            dineroUI.text = dineros.ToString();
            return true;
        }
    }

    public void ganarDinero(float dinero)
    {
        winMoney.Play(0);
        dineros += dinero;
        dineroUI.text = dineros.ToString();
    }

    public void UpdateHP(float cantidad)
    {
        vida += cantidad;
        vidaUI.text = ((int)vida).ToString();
        Debug.Log(vida);
        perder();
    }
    public void perder(){
        if(vida<=0){
            Debug.Log("entra en perder");
            gameOverCanvas.gameObject.SetActive(true);
            mainCanvas.gameObject.SetActive(false);
        }
    }
}
