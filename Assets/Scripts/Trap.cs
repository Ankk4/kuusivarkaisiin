using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    public float spawnPadding = 10f;
    public float ySpawn = -0.2f;

    public int money = 0;
    public float burden = 0.01f;
    public float timeActive = 3f;
    public bool activated = false;

    private bool alreadyDestroyed = false;
    private float timeElapsed;
    private Bounds groundSize;

    void Start()
    {
        this.groundSize = GameObject.FindGameObjectWithTag("Ground").GetComponent<Collider>().bounds;
    }

    // Update is called once per frame
    void Update()
    {     
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Hazard")
        {
            col.gameObject.transform.position = NewPosition();
        }

        if (col.gameObject.tag == "Collectable")
        {
            transform.position = NewPosition();
        }
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
