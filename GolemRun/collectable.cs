using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectable : MonoBehaviour
{
   public GameObject puertaCatacumbas;
   public GameObject puertaZonaIzq;
   public GameObject puertaZonaDer;
 
    
   private void OnDestroy() {
      
      puertaCatacumbas.transform.Rotate(Vector3.forward,160f);
      puertaZonaDer.transform.Rotate(Vector3.forward,-100f);
      puertaZonaIzq.transform.Rotate(Vector3.forward,110f);
   }


}
