using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer rend;
    public Color highlightedColor;
    public Color creatableColor;

    public LayerMask obstacles;

    public bool isWalkable;
    public bool isCreatable;

    private GM gm;
    private CharacterCreation creador;

    public float amount;
    private bool sizeIncrease;

	private AudioSource source;
    [HideInInspector]public int distanciaAcum;
    //Pathfinding
    [HideInInspector]public List<Tile> vecinos;
    [HideInInspector]public Tile padre;
    [HideInInspector]public float acumulada;
    [HideInInspector]public float Hvalor;//Distancia al final
    [HideInInspector] public float Fvalor { get { return acumulada + Hvalor; } }
    //
    private void Awake()
    {
        vecinos = new List<Tile>();
        padre = null;
    }
    private void Start()
    {
		source = GetComponent<AudioSource>();
        gm = FindObjectOfType<GM>();
        creador = FindObjectOfType<CharacterCreation>();
        rend = GetComponent<SpriteRenderer>();

    }
    /// <summary>
    /// Busca los que estan alrededor y los añade a la lista de vecinos
    /// </summary>
    public void getVecinos()
    {
        Collider2D[] detectados = Physics2D.OverlapBoxAll(this.transform.position, this.transform.localScale *20, 0f, LayerMask.GetMask("Tile", "Water"), -1f, 1f);
        
        string v = "SOY :"+this.name + " Vecinos : ";
        foreach (Collider2D coll in detectados)
        {
            if (coll.name != this.name && (coll.transform.position.x==this.transform.position.x || coll.transform.position.y == this.transform.position.y))
            {
                v += coll.name + " ,";
                Tile tileFromCollider = coll.gameObject.GetComponent<Tile>();
                vecinos.Add(tileFromCollider); 
            }
        }
        //Debug.Log(v);
 
    }

    public bool isClear() // does this tile have an obstacle on it. Yes or No?
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 0.2f, obstacles);

        if (col != null)
        {
            return false;

        }else if (this.gameObject.layer == 4)
        {
            if (gm.selectedUnit != null)
            {
                if (gm.selectedUnit.claseTropa == ClaseUnidad.murcielago)
                    return true;
                else
                    return false;
            }
            return false;
        }
        else
            return true;
    }

    public void Highlight() {
		
        rend.color = highlightedColor;
        isWalkable = true;
    }

    public void Reset()
    {
        rend.color = Color.white;
        distanciaAcum = 0;
        isWalkable = false;
        isCreatable = false;
    }

    public void SetCreatable() {
        if(gm.playerTurn==2)
            rend.color = creatableColor;
        isCreatable = true;
    }

    private void OnMouseDown()
    {
        if (isWalkable == true) {

            Debug.Log("Estoy en el pathfinding");
            Vector2 posi = gm.selectedUnit.transform.position;
            Tile posicionEnem = gm.tilePos[posi];//Tile del npc inicialmente
            List<Tile> caminoAseguir = gm.pathfinding.Aestrella(posicionEnem, this);
            
            gm.selectedUnit.followPath(caminoAseguir);
            

        } else if (isCreatable == true && gm.createdUnit != null) {
            Unit unit;
            if(gm.chosenTeam == 1)
                unit = Instantiate(gm.createdUnit, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity, creador.player1Units.transform);
            else
                unit = Instantiate(gm.createdUnit, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity, creador.player2Units.transform);
            unit.hasMoved = true;
            unit.hasAttacked = true;
            gm.ResetTiles();
            gm.createdUnit = null;

        //Pueblo
        } else if (isCreatable == true && gm.createdVillage != null) {
            Village pueblo;
            if (gm.chosenTeam == 1)
                pueblo = Instantiate(gm.createdVillage, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity, creador.player1Structures.transform);
            else
                pueblo = Instantiate(gm.createdVillage, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity, creador.player2Structures.transform);
            gm.ResetTiles();
            gm.createdVillage = null;

            if (TownMaker(pueblo))
            {
                pueblo.isVillage = true;
            }

        }
    }

    //Hacia la izquierda no va
    private bool TownMaker(Village pueblo)
    {
        Village[,] arrayCasas = new Village[4,4];
        Village[] casas = AdjacentHouses();
        if (casas != null)
        {
            //Creamos array 2D con todas las casas vecinas a nuestros vecinos
            for (int i = 0; i < casas.Length; i++)
            {
                if (casas[i] != null)
                {
                    Village[] otras = gm.tilePos[casas[i].transform.position].AdjacentHouses();
                    for (int j = 0; j < otras.Length; j++)
                        arrayCasas[i, j] = otras[j];
                }
                else
                {
                    for (int j = 0; j < arrayCasas.GetLength(1); j++)
                        arrayCasas[i, j] = null;
                }
            }
            //Navegamos dicho array buscando casas que coinciden
            for (int i = 0; i < arrayCasas.GetLength(0); i++)
            {
                //Navegamos los vecinos a comprovar
                for (int j = 0; j < arrayCasas.GetLength(1); j++)
                {
                    //Hay casa ahí?
                    if (arrayCasas[i, j] != null)
                    {
                        //Navegamos los otros vecinos para buscar coincidencias
                        for (int a = 0; a < arrayCasas.GetLength(0); a++)
                        {
                            for (int b = 0; b < arrayCasas.GetLength(1); b++)
                            {
                                //Hay casa ahí?
                                if (arrayCasas[a, b] != null)
                                {
                                    Debug.Log("Yo soy " + this + " Comparando " + gm.tilePos[arrayCasas[i, j].transform.position] + " con " + gm.tilePos[arrayCasas[a, b].transform.position]);
                                    Debug.Log("Comparando los puntos: " + i + "-" + j + " " + a + "-" + b);
                                    //Mirar si la casa es igual
                                    if (gm.tilePos[arrayCasas[i, j].transform.position] != this && (a != i || b != j) && (arrayCasas[i, j] == arrayCasas[a, b]))
                                    {
                                        pueblo.miembros = new Village[4] {pueblo, casas[i], casas[a], arrayCasas[i,j]};
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    public Village[] AdjacentHouses()
    {
        Village[] arrayCasas = new Village[4];
        for(int i = 0; i < vecinos.Count; i++)
        {
            Village house = GetHouse(vecinos[i]);
            arrayCasas[i] = house;
        }
        return arrayCasas;
    }

    private Village GetHouse(Tile tile)
    {
        Collider2D col = Physics2D.OverlapCircle(tile.transform.position, 0.2f, obstacles);
        if (col != null)
        {
            Village aldea = col.GetComponent<Village>();
            return aldea;
        }
        return null;
    }

    private void OnMouseEnter()
    {
        if (isClear() == true) {
			//source.Play();
			sizeIncrease = true;
            transform.localScale += new Vector3(amount, amount, amount);
        }
        
    }

    private void OnMouseExit()
    {
        if (isClear() == true)
        {
            sizeIncrease = false;
            transform.localScale -= new Vector3(amount, amount, amount);
        }

        if (isClear() == false && sizeIncrease == true) {
            sizeIncrease = false;
            transform.localScale -= new Vector3(amount, amount, amount);
        }
    }
}
