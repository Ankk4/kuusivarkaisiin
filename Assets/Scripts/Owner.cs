using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Owner : MonoBehaviour 
{
    //Rigidbody rb;

    public Transform goal;
    public Transform goal2;
    UnityEngine.AI.NavMeshAgent agent;
    Animator animator;

    void Start()
    {
        //rb = gameObject.GetComponent<Rigidbody>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }  


    void Update()
    {

    }

	// Update is called once per frame
	void FixedUpdate () 
    {
        float distanceToPlayer1 = CheckSightToPlayer(goal.position, "Player1");
        float distanceToPlayer2 = CheckSightToPlayer(goal2.position, "Player2");

        if (distanceToPlayer1 == distanceToPlayer2)
            agent.destination = transform.position;
        else if (distanceToPlayer1 < distanceToPlayer2) {
            agent.destination = goal.position;
            if (distanceToPlayer1 < 2.0f) {
                Player caughtPlayer = goal.gameObject.GetComponent<Player>();
                caughtPlayer.GetOffMyLawn();
            }
        }
        else {
            agent.destination = goal2.position;
            if (distanceToPlayer2 < 2.0f) {
                Player caughtPlayer = goal2.gameObject.GetComponent<Player>();
                caughtPlayer.GetOffMyLawn();
            }
        }
	}

    float CheckSightToPlayer(Vector3 playerPos, string desiredTargetTag)
    {
        Vector3 myPos = transform.position;
        float distanceToPlayer = Vector3.Distance(myPos, playerPos);
        RaycastHit hit;
        if (Physics.Raycast(myPos, playerPos - myPos, out hit, distanceToPlayer) && hit.collider.gameObject.tag == desiredTargetTag)
        {
            //Debug.DrawRay(sightStartPos, p1pos, Color.yellow);
            //Debug.DrawRay(sightStartPos, Vector3.MoveTowards(sightStartPos, hit.collider.transform.position, distanceToPlayer), Color.green);
            //Debug.Log("hit! " + hit.collider.tag);
            return distanceToPlayer;
        }
        else
        {
            //Debug.DrawRay(sightStartPos, p1dir, Color.red);
            //Debug.Log("no hit :(");
            return 10000.0f;
        }
    }

}
