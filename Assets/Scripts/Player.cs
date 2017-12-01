﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour 
{
    Rigidbody rb;
    
    public bool hasTree = false;
    public int playerMoney = 0;
    public int currentMoney = 0;
    public float speed = 5.0f;
    public float turnSpeed = 5.0f;

    public Text moneyText;

    public GameObject carriedObject;
    public Transform backPack;

    [Range(1,4)]public int playerID = 1;

    public float burden = 1;

    private string input_xAxis;
    private string input_yAxis;
    public bool enableKeyboardInput = false;
    
	// Use this for initialization
	void Start () 
    {
        rb = gameObject.GetComponent<Rigidbody>();
        if (enableKeyboardInput) {
            input_xAxis = "Horizontal";
            input_yAxis = "Vertical";
        }
        else
        {
            input_xAxis = "Horizontal" + playerID.ToString();
            input_yAxis = "Vertical" + playerID.ToString();
        }

        moneyText.text = "PLAYER " + playerID + " MONEYS: " + playerMoney;

	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        
        Vector3 targetVelocity = Vector3.Normalize(new Vector3(Input.GetAxis(input_xAxis), 0, Input.GetAxis(input_yAxis)));
        rb.velocity = targetVelocity * speed * burden;
        var localVelocity = gameObject.transform.InverseTransformDirection(rb.velocity);
        if (localVelocity.x !=0 && localVelocity.z != 0)
        {
            gameObject.transform.rotation = Quaternion.LookRotation(targetVelocity, Vector3.up);
        }
        //Debug.Log("velocity" + localVelocity.z);
        //rb.AddForce(transform.forward *(Input.GetAxis("Vertical")) * speed);
        //rb.AddForce(transform.right * (Input.GetAxis("Horizontal")) * speed);
        //rb.AddTorque(transform.up * (Input.GetAxis("Horizontal")) * turnSpeed);
	}
    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Collectable" && !hasTree)
        {
            carriedObject = other.gameObject;
            carriedObject.transform.parent = backPack;
            carriedObject.transform.position = backPack.position;
            carriedObject.transform.rotation = backPack.rotation;

            Wood wood = other.gameObject.GetComponent<Wood>();

            currentMoney = wood.money;
            burden = wood.burden;

            hasTree = true;
            other.enabled = false;
        }


        if (other.gameObject.tag == "Shop" && hasTree)
        {
            Destroy(carriedObject);

            playerMoney += currentMoney;
            moneyText.text = "PLAYER " + playerID + " MONEYS: " + playerMoney;
            
            currentMoney = 0;
            burden = 1;
            hasTree = false;
        }
    }
}
