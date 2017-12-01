using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kekkonen : MonoBehaviour {

    public Vector3 startLocation;
    public Vector3 endLocation;
    public float omegaX = 1.0f;
    public float amplitudeX = 10.0f;
    public float omegaY = 1.0f;
    public float amplitudeY = 20.0f;
    public float index;
    public int speed = 10;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;

        index += Time.deltaTime;
        float x = amplitudeX * Mathf.Cos(omegaX * index);
        float y = amplitudeY * Mathf.Sin(omegaY * index);

        transform.LookAt(new Vector3(-x, endLocation.y, endLocation.z));
        transform.localPosition = new Vector3(x, y, transform.position.z);
       
        transform.position = Vector3.MoveTowards(
            transform.position, 
            this.endLocation,
            step
        );
        
	}
}
