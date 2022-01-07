using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class parpadea : MonoBehaviour
{
    // Start is called before the first frame update
    private float tiempo = 0.0f;
    public TextMeshProUGUI gameOver;
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        tiempo += Time.deltaTime;
        if(tiempo >= 0.8f){
            if(gameOver.enabled == false){
                gameOver.enabled = true;
            }
            else{
                gameOver.enabled = false;
            }
            tiempo =0f;
        }
    }
}
