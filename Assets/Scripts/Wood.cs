using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour {
    public int money = 0;
    public float burden = 0.1f;
    public int maxHealth = 100;
    public int currentHealth = 10;
    public float woodMass = 3;

	// Use this for initialization
	void Start () 
    {
        currentHealth = maxHealth;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.mass = woodMass;
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}
}
