using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TutoScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Button next;
    public Button prev;
    public TextMeshProUGUI explicacion;
    public List<Image> images;
    private int estado = 0;
    private string[] descripciones = new string[8] {
        "Bienvenid@ al tutorial básico de PCWell,\n Soy Kartana, y seré tu guía por este tutorial.\nPulsa el botón Next para continuar.\n Si ya sabes jugar pulsa back.",
        "Tú misión es proteger los archivos de datos del PC. Enhorabuena, ahora eres un Antivirus :D \nLas carpetas de archivos contienen información valiosa. Si todos los datos son robados, el sistema operativo colapsará.",
        "A lo largo de la partida irán apareciendo PopUps en la pantalla. Cada uno contiene un microjuego. Puedes interactuar con él con el click Izquierdo del ratón. También puedes cerrarlos o minimizarlos\nPara moverte por el entorno 3D in game usa wasd." ,
        "Algunos microjuegos son ataques de hackers escondidos. ¡Si juegas a ellos empezará un ataque! \n \nDeberás determinar que microjuegos están libres de virus. Fíjate en los detalles.",
        "No te preocupes si fallas. No eres la última línea de defensa que tenemos ;) \n \nPuedes comprar unidades de antivirus desde el menú desplegable que tienes en la parte inferior izquierda de la pantalla.",
        "Estas unidades patrullarán el PC y buscarán a los atacantes. Con cada ataque que repelan se volverán más débiles. Incluso puede que no sean capaces de detener una amenaza si son pocos.",
        "A más tengas mejor. El problema es que no son gratis. Cada unidad se compra con BKoins.\n\n¿Y como se consiguen las BKoins? Efectivamente. Ganando microjuegos o cerrando los que contengan virus.",
        "El tutorial ha finalizado. Espero que te haya servido de ayuda. \n\nLa tarea de un antivirus no tiene fin, al igual que esta simulación. Trata de aguantar todo lo que puedas \n\nSuerte ^^"


    };
    //Sonido
    public AudioSource buttonSound;

    void Start()
    {
        prev.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        cambiarCosas();
    }
    void cambiarCosas(){

        if(estado>0){
            prev.enabled=true;
            prev.gameObject.SetActive(true);
        }
        else{
            prev.enabled=false;
            prev.gameObject.SetActive(false);
        }
        if(estado>6){
            next.enabled=false;
            next.gameObject.SetActive(false);
        }
        else{
            next.enabled=true;
            next.gameObject.SetActive(true);
        }
        /*switch(estado){
            case <9:
            break;
        }*/
        explicacion.text =descripciones[estado];
        for(int i = 0; i<8; i++){
            if(i == estado){
                images[i].enabled=true;
            }
            else{
                images[i].enabled=false;
            }
        }
    }
    public void siguiente(){
        buttonSound.Play(0);
        if(estado<7){
            estado++;
            Debug.Log(estado);
        }
    }
    public void anterior(){
        buttonSound.Play(0);
        if(estado>0){
            estado--;
        }
        Debug.Log(estado);
    }
}
