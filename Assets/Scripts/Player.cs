using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
    Rigidbody rb;
    public float speed = 5.0f;
    public float turnSpeed = 5.0f;
    
	// Use this for initialization
	void Start () 
    {
        rb = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        Vector3 targetVelocity = Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        rb.velocity = targetVelocity * speed;
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
}
