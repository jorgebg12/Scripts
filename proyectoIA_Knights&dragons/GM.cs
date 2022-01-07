using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    public Unit selectedUnit;

    public int playerTurn = 2;

    public Transform selectedUnitSquare;


    private Animator camAnim;
    public Image playerIcon; 
    public Sprite playerOneIcon;
    public Sprite playerTwoIcon;

    public GameObject unitInfoPanel;
    public Vector2 unitInfoPanelShift;
    Unit currentInfoUnit;
    public Text heathInfo;
    public Text attackDamageInfo;
    public Text armorInfo;
    public Text defenseDamageInfo;

    public int player1Gold;
    public int player2Gold;

    public List<GameObject> darkUnits = new List<GameObject>();
    public List<GameObject> unactive = new List<GameObject>();

    public Text player1GoldText;
    public Text player2GoldText;

    public Unit createdUnit;
    public Village createdVillage;
    public int? chosenTeam;

    public GameObject blueVictory;
    public GameObject darkVictory;

    public Pathfinding pathfinding;

	private AudioSource source;
    //Condition Manager
    public Unit murcielago;
    public Unit caballero;
    public Unit arquero;
    public Village estructura;
    public Unit castilloEnemigo;

    [HideInInspector] public Village undefendedVillage;
    [HideInInspector] public Unit closeVillage;
    //
    [HideInInspector] public Dictionary<Vector2, Tile> tilePos;

    private void Start()
    {
        tilePos = new Dictionary<Vector2, Tile>();
		source = GetComponent<AudioSource>();
        camAnim = Camera.main.GetComponent<Animator>();
        
        asignarVecinos();
        Invoke("EndTurn", 0.005f);
        //EndTurn();
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("b")) && playerTurn == 2) {
            EndTurn();
        }

        if (selectedUnit != null) // moves the white square to the selected unit!
        {
            selectedUnitSquare.gameObject.SetActive(true);
            selectedUnitSquare.position = selectedUnit.transform.position;
        }
        else
        {
            selectedUnitSquare.gameObject.SetActive(false);
        }

    }

    void asignarVecinos()
    {
        Tile[] tiles = FindObjectsOfType<Tile>();
        Debug.Log("Num tiles " + tiles.Length);
        int s = 1;
        foreach (Tile tile in tiles)
        {
            tile.name = "Tile" + s++;
            tile.getVecinos();
            tilePos.Add(tile.transform.position, tile);
        
        }
    }

    // Sets panel active/inactive and moves it to the correct place
    public void UpdateInfoPanel(Unit unit) {

        if (unit.Equals(currentInfoUnit) == false)
        {
            unitInfoPanel.transform.position = (Vector2)unit.transform.position + unitInfoPanelShift;
            unitInfoPanel.SetActive(true);

            currentInfoUnit = unit;

            UpdateInfoStats();

        } else {
            unitInfoPanel.SetActive(false);
            currentInfoUnit = null;
        }

    }

    // Updates the stats of the infoPanel
    public void UpdateInfoStats() {
        if (currentInfoUnit != null)
        {
            attackDamageInfo.text = currentInfoUnit.attackDamage.ToString();
            defenseDamageInfo.text = currentInfoUnit.defenseDamage.ToString();
            armorInfo.text = currentInfoUnit.armor.ToString();
            heathInfo.text = currentInfoUnit.health.ToString();
        }
    }

    /*public void UpdateDarkUnits(bool eliminar, Unit unidad){
        if(eliminar){
            //elimina a la unidad
            darkUnits.Remove(unidad.gameObject);
        }
        else{
            darkUnits.Add(unidad.gameObject);
        }
    }*/

    public void UpdateDarkUnits(Unit[] units){
        darkUnits.Clear();
        foreach (Unit unit in units) {
            if (unit.playerNumber == 1 && unit.claseTropa < ClaseUnidad.edificio) {
                darkUnits.Add(unit.gameObject);
            }
        }
    }

    public void DestroyUnactiveUnits(){
        while (unactive.Count > 0){
            GameObject deadUnit = unactive[0];
            unactive.Remove(deadUnit);
            Destroy(deadUnit);
        } 
        Debug.Log(unactive.Count);
    }

    // Moves the udpate panel (if the panel is actived on a unit and that unit moves)
    public void MoveInfoPanel(Unit unit) {
        if (unit.Equals(currentInfoUnit))
        {
            unitInfoPanel.transform.position = (Vector2)unit.transform.position + unitInfoPanelShift;
        }
    }

    // Deactivate info panel (when a unit dies)
    public void RemoveInfoPanel(Unit unit) {
        if (unit.Equals(currentInfoUnit))
        {
            unitInfoPanel.SetActive(false);
			currentInfoUnit = null;
        }
    }

    public void ResetTiles() {
        Tile[] tiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in tiles)
        {
            tile.Reset();
        }
    }

    void EndTurn() {
		source.Play();
        camAnim.SetTrigger("shake");

        // deselects the selected unit when the turn ends
        if (selectedUnit != null) {
            selectedUnit.ResetWeaponIcon();
            selectedUnit.isSelected = false;
            selectedUnit = null;
        }

        ResetTiles();
        DestroyUnactiveUnits();

        Unit[] units = FindObjectsOfType<Unit>();
        foreach (Unit unit in units) {
            unit.hasAttacked = false;
            unit.hasMoved = false;
            unit.ResetWeaponIcon();
        }
        
        foreach (GameObject gameObject in unactive)
        {
            Destroy(gameObject);
        }

        if (playerTurn == 1) {
            playerIcon.sprite = playerTwoIcon;
            playerTurn = 2;
        } else if (playerTurn == 2) {
            playerIcon.sprite = playerOneIcon;
            playerTurn = 1;
            UpdateDarkUnits(units);
            PerformAITurn();            
        }

        GetGoldIncome(playerTurn);
        GetComponent<CharacterCreation>().CloseCharacterCreationMenus();
        createdUnit = null;
        createdVillage = null;
        chosenTeam = null;
        undefendedVillage = null;
    }

    void PerformAITurn() {
        StartCoroutine(AITurn());
    }

    IEnumerator AITurn()
    {
        foreach (GameObject darkUnit in darkUnits) {
            darkUnit.GetComponent<CerebroSimple>().Perform();
            yield return new WaitForSeconds(.5f);
        }
        this.gameObject.GetComponent<CerebroSimple>().Perform();
        yield return new WaitForSeconds(.5f);
        EndTurn();
    }

    void GetGoldIncome(int playerTurn) {
        foreach (Village village in FindObjectsOfType<Village>())
        {
            if (village.playerNumber == playerTurn)
            {
                if (playerTurn == 1)
                {
                    player1Gold += village.goldPerTurn;
                    if (village.isVillage)
                        player1Gold += village.goldPerTurn;
                }
                else
                {
                    player2Gold += village.goldPerTurn;
                    if (village.isVillage)
                        player2Gold += village.goldPerTurn;
                }
            }
        }
        UpdateGoldText();
    }

    public void UpdateGoldText()
    {
        player1GoldText.text = player1Gold.ToString();
        player2GoldText.text = player2Gold.ToString();
    }

    // Victory UI

    public void ShowVictoryPanel(int playerNumber) {

        if (playerNumber == 1)
        {
            blueVictory.SetActive(true);
        } else if (playerNumber == 2) {
            darkVictory.SetActive(true);
        }
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
