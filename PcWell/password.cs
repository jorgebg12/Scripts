using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class password : MonoBehaviour
{
    public Text textoEntrada;
    public GameObject boton;
    public GameObject cuadroTexto;
    public Animator backgroundAnimator;
    private string contrasena = "qwerty";
    public bool kind;
    public bool completado = true;
    public bool entra = false;
    private CambiarCanvas controlador;
    private GestionaPassword contraseñador;
    private void Start()
    {
        controlador = FindObjectOfType<CambiarCanvas>();
        contraseñador = FindObjectOfType<GestionaPassword>();
        contrasena = contraseñador.contraseña;
    }

    public void checkPassword(){
        completado = false;
        entra = true;
        if (kind) {
            if(textoEntrada.text == contrasena){
                Debug.Log("Contraseña correcta");
                completado = true;
                StartCoroutine(cerrarJuego());
                backgroundAnimator.SetBool("Done", true);
            }
            else{
                Debug.Log("MUMAL 1");
                StartCoroutine(cerrarJuego());
                backgroundAnimator.SetBool("Failed", true);
            }
        }
        else {
            if(textoEntrada.text != contrasena){
                Debug.Log("Contraseña incorrecta");
                completado = true;
                StartCoroutine(cerrarJuego());
                backgroundAnimator.SetBool("Done", true);
            }
            else{
                Debug.Log("MUMAL 2");
                StartCoroutine(cerrarJuego());
                backgroundAnimator.SetBool("Failed", true);
            }
        }

        boton.SetActive(false);
        cuadroTexto.SetActive(false);      
    }
    private void OnDestroy()
    {
        if(!entra && completado && kind)
            controlador.generador.GenerarBarco(Random.Range(2, 4));
        else if( (!completado && kind) || (!completado && !kind))
            controlador.generador.GenerarBarco(Random.Range(2, 4));
        else
            controlador.jugador.ganarDinero(10f);
    }
    IEnumerator cerrarJuego()
    {
        GameObject padre = this.gameObject.transform.parent.parent.gameObject;
        yield return new WaitForSeconds(2f);
        Destroy(padre);
    }
}