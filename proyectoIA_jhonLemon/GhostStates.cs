using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States
{
    Patrulla,
    Investigar,
    Perseguir,
    Alarma,
    Aturdido,
    Formacion,
    Ayuda
}

public class GhostStates : MonoBehaviour
{
    public Nodos nodoActual;
    public Pathfinding pathfinding;
    //TEST
    public Nodos inicial;
    public Nodos final;
    public float radio = 10;

    public Transform puntoVigilancia;
    public Alarma alarma;

    //
    public States state;
    public GameObject[] fantasmasCercanos;
    private Vector3 origen;
    private Vector3 Destino;
    private GameObject FantasmaObjetivo;
    public Vector3 zonaRefuerzo;
    public Vector3 anguloRefuerzo;
    public AudioSource audioData;

    public int nFantasmasFormacion;
    private GameObject[] fantasmasEnFormacion;
    //Cambiar a privado
    private GhostMovement movimiento;
    //Camino a seguir
    private Nodos[] camino;
    //Datos para patrullas
    public Vector3[] puntosPatrulla;
    private int estadoPatrulla;
    private int estadoCamino;
    private int estadoCaminoPatrulla;
    private bool patrullando;

    private bool enFormacion;
    
    //Investigar
    private bool investigando;
    private Nodos[] caminoInvestigar;
    private int siguienteInvestigar;
    private Nodos puntoFinal;
    private float tiempoInvestigando;
    //Alarma
    private bool alarmado;
    private Nodos[] caminoAlarma;
    private int siguientePuntoAlarma;


    //Perseguir
    private int estadoCaminoPersecucion;
    private float tiempoEspera = 0f;
    private float distanciaEntre;
    private GameObject jugador;


    private void Awake()
    {
        nodoActual = pathfinding.buscarNodoCercano(transform.position);
        jugador= GameObject.Find("JohnLemon");

    }
    // Start is called before the first frame update
    void Start()
    {
        state = States.Patrulla;
        movimiento = gameObject.GetComponent<GhostMovement>();
        estadoPatrulla = 0;
        estadoCamino = 0;
        estadoCaminoPatrulla = 0;
        estadoCaminoPersecucion = 0;
        patrullando = false;
        investigando = false;
        alarmado = false;
        //Asignar los fantasmasCercanos

        fantasmasCercanos = FantasmasCercanos.buscarFantasmasCercanos(transform.position, radio);
        anguloRefuerzo = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (state)
        {
            case States.Patrulla:
                if(patrullando && estadoCaminoPatrulla >= camino.Length-1)
                {
                    estadoPatrulla++;
                    if(estadoPatrulla >= puntosPatrulla.Length)
                        estadoPatrulla = 0;
                    estadoCaminoPatrulla = 0;
                    patrullando = false;
                }
                if (!patrullando)
                {
                    camino = pathfinding.Dijkstra(nodoActual, pathfinding.buscarNodoCercano(puntosPatrulla[estadoPatrulla]));
                    //camino = nodoActual.Buscar(puntosPatrulla[estadoPatrulla].coordenadas);
                    patrullando = true;
                }
                
                //Patrullar

                Vector3 origen;
                Vector3 end;
                if(estadoCaminoPatrulla == 0)
                {
                    origen = nodoActual.coordenadas;
                }
                else
                    origen = camino[estadoCaminoPatrulla - 1].coordenadas;

                if(estadoCaminoPatrulla >= camino.Length-1)
                {
                    end = camino[estadoCaminoPatrulla].coordenadas;
                }
                else
                    end = camino[estadoCaminoPatrulla + 1].coordenadas;
                if(Move(camino[estadoCaminoPatrulla].coordenadas, end, origen))
                {
                    
                    estadoCaminoPatrulla++;
                    nodoActual = camino[estadoCaminoPatrulla];
                    //GhostMovement.CurrentWaypointIndex = 0;
                }
                    
            break;
            case States.Investigar:
                //Ir hacia Destino e investigar área
                //obtener el nodo más cercano a Destino y buscar el camino para llegar
                // dar una vuelta por el sitio y pasar al estado patrulla
                if(!investigando){
                    investigando = true;
                    puntoFinal = pathfinding.buscarNodoCercano(Destino);
                    caminoInvestigar = pathfinding.Dijkstra(nodoActual, puntoFinal);
                    siguienteInvestigar = 1;
                    tiempoInvestigando = 5f;
                    //Debug.LogWarning("Investigando ruido en: " + Destino);
                }
                else {
                    if (nodoActual == puntoFinal){
                        tiempoInvestigando -= Time.deltaTime;
                        if(tiempoInvestigando <= 0f){
                            //volver al estado patrulla
                            state = States.Patrulla;
                            investigando = false;
                            //Debug.LogWarning("Fin de investigacion en: " + this.transform.position);
                        }else{
                            Move(caminoInvestigar[siguienteInvestigar].coordenadas, caminoInvestigar[siguienteInvestigar].coordenadas, nodoActual.coordenadas);
                        }
                    }
                    else if (siguienteInvestigar == caminoInvestigar.Length-1){
                        if(Move(caminoInvestigar[siguienteInvestigar].coordenadas, caminoInvestigar[siguienteInvestigar].coordenadas, nodoActual.coordenadas)){
                            nodoActual = caminoInvestigar[siguienteInvestigar];
                        }
                    }
                    else if (Move(caminoInvestigar[siguienteInvestigar].coordenadas, caminoInvestigar[siguienteInvestigar+1].coordenadas, nodoActual.coordenadas)){
                        nodoActual = caminoInvestigar[siguienteInvestigar];
                        siguienteInvestigar++;
                    }
                }
                
            break;
            case States.Perseguir:

                if(tiempoEspera<=0f){

                    camino=pathfinding.Dijkstra(nodoActual, pathfinding.buscarNodoCercano(jugador.transform.position));
                    estadoCaminoPersecucion=0;
                    distanciaEntre=Mathf.Sqrt(Mathf.Pow((jugador.transform.position.x - nodoActual.coordenadas.x), 2) + Mathf.Pow((jugador.transform.position.z - nodoActual.coordenadas.z), 2));
                    tiempoEspera=6f;
                    if(distanciaEntre < 10f) tiempoEspera=1f;
                }
                tiempoEspera-=Time.deltaTime;

                Vector3 origen2=nodoActual.coordenadas;
                if(camino.Length>2)
                    if(Move(camino[estadoCaminoPersecucion].coordenadas, camino[estadoCaminoPersecucion + 1].coordenadas, origen2))
                    {
                        estadoCaminoPersecucion++;
                        nodoActual = camino[estadoCaminoPersecucion];
                      
                    }
            break;
            case States.Aturdido:
                //No hacer nada
            break;
            case States.Alarma:
                //Debug.Log("PIIIIIIIIIIIIIIIIIIIIIIII (alarma activada)");
                //para eso, obtener el nodo más cercano al destino y buscar un camino hasta él.
                if(!alarmado){
                    alarmado = true;
                    puntoFinal = pathfinding.buscarNodoCercano(Destino);
                    caminoAlarma = pathfinding.Dijkstra(nodoActual, puntoFinal);
                    siguientePuntoAlarma = 0;
                    movimiento.speed *= 2;
                }
                else {
                    if (nodoActual == puntoFinal){
                        //ha llegado al origen de la alarma o a su punto de vigilancia
                    }
                    else if (siguientePuntoAlarma == caminoAlarma.Length-1){
                        if(Move(caminoAlarma[siguientePuntoAlarma].coordenadas, caminoAlarma[siguientePuntoAlarma].coordenadas, nodoActual.coordenadas)){
                            nodoActual = caminoAlarma[siguientePuntoAlarma];
                        }
                    }
                    else if (Move(caminoAlarma[siguientePuntoAlarma].coordenadas, caminoAlarma[siguientePuntoAlarma+1].coordenadas, nodoActual.coordenadas)){
                        nodoActual = caminoAlarma[siguientePuntoAlarma];
                        siguientePuntoAlarma++;
                    }
                }
            break;
            case States.Ayuda:
                if(nFantasmasFormacion >= fantasmasCercanos.Length-1) {
                    camino = pathfinding.Dijkstra(nodoActual, pathfinding.buscarNodoCercano(Destino));
                    crearFormacion(transform.rotation.y);

                    if(estadoCamino >= camino.Length-1)
                    {
                        Levantar(FantasmaObjetivo); 
                        romperFormacion();
                    }

                    //ir hacia el fantasma que necesita ayuda
                    if(estadoCamino == 0)
                    {
                        origen = nodoActual.coordenadas;
                    }
                    else
                        origen = camino[estadoCamino - 1].coordenadas;
                    
                    if(Move(camino[estadoCamino].coordenadas, camino[estadoCamino + 1].coordenadas, origen))
                    {              
                        estadoCamino++;
                        nodoActual = camino[estadoCamino];
                    }
                }
                //esperar a los fantasmas que vienen a hacer formacion
                               
            break;
            case States.Formacion:
                if (enFormacion == true) {
                    Destino = FantasmaObjetivo.transform.position;
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(anguloRefuerzo), 40 * Time.deltaTime);
                    estadoCamino = 0;
                }

                camino = pathfinding.Dijkstra(nodoActual, pathfinding.buscarNodoCercano(Destino));
                
                if(estadoCamino >= camino.Length-1 && enFormacion == false)
                {
                    listoEnFormacion(FantasmaObjetivo);
                    enFormacion = true;                    
                }
                
                if(camino.Length>2) {
                    if(estadoCamino == 0)
                    {
                        origen = nodoActual.coordenadas;
                    }
                    else
                        origen = camino[estadoCamino - 1].coordenadas;

                
                    if(Move(camino[estadoCamino].coordenadas, camino[estadoCamino + 1].coordenadas, origen))
                    {
                        estadoCamino++;
                        nodoActual = camino[estadoCamino];
                    }
                }

            break;
            default:
            break;
        }
    }

    public void UpdateState(States nuevoEstado, Vector3 objetivo)
    {
        if(state < States.Aturdido || (nuevoEstado == States.Alarma && state == States.Patrulla)){
            state = nuevoEstado;
            Destino = objetivo;
        }

    }

    public void UpdateStateAlarma(States nuevoEstado)
    {
        state = nuevoEstado;
        alarmado = false;
        movimiento.speed /= 2;
    }

    public void Ayudar(int profundidad, GameObject fantasmaObjetivo, GameObject[] fantasmasEnJuego)
    {
        //Llamada recursiva para que los mas cercanos (profundidad == 1) ayuden y los siguientes (profundidad == 2) se pongan en alarma
        switch(profundidad)
        {
            case 0:
                if(state != States.Aturdido){
                    state = States.Ayuda;
                    Destino = fantasmaObjetivo.transform.position;
                    FantasmaObjetivo = fantasmaObjetivo;
                    Debug.Log("HE OIDO A UN FANTASMA PIDIENDO AYUDA!");
                    foreach(GameObject fantasmas in fantasmasEnJuego)
                    {
                        Debug.Log("Voy a ayudar a un fantasma, cubridme! (Pide ayuda con prof. 1. Va a ayudar al fantasma caído)");
                        fantasmas.GetComponent<GhostStates>().Ayudar(1, this.gameObject, fantasmasEnJuego);
                    }

                    fantasmasEnFormacion = new GameObject[fantasmasEnJuego.Length - 1];
                    int i = 0;
                    
                    foreach (GameObject fantasma in fantasmasEnJuego) {
                        if (fantasma != fantasmaObjetivo) {
                            fantasmasEnFormacion[i] = fantasma;
                            i++;
                        }
                    }
                }
            break;
            case 1:
                if(state != States.Ayuda && state != States.Aturdido){
                    Debug.Log("Recibido, cubriré al compañero! (Se pone en alerta porque es un fantasma cercano pero no va a ayudar)");
                    state = States.Formacion;
                    Destino = fantasmaObjetivo.transform.position;
                    FantasmaObjetivo = fantasmaObjetivo;
                }
            break;
        }
    }

    public void Caer()
    {
        state = States.Aturdido;

        audioData.Play(0);
        //Funcion en la que se cae el fantasma en una piscina de limón
        fantasmasCercanos = FantasmasCercanos.buscarFantasmasCercanos(transform.position, 100);
        Debug.Log("ME HE CAIDO! (Buscando fantasmas inmediatamente cercanos)");
        GameObject fantasma = fantasmasCercanos[0];
        int i = 0;
        while(fantasma == this.gameObject) 
            fantasma = fantasmasCercanos[++i];
        fantasma.GetComponent<GhostStates>().Ayudar(0, this.gameObject, fantasmasCercanos);
        Debug.Log("he pedido ayuda a " + fantasma.name);
    }

    public void crearFormacion(float anguloJefe)
    {
        float anguloEntreFantasmas = 360 / fantasmasCercanos.Length;
        int i = 0;

        foreach (GameObject fantasma in fantasmasEnFormacion) {
            fantasma.GetComponent<GhostStates>().anguloRefuerzo = new Vector3 (0f, anguloJefe + anguloEntreFantasmas * i, 0f);
            i++;
        }
    }

    public void romperFormacion()
    {
        foreach(GameObject fantasmas in fantasmasEnFormacion) 
            fantasmas.GetComponent<GhostStates>().dejarFormacion();
    }

    public void dejarFormacion()
    {
        state = States.Patrulla;
        estadoCamino = 0;
        enFormacion = false;
    }

    public void Levantar(GameObject fantasmaObjetivo)
    {
        fantasmaObjetivo.GetComponent<GhostStates>().state = States.Patrulla;
    }

    public void listoEnFormacion(GameObject fantasmaObjetivo)
    {
        fantasmaObjetivo.GetComponent<GhostStates>().nFantasmasFormacion++;
    }

    public Vector3 siguienteObjetivoEnPatrulla(){
        return camino[estadoCaminoPatrulla].coordenadas;
    }

    private bool Move(Vector3 objetivo, Vector3 nextobjetivo, Vector3 previousobjetive)
    {
        return movimiento.moveTo(objetivo, nextobjetivo, previousobjetive, enFormacion);
    }
}
