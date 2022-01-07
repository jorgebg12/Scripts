using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void cargarJuego(){
        Debug.Log("cargo");
        SceneManager.LoadScene("PruebaCosas");
    }
    public void cargarTuto(){
        SceneManager.LoadScene("Tutorial");
    }
    public void cargarMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    public void salir(){
        Debug.Log("Salgo");
        Application.Quit();
    }
}
