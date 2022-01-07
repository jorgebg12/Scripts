using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class palanca : MonoBehaviour
{

    public GameObject statue;
    public Vector3 desiredRotation;
    private Animator anim;

    public puzzleEstatuas scriptPuzzle;
    


    void Start() {

        anim = this.GetComponent<Animator>();
        
    }
    void Update()
    {

       
        
    }

    public void startRotation(){//Animation Event, se llama cuando acaba la animacion de la palanca

      if(scriptPuzzle.resuelto==false){

        StartCoroutine(rotateStatue());

      }

    }
    

   IEnumerator rotateStatue(){

      Vector3 origen = statue.transform.rotation.eulerAngles;
      Vector3 destino = origen + (Vector3.up * 90);


      Debug.Log(destino);

    float elapsed = 0.0f;

     while( elapsed < 1f)
     {

      statue.transform.Rotate(Vector3.up, Time.deltaTime *90.0f );
      elapsed += Time.deltaTime;
      yield return null;

     }

     Vector3 arreglo = statue.transform.eulerAngles;
      
      arreglo.y = Mathf.Round(arreglo.y / 90) * 90;


      statue.transform.eulerAngles = arreglo;


      checkCorrectPosition();
      scriptPuzzle.checkSolved();   
   }
   
   public void checkCorrectPosition(){

      if(statue.transform.eulerAngles == desiredRotation){

          GameObject vela = statue.transform.Find("vela").gameObject;
          GameObject luz = vela.transform.Find("Light").gameObject;

          luz.SetActive(true);

        }
        if(statue.transform.eulerAngles != desiredRotation){

          GameObject vela = statue.transform.Find("vela").gameObject;
          GameObject luz = vela.transform.Find("Light").gameObject;

          luz.SetActive(false);

        }

   }

   
}
