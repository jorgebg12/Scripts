using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreation : MonoBehaviour
{

    GM gm;

    public Button player1openButton;
    public Button player2openButton;

    public GameObject player1Menu;
    public GameObject player2Menu;

    public GameObject player1Units;
    public GameObject player2Units;

    public GameObject player1Structures;
    public GameObject player2Structures;

    public Pathfinding pathfinding;
    public List<Tile> listaCreatable;

    private void Start()
    {
        gm = FindObjectOfType<GM>();
        pathfinding = GetComponent<Pathfinding>();
    }

    private void Update()
    {
        if (gm.playerTurn == 1)
        {
            player1openButton.interactable = true;
            player2openButton.interactable = false;
        }
        else
        {
            player2openButton.interactable = true;
            player1openButton.interactable = false;
        }
    }

    public void ToggleMenu(GameObject menu) {
        menu.SetActive(!menu.activeSelf);
    }

    public void CloseCharacterCreationMenus() {
        player1Menu.SetActive(false);
        player2Menu.SetActive(false);
    }

    public void BuyUnit (Unit unit) {

        if (unit.playerNumber == 1 && unit.cost <= gm.player1Gold)
        {
            player1Menu.SetActive(false);
            gm.player1Gold -= unit.cost;
        } else if (unit.playerNumber == 2 && unit.cost <= gm.player2Gold)
        {
            player2Menu.SetActive(false);
            gm.player2Gold -= unit.cost;
        } else {
            print("NOT ENOUGH GOLD, SORRY!");
            return;
        }

        gm.UpdateGoldText();
        gm.createdUnit = unit;
        gm.chosenTeam = unit.playerNumber;

        DeselectUnit();
        SetCreatableTiles(unit.playerNumber, false);
    }

    public void BuyVillage(Village village) {
        if (village.playerNumber == 1 && village.cost <= gm.player1Gold)
        {
            player1Menu.SetActive(false);
            gm.player1Gold -= village.cost;
        }
        else if (village.playerNumber == 2 && village.cost <= gm.player2Gold)
        {
            player2Menu.SetActive(false);
            gm.player2Gold -= village.cost;
        }
        else
        {
            print("NOT ENOUGH GOLD, SORRY!");
            return;
        }
        gm.UpdateGoldText();
        gm.createdVillage = village;
        gm.chosenTeam = village.playerNumber;

        DeselectUnit();
        Debug.Log("ESTOY AQUI");
        SetCreatableTiles(village.playerNumber, true);

    }

    void SetCreatableTiles(int jugador, bool edificio) {
        gm.ResetTiles();
        listaCreatable = null;
        List<Tile> lista = new List<Tile>();

        //Colocar unidades
        //Jugador 1
        if (jugador == 1)
        {
            foreach (Transform hijo in player1Structures.transform)
            {
                Tile casilla = gm.tilePos[hijo.position];
                foreach (Tile tile in casilla.vecinos)
                    lista.Add(tile);
            }
        }
        //Jugador 2
        else
        {
            foreach (Transform hijo in player2Structures.transform)
            {
                Tile casilla = gm.tilePos[hijo.position];
                foreach (Tile tile in casilla.vecinos)
                    lista.Add(tile);
            }
        }
        //Colocar edificios
        if (edificio)
        {
            //Jugador 1
            if (jugador == 1)
            {
                foreach (Transform hijo in player1Units.transform)
                {
                    Tile casilla = gm.tilePos[hijo.position];
                    foreach (Tile tile in casilla.vecinos)
                        lista.Add(tile);
                }
            }
            //Jugador 2
            else
            {
                foreach (Transform hijo in player2Units.transform)
                {
                    Tile casilla = gm.tilePos[hijo.position];
                    foreach (Tile tile in casilla.vecinos)
                        lista.Add(tile);
                }
            }
        }
        listaCreatable = lista;
        foreach(Tile tile in lista)
        {
            if (tile.isClear())
            {
                tile.SetCreatable();
            }
        }

        //Tile[] tiles = FindObjectsOfType<Tile>();
        //foreach (Tile tile in tiles)
        //{
        //    if (tile.isClear())
        //    {
        //        tile.SetCreatable();
        //    }
        //}
    }

    void DeselectUnit() {
        if (gm.selectedUnit != null)
        {
            gm.selectedUnit.isSelected = false;
            gm.selectedUnit = null;
        }
    }




}
