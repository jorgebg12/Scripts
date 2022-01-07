using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class initialMenu : MonoBehaviour
{

    public string escenaInicio;
    public Animator transicion;

    public Animator golem;
    public float timeAnim;
    public void jugar(){

        StartCoroutine(LoadAnimation(escenaInicio));

    }

    public void exit(){

        StartCoroutine(bye());  
        

    }

    IEnumerator LoadAnimation(string Nextlevel)
    {

        
        golem.SetTrigger("atack");    
        yield return new WaitForSeconds(1.2f);
        transicion.SetTrigger("start");
        yield return new WaitForSeconds(timeAnim);

        

        SceneManager.LoadScene(Nextlevel);
    }

    IEnumerator bye()
    {

        golem.SetTrigger("despedida");    
        yield return new WaitForSeconds(2f);
        Application.Quit();
        
    }

}
