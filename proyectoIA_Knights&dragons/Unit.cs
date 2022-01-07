using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ClaseUnidad
{
    caballero,
    arquero,
    murcielago,
    rey,
    edificio,
    castillo
}

public class Unit : MonoBehaviour
{
    public ClaseUnidad claseTropa;
    public bool isSelected;
    public bool hasMoved;
    public bool isDefending = false;

    public int tileSpeed;
    public float moveSpeed;

    private GM gm;
    private CharacterCreation cc;

    public int attackRadius;
    public bool hasAttacked;
    public List<Unit> enemiesInRange = new List<Unit>();

    public List<Unit> nearbyEnemies = new List<Unit>();
    public List<Unit> nearbyStructures = new List<Unit>();
    public int target = -1;
    public Transform pos;

    public Tile[] availableTiles;

    public int playerNumber;

    public GameObject weaponIcon;

    // Attack Stats
    public int health;
    [HideInInspector]public int totalHP;
    public int attackDamage;
    public int defenseDamage;
    public int armor;

    public DamageIcon damageIcon;

    public int cost;

	public GameObject deathEffect;

	private Animator camAnim;

	private AudioSource source;

    public Text displayedText;
    //Las casillas que hacen el pathfindig
    [HideInInspector]public List<Tile> walkableTiles = new List<Tile>();

   


