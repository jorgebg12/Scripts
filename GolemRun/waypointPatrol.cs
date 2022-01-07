using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class waypointPatrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;

    [HideInInspector]public bool activado = false;

    public GameObject activator;
    public GameObject player;

    int m_CurrentWaypointIndex;

    Animator animator;

    bool checkDistancePlayer;
    public LayerMask characterLayer;
    public float radius;

    void Start ()
    {
        animator = GetComponent<Animator>();
    }

    void Update ()
    {
        if(activator==null){

            //Activar animacio
            checkDistancePlayer = Physics.CheckSphere(this.transform.position, radius, characterLayer);


            animator.SetBool("camina", true);
        


            if(checkDistancePlayer){

                navMeshAgent.SetDestination (player.transform.position);

            }else if(navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
                navMeshAgent.SetDestination (waypoints[m_CurrentWaypointIndex].position);
                Debug.Log(waypoints[m_CurrentWaypointIndex]);
            }
        }
    }
}
