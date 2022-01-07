using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeneradorMalo : MonoBehaviour
{
    public TextMeshProUGUI portada;

    public Text contenido;
    public GameObject padre;
    private CambiarCanvas controlador;

    private string[] portadas = new string[5] {
        "De: estoesmuysegurolojuro@hmmmMail.pen \n Para: Tudireccion@Hemail.com \n \n Asunto: Dinero",
        "De: correomuycaro9982658@Nigger.blk \n Para: MuchaGente@Hemail.com \n \n Asunto: Soy un principe Nigeriano",
        "De: scammer12437451634829@scam.scm \n Para: Tudireccion@Hemail.com \n \n Asunto: Necesito tu ayuda",
        "De: CriptoBro.CriptoCurrencyFTW@NotFungible.NFT \n Para: TuDirección@Hemail.com \n \n Asunto: ¡Compra Criptomonedas!",
        "De: AlbinoOnline@gamail.dot \n Para: TuDirección@Hemail.com \n \n Asunto: AlbinoOnline"

    };

    private string[] contenidos = new string[5]{
        "\n Hola, [Usuario] soy tu amigo Mahakala. Hoy me he levantado con ganas de recompensarte por nuestra amistad, con lo que te quiero dar 69420$ solamente a tí. \n Porfabor, dame tu numero de cuenta y la contraseña \n Atentamente, tu amigo Mahalaka",
        "\n Hola, Amigo soy el principe Nigeriano Abayomrunkoje.\n Como acabo de recibir mi herencia, quiero regalar una pequeña cantidad de 999999999$ a toda la gente que conteste mi correo indicando su número de cuenta y contraseña.",
        "\n Hola, soy Jeff Gates y te estoy escribiendo desde mi cuenta Terciaria, para decirte que me han pillado con las manos en la masa y necesito deshacerme de algunos millones, Si me pasas tu número de cuenta y contraseña te los puedo pasar para que te los quedas. \n Confia en mí, soy el real. El nombre de mi mujer es Melinda Bezos",
        "\n HOla [Usuario]. Es el momento ideal para empezar a invertir en la criptomoneda burroCoin. ¡Está subiendo como la espuma! Vas a ganar muchísimo dinero en muy poco tiempo. Además sin ningún tipo de riesgo. ¡Todo son ventajas! \n Así que no esperes más y empieza a invertir hoy en burroCoin. No seas Burro ;p",
        "\n AlbinoOnline es un mmorpg no lineal, en el que escribes tu propia historia sin limitarte a seguir un camino prefijado. Explora un amplio mundo abierto con 5 biomas únicos, todo cuánto hagas tendrá su repercusión en el mundo, con la economía orientada al jugador de Albino, los jugadores crean prácticamente todo el equipo a partir de los recursos que consiguen, el equipo que llevas define quién eres, cambia de arma y armadura para pasar de caballero a mago, o juega como una mezcla de ambas clases. Aventúrate en el mundo abierto frente a los habitantes y las criaturas de Albino, inicia expediciones o adéntrate en mazmorras en las que encontrarás enemigos aún más difíciles, enfréntate a otros jugadores en encuentros en el mundo abierto, lucha por los territorios o por ciudades enteras en batallas tácticas, relájate en tu isla privada, donde podrás construir un hogar, cultivar cosechas y criar animales, únete a un gremio, todo es mejor cuando se trabaja en grupo. Adéntrate ya en el mundo de Albino y escribe tu propia historia."
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