    private void Start()
    {
		source = GetComponent<AudioSource>();
		camAnim = Camera.main.GetComponent<Animator>();
        gm = FindObjectOfType<GM>();
        cc = FindObjectOfType<CharacterCreation>();
        totalHP = health;
        UpdateHealthDisplay();
    }
    private void fixPosition()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 0.1f);
        if (col == null)
        {
            this.gameObject.transform.position = col.transform.position;
        }
    }

    private void UpdateHealthDisplay ()
    {
        if (claseTropa == ClaseUnidad.castillo)
        {
            displayedText.text = health.ToString();
        }
    }

    private void OnMouseDown() // select character or deselect if already selected
    {
        
        ResetWeaponIcon();

        if (isSelected == true)
        {
            
            isSelected = false;
            gm.selectedUnit = null;
            gm.ResetTiles();

        }
        else {
            if (playerNumber == gm.playerTurn && gm.playerTurn == 2) { // select unit only if it's his turn
                if (gm.selectedUnit != null)
                { // deselect the unit that is currently selected, so there's only one isSelected unit at a time
                    gm.selectedUnit.isSelected = false;
                }
                gm.ResetTiles();

                gm.selectedUnit = this;

                isSelected = true;
				if(source != null){
					source.Play();
				}
				
                GetWalkableTiles();
                GetEnemies();
            }

        }



        Collider2D[] colisiones = Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.15f);
        if (colisiones != null)
        {
            foreach (Collider2D col in colisiones)
            {
                if (col.gameObject.layer == 9)
                {
                    Unit unit = col.GetComponent<Unit>(); // double check that what we clicked on is a unit
                    if (unit != null && gm.selectedUnit != null)
                    {
                        if (gm.selectedUnit.enemiesInRange.Contains(unit) && !gm.selectedUnit.hasAttacked)
                        { // does the currently selected unit have in his list the enemy we just clicked on
                            gm.selectedUnit.Attack(unit);

                        }
                    }
                    break;
                }
            }
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            gm.UpdateInfoPanel(this);
        }
    }



    public void GetWalkableTiles() { // Looks for the tiles the unit can walk on
        if (hasMoved == true) {
            return;
        }
        walkableTiles.Clear();
        /*
        Tile[] tiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in tiles) {
            if (Mathf.Abs(transform.position.x - tile.transform.position.x) + Mathf.Abs(transform.position.y - tile.transform.position.y) <= tileSpeed)
            { // how far he can move
                if (tile.isClear() == true)
                { // is the tile clear from any obstacles
                    tile.Highlight();
                    walkableTiles.Add(tile);
                }

            }          
        }*/

        Tile tileUnidad = gm.tilePos[this.transform.position];

        if (tileSpeed <= 0)
            return;
        
        foreach (Tile vecino in tileUnidad.vecinos)
        {
            if (vecino.isClear())
            {
                vecino.distanciaAcum = 1;
                vecino.Highlight();
                compruebaAccesible(vecino, vecino.distanciaAcum);
            }
        }

    }

    public void GetWalkableTilesIA() { // Looks for the tiles the unit can walk on
        if (hasMoved == true) {
            return;
        }
        walkableTiles.Clear();
        
        Tile tileUnidad = gm.tilePos[this.transform.position];

        if (tileSpeed <= 0)
            return;
        
        foreach (Tile vecino in tileUnidad.vecinos)
        {
            if (vecino.isClear())
            {
                vecino.distanciaAcum = 1;
                vecino.isWalkable = true;
                compruebaAccesibleIA(vecino, 1);
            }
        }

    }

    public List<Tile> GetSuroundingTiles(Unit unit){
        unit.setPos();
        Tile unitTile = gm.tilePos[unit.pos.position];
        List<Tile> suroundingTiles = new List<Tile>();
        foreach (Tile vecino in unitTile.vecinos)
        {
            if (vecino.isClear())
            {
                suroundingTiles.Add(vecino);
            }
        }
        //Debug.LogWarning("Vecinos=" + suroundingTiles.Count);
        return suroundingTiles;
    }

    public Transform GetCloseTilePos(Unit myTarget){
        List<Tile> tiles = GetSuroundingTiles(myTarget);

        foreach (Tile tile in tiles)
        {
            if(walkableTiles.Contains(tile)){
                return tile.transform;
            }
        }
        return null;
    }

    void compruebaAccesible(Tile actual, int distanciaActual)
    {
        

        if (distanciaActual >= tileSpeed)
            return;
            
        if (!actual.isClear())
            return;

        ++distanciaActual;
        if(!walkableTiles.Contains(actual))
            walkableTiles.Add(actual);

        foreach (Tile vecino in actual.vecinos)
        {
            if (walkableTiles.Contains(vecino))
            {
                if (vecino.distanciaAcum > distanciaActual)
                    vecino.distanciaAcum = distanciaActual;

                compruebaAccesible(vecino, distanciaActual);
                continue;
            }

            if (vecino.isClear())
            {
                vecino.distanciaAcum = distanciaActual;
                vecino.Highlight();
                compruebaAccesible(vecino, distanciaActual);
            }
        }

    }

    void compruebaAccesibleIA(Tile actual, int distanciaActual)//Modificar
    {
        if (distanciaActual >= tileSpeed)
            return;
            
        if (!actual.isClear()){
            return;
        }

        ++distanciaActual;
        if (!walkableTiles.Contains(actual))
            walkableTiles.Add(actual);

        foreach (Tile vecino in actual.vecinos)
        {
            if (walkableTiles.Contains(vecino))
            {
                if (vecino.distanciaAcum > distanciaActual)
                    vecino.distanciaAcum = distanciaActual;

                compruebaAccesibleIA(vecino, distanciaActual);
                continue;
            }

            if (vecino.isClear())
            {
                vecino.distanciaAcum = distanciaActual;
                vecino.isWalkable=true;
                compruebaAccesibleIA(vecino, distanciaActual);
            }
        }

    }

    public void GetEnemies() {
    
        enemiesInRange.Clear();

        Unit[] enemies = FindObjectsOfType<Unit>();
        foreach (Unit enemy in enemies)
        {
            if (Mathf.Abs(transform.position.x - enemy.transform.position.x) + Mathf.Abs(transform.position.y - enemy.transform.position.y) <= attackRadius) // check is the enemy is near enough to attack
            {
                if (enemy.playerNumber != gm.playerTurn && !hasAttacked) { // make sure you don't attack your allies
                    enemiesInRange.Add(enemy);
                    enemy.weaponIcon.SetActive(true);
                }

            }
        }
    }

    public List<Unit> GetAllStructuresOfPlayer(int player) {
        Unit[] boardElements = FindObjectsOfType<Unit>();
        List<Unit> s = new List<Unit>();
        foreach (Unit element in boardElements)
        {
            if(element.tag == "Structure" && element.playerNumber == player){
                s.Add(element);
            }
        }
        return s;
    }

    public void setPos(){
        pos = this.transform;
    }

    public void GetNearbyEnemies() {
    
        nearbyEnemies.Clear();

        Unit[] enemies = FindObjectsOfType<Unit>();
        Tile enemyTile;
        foreach (Unit enemy in enemies)
        {
            if(enemy.playerNumber != gm.playerTurn){
                enemy.setPos();
                enemyTile = gm.tilePos[enemy.pos.position];
                foreach (Tile vecino in enemyTile.vecinos)
                {
                    if(walkableTiles.Contains(vecino)){
                        nearbyEnemies.Add(enemy);
                    }
                }
            }
        }
    }

     public void GetNearbyStructures() {
    
        nearbyStructures.Clear();

        Unit[] structures = FindObjectsOfType<Unit>();
        Tile structureTile;
        foreach (Unit structure in structures)
        {
            if(structure.playerNumber == gm.playerTurn && (structure.tag == "Structure" || structure.tag == "Castle")){
                structure.setPos();
                structureTile = gm.tilePos[structure.pos.position];
                foreach (Tile vecino in structureTile.vecinos)
                {
                    if(walkableTiles.Contains(vecino)){
                        nearbyStructures.Add(structure);
                    }
                }
            }
        }
    }

    public Unit FindEnemyCastle(){
        Unit[] enemies = FindObjectsOfType<Unit>();
        foreach (Unit enemy in enemies)
        {
            if(enemy.tag == "Castle" && enemy.playerNumber != gm.playerTurn){
                return enemy;
            }
        }
        return null;
    }

    public void Move(Transform movePos)
    {
        gm.ResetTiles();
        StartCoroutine(StartMovement(movePos));
    }
    public void followPath(List<Tile> listado)
    {
        gm.ResetTiles();
        StartCoroutine(followPathCorroutine(listado));
    }

    public void Attack(Unit enemy) {
        hasAttacked = true;

        int enemyDamege = attackDamage - enemy.armor;
        int unitDamage = enemy.defenseDamage - armor;

        if (enemyDamege >= 1)
        {
            enemy.health -= enemyDamege;
            enemy.UpdateHealthDisplay();
            DamageIcon d = Instantiate(damageIcon, enemy.transform.position, Quaternion.identity);
            d.Setup(enemyDamege);
        }

        if (transform.tag == "Archer" && enemy.tag != "Archer")
        {
            if (Mathf.Abs(transform.position.x - enemy.transform.position.x) + Mathf.Abs(transform.position.y - enemy.transform.position.y) <= 1) // check is the enemy is near enough to attack
            {
                if (unitDamage >= 1)
                {
                    health -= unitDamage;
                    UpdateHealthDisplay();
                    DamageIcon d = Instantiate(damageIcon, transform.position, Quaternion.identity);
                    d.Setup(unitDamage);
                }
            }
        } else {
            if (unitDamage >= 1)
            {
                health -= unitDamage;
                UpdateHealthDisplay();
                DamageIcon d = Instantiate(damageIcon, transform.position, Quaternion.identity);
                d.Setup(unitDamage);
            }
        }

        if (enemy.health <= 0)
        {
         
            if (deathEffect != null){
				Instantiate(deathEffect, enemy.transform.position, Quaternion.identity);
				camAnim.SetTrigger("shake");
			}

            if (enemy.claseTropa == ClaseUnidad.castillo)
            {
                gm.ShowVictoryPanel(enemy.playerNumber);
            }

            if (enemy.claseTropa == ClaseUnidad.edificio)
            {
                Debug.Log("Romper casa");
                //Si el que ataco no es una aldea
                //Buscar si uno de sus 4 miembros es aldea
                if (!enemy.GetComponent<Village>().isVillage)
                {
                    switch (gm.playerTurn)
                    {
                        case 1:
                            foreach (Transform estructuras in cc.player2Structures.transform)
                            {
                                if (estructuras.GetComponent<Village>().isVillage && Array.Exists(estructuras.GetComponent<Village>().miembros, x => x == enemy.GetComponent<Village>()))
                                {
                                    //Existe una población con esta estructura
                                    estructuras.GetComponent<Village>().isVillage = false;
                                    estructuras.GetComponent<Village>().miembros = null;
                                }
                            }
                            break;
                        case 2:
                            foreach (Transform estructuras in cc.player1Structures.transform)
                            {
                                if (estructuras.GetComponent<Village>().isVillage && Array.Exists(estructuras.GetComponent<Village>().miembros, x => x == enemy.GetComponent<Village>()))
                                {
                                    //Existe una población con esta estructura
                                    estructuras.GetComponent<Village>().isVillage = false;
                                    estructuras.GetComponent<Village>().miembros = null;
                                }
                            }
                            break;
                    }
                    
                }
            }

            //GetWalkableTiles(); // check for new walkable tiles (if enemy has died we can now walk on his tile)
            gm.RemoveInfoPanel(enemy);
            gm.unactive.Add(enemy.gameObject);
            enemy.gameObject.SetActive(false);
        }

        if (health <= 0)
        {

            if (deathEffect != null)
			{
				Instantiate(deathEffect, enemy.transform.position, Quaternion.identity);
				camAnim.SetTrigger("shake");
			}

			if (claseTropa == ClaseUnidad.castillo)
            {
                gm.ShowVictoryPanel(playerNumber);
            }

            gm.ResetTiles(); // reset tiles when we die
            gm.RemoveInfoPanel(this);
            gm.unactive.Add(this.gameObject);
            this.gameObject.SetActive(false);
        }

        gm.UpdateInfoStats();
  

    }
    /// <summary>
    /// Condition manager
    /// </summary>
    

    //
    public void ResetWeaponIcon() {
        Unit[] enemies = FindObjectsOfType<Unit>();
        foreach (Unit enemy in enemies)
        {
            enemy.weaponIcon.SetActive(false);
        }
    }

    IEnumerator StartMovement(Transform movePos) { // Moves the character to his new position.


        while (transform.position.x != movePos.position.x) { // first aligns him with the new tile's x pos
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(movePos.position.x, transform.position.y), moveSpeed * Time.deltaTime);
            yield return null;
        }
        while (transform.position.y != movePos.position.y) // then y
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, movePos.position.y), moveSpeed * Time.deltaTime);
            yield return null;
        }

        hasMoved = true;
        ResetWeaponIcon();
        GetEnemies();
        gm.MoveInfoPanel(this);
    }

    IEnumerator followPathCorroutine(List<Tile> listado)
    { // Moves the character to his new position.

        foreach(Tile tile in listado)
        {

            while (transform.position.x != tile.transform.position.x )
            {
                transform.position = Vector2.MoveTowards(this.transform.position, tile.transform.position, moveSpeed * Time.deltaTime);
                yield return null;
                
            }
            while(transform.position.y != tile.transform.position.y)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, tile.transform.position, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
        
        hasMoved = true;
        ResetWeaponIcon();
        GetEnemies();
        gm.MoveInfoPanel(this);
    }


}
