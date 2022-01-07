using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostView : MonoBehaviour
{
    public Transform player;
   // public GameEnding gameEnding;

    public static bool m_IsPlayerInRange;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
            Debug.Log("hola");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
            Debug.Log("adios");

        }
    }

    public static bool IsPlayerInRange
    {
        get { return m_IsPlayerInRange; }

    }
}
