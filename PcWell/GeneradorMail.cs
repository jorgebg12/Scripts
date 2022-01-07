using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeneradorMail : MonoBehaviour
{
    public TextMeshProUGUI portada;

    public Text contenido;
    public GameObject padre;
    private CambiarCanvas controlador;

    private string[] portadas = new string[5] {
        "De: Capitao Wassap \n Para: TuDirección@Hemail.com \n \n Asunto: Proyecto grupal",
        "De: Profesor Pedra \n Para: TuDirección@Hemail.com \n \n Asunto: Clase de refuerzo",
        "De: Pipo Clown \n Para: TuDirección@Hemail.com \n \n Asunto: Presentación",
        "De: xXNoobMaster69Xx \n Para: TuDirección@Hemail.com \n \n Asunto: Memardo",
        "De: Gigante Noble \n Para: TuDirección@Hemail.com \n \n Asunto: Torneillo CrushRoyale"

    };

    private string[] contenidos = new string[5]{
        "\n Buenas [TuNombre]. Te mando este correo para recordarte que mañana tienes que traer la cartulina con el trabajo. Por favor no te olvides, que la nota de todos depende de ello. Si te olvidas por lo que sea te hacemos pasillito, eh.",
        "\n Saludos [TuNombre]. Debido a tus bajas calificaciones en la asignatura de matemáticas, te recomiendo encarecidamente que acudas a la clase de refuerzo que impartiré el próximo jueves, dia 31 de Febrero a las 5 de la tarde. Si quiere aprobar el trimestre debe ponerse las pilas. Este podría ser el primer paso. \n Atentamente, \n Profesor A. Pedra",
        "\n Hey. Hay que exponer el trabajo ese que hicimos sobre la mitocondria. Te toca explicar la última parte. Nos vemos en clase crack.",
        "\n Mira este tremendo memardo jajgajkfakfjkafjkafkjskgjalgalfjkawjidw",
        "\nHola xXTremendoProasoXx. Este fin de semana hay torneo de CrushRoyale. Necesitamos a todos los miembros del clan y tú eres de los que más vicia. ¿Vas a poder venir? Porfa di que sí."
    };
    // Start is called before the first frame update
    private void Start()
    {
        controlador = FindObjectOfType<CambiarCanvas>();
    }
    void Awake(){
        
        int randomNum = Random.Range(0,5);
        Debug.Log(randomNum);
        portada.text = portadas[randomNum];
        contenido.text = contenidos[randomNum];
    }

    public void clickIncorrecto()
    {
        GameObject padre = this.gameObject.transform.parent.parent.parent.gameObject;
        controlador.generador.GenerarBarco(Random.Range(2, 4));
        Debug.Log(padre.name);
        Destroy(padre);
    }
    public void clickCorrecto()
    {
        GameObject padre = this.gameObject.transform.parent.parent.parent.gameObject;
        Debug.Log(padre.name);
        controlador.jugador.ganarDinero(10f);
        Destroy(padre);
    }
}
