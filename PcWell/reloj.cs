using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class reloj : MonoBehaviour
{
    private int hora = 00;
    private int minutos = 00;
    private float segundos = 00;
    private Text texto;

    private void Awake()
    {
        hora = 00;
        minutos = 00;
        segundos = 00;
    }

    // Start is called before the first frame update
    void Start()
    {
        texto = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        segundos += Time.deltaTime;
        if ( segundos >= 60)
        {
            minutos++;
            if (minutos >= 60)
            {
                hora++;
                if (hora >= 24)
                    hora -= 24;
                minutos -= 60;
            }
            segundos -= 60;
        }
        texto.text = TiempoAguantado();
    }

    string TiempoAguantado()
    {
        string textoHora = (hora < 10) ? "0" + hora : hora.ToString();
        string textoMinutos = (minutos < 10) ? "0" + minutos : minutos.ToString();
        string textoSegundos = (Mathf.Floor(segundos) < 10) ? "0" + Mathf.Floor(segundos) : Mathf.Floor(segundos).ToString();
        return textoHora + ":" + textoMinutos + ":" + textoSegundos;
    }
}
