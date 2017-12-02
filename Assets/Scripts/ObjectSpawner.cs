using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

    public float spawnPadding = 10f;
    public float ySpawn = -0.2f;
    public float spawnTime = 3f;            // How long between each spawn.           
    public Bounds groundSize;
    public GameObject[] hazardList;

    private float timeElapsed = 0;
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
        Instantiate(hazardList[Random.Range(0, hazardList.Length)], NewPosition(), transform.rotation);
    }

    Vector3 NewPosition()
    {
        float xMin = (this.groundSize.center.x - this.groundSize.size.x / 2) + spawnPadding;
        float xMax = (this.groundSize.center.x + this.groundSize.size.x / 2) - spawnPadding;
        float zMin = (this.groundSize.center.z - this.groundSize.size.z / 2) + spawnPadding;
        float zMax = (this.groundSize.center.z + this.groundSize.size.z / 2) - spawnPadding;
        return new Vector3(Random.Range(xMin, xMax), ySpawn, Random.Range(zMin, zMax));
    }
}
