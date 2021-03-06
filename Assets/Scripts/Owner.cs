﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Owner : MonoBehaviour 
{
    //Rigidbody rb;

    public GameObject[] players;
    UnityEngine.AI.NavMeshAgent agent;
    Animator animator;

    public Bounds groundSize;
    public float spawnPadding = 15f;
    public float changeTime = 30f;
    public float elapsedTime = 0f;

    void Start()
    {
        //rb = gameObject.GetComponent<Rigidbody>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        animator = GetComponent<Animator>();
        this.groundSize = GameObject.FindGameObjectWithTag("Ground").GetComponent<Collider>().bounds;
    }  


    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

	// Update is called once per frame
	void FixedUpdate () 
    {
        elapsedTime += Time.deltaTime;
        int closestPlayerID = 0;
        float closestPlayerDistance = 10000.0f;
        for (int i = 0;  i < players.Length; i++)
        {
            float distanceToPlayer = GetDistanceToPlayer(players[i].transform.position, "Player" + (i + 1).ToString());
            if (distanceToPlayer != 10000.0f && distanceToPlayer < closestPlayerDistance) {
                closestPlayerID = i;
                closestPlayerDistance = distanceToPlayer;
            }
        }
        if (closestPlayerDistance != 10000.0f)
            agent.destination = players[closestPlayerID].transform.position;
        else if(elapsedTime > changeTime)
            agent.destination = GetFallbackDestination();

        if (closestPlayerDistance < 2.0f)
        {
            Player caughtPlayer = players[closestPlayerID].GetComponent<Player>();
            caughtPlayer.GetOffMyLawn();
        }
	}

    Vector3 GetFallbackDestination()
    {
        elapsedTime = 0;
        float xMin = (this.groundSize.center.x - this.groundSize.size.x / 2) + spawnPadding;
        float xMax = (this.groundSize.center.x + this.groundSize.size.x / 2) - spawnPadding;
        float zMin = (this.groundSize.center.z - this.groundSize.size.z / 2) + spawnPadding;
        float zMax = (this.groundSize.center.z + this.groundSize.size.z / 2) - spawnPadding;
        return new Vector3(Random.Range(xMin, xMax), transform.position.y, Random.Range(zMin, zMax));
    }

    float GetDistanceToPlayer(Vector3 playerPos, string desiredTargetTag)
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
