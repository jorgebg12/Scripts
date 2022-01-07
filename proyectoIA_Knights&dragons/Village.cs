using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{
    public bool isVillage;
    public Village[] miembros;
    public int goldPerTurn;
    public int playerNumber;
    public int cost;

    private void Awake()
    {
        isVillage = false;
    }
}
