using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

    public GameObject bearTrap;
    public float spawnTime = 3f;            // How long between each spawn.
    public float timeElapsed = 0;           
    public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
    public Bounds groundSize;

	// Use this for initialization
	void Start () {
        this.groundSize = GameObject.FindGameObjectWithTag("Ground").GetComponent<Collider>().bounds;
	}
	
	// Update is called once per frame
	void Update () {
        this.timeElapsed += Time.deltaTime;
        if (timeElapsed > spawnTime)
        {
            Spawn();
            timeElapsed = 0;
        }
	}

    public void Spawn()
    {
        Vector3 pos = new Vector3(
            Random.Range(this.groundSize.center.x - this.groundSize.size.x / 2, this.groundSize.center.x + this.groundSize.size.x / 2),
            0.5f,
            Random.Range(this.groundSize.center.z - this.groundSize.size.z / 2, this.groundSize.center.z + this.groundSize.size.z / 2)
        );

        GameObject ob = (GameObject)Instantiate(this.bearTrap, pos, transform.rotation);
    }

    void OnTriggerEnter(Collider other)
    {
        print("OHO OSUI");
        Vector3 pos = new Vector3(
            Random.Range(this.groundSize.center.x - this.groundSize.size.x / 2, this.groundSize.center.x + this.groundSize.size.x / 2),
            0.5f,
            Random.Range(this.groundSize.center.z - this.groundSize.size.z / 2, this.groundSize.center.z + this.groundSize.size.z / 2)
        );
        other.transform.position = pos;
    }
}
