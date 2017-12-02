using System.Collections;
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
    public Trap activatedTrap;
    public int cutStrength = 1;
    public bool cutAvailable = true;
    public Transform backPack;
    [Range(1, 4)]public int playerID = 1;
    public float burden = 1;
    public bool enableKeyboardInput = false;
    public AudioSource audio;
    public AudioClip[] screams;
    public AudioClip[] cuttings;

    private float trapTime;
    private float initSpeed;
    private Collider carriedCol;
    private string input_xAxis;
    private string input_yAxis;    

    Animator animator;

	// Use this for initialization
	void Start () 
    {
        animator = GetComponent<Animator>();
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
        initSpeed = speed;

	}

    void Update()
    {
        if (Input.GetButtonDown("Fire2") && hasTree)
        {
            DropTree();
        }
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
        animator.SetFloat("Speed", targetVelocity.magnitude);
	}

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Trap")
        {
            Trap trap = other.gameObject.GetComponent<Trap>();
            if (!trap.activated) 
            {
                if (carriedObject != null && hasTree) {
                    DropTree();
                }
                trap.activated = true;
                trapTime = trap.timeActive;                
                StartCoroutine(Trapped(trapTime));
                Destroy(trap.gameObject, trapTime);
            }
        }

        if (other.gameObject.tag == "Shop" && hasTree)
        {
            Destroy(carriedObject);

            playerMoney += currentMoney;
            moneyText.text = "PLAYER " + playerID + " MONEYS: " + playerMoney;
  
            ResetPlayerStats();
        }

        if (other.gameObject.tag == "Trap")
        {
            StartCoroutine(Trapped(1.0F));
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Collectable" && !hasTree)
        {
            
            if(Input.GetButtonDown("Fire1") && cutAvailable)
            {
                StartCoroutine(CutTree(other, 0.15F));
            }
        }
    }

    void CollectTree(Collider col)
    {
        carriedObject = col.gameObject;
        carriedCol = col;

        col.enabled = false;

        Rigidbody colRb = carriedObject.GetComponent<Rigidbody>();
        if (colRb != null)
        {
            Destroy(colRb);
        }

        carriedObject.transform.parent = backPack;
        carriedObject.transform.position = backPack.position;
        carriedObject.transform.rotation = backPack.rotation;

        Wood wood = carriedObject.GetComponent<Wood>();

        currentMoney = wood.money;
        burden = wood.burden;

        hasTree = true;        
    }

    void DropTree()
    {
        carriedObject.transform.parent = null;

        Wood wood = carriedObject.GetComponent<Wood>();
        Rigidbody carriedRb = carriedObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        carriedRb.mass = wood.woodMass;

        carriedCol.enabled = true;
        carriedCol = null;
        ResetPlayerStats();
    }

    void ResetPlayerStats()
    {
        currentMoney = 0;
        burden = 1;
        hasTree = false;
        speed = initSpeed;
    }

    IEnumerator CutTree(Collider col, float waitTime)
    {
        animator.SetBool("Attacking",true);
        cutAvailable = false;
        Wood wood = col.gameObject.GetComponent<Wood>();
        wood.currentHealth -= cutStrength;

        col.gameObject.transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z - 90 / wood.maxHealth, Space.Self);
        if (wood.currentHealth <= 0)
        {
            CollectTree(col);
        }
        yield return new WaitForSeconds(waitTime);
        cutAvailable = true;
        animator.SetBool("Attacking", false);
    }

    IEnumerator Trapped(float waitTime)
    {
        audio.clip = screams[Random.Range(0, screams.Length)];
        audio.Play();

        float tempSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(waitTime);
        speed = tempSpeed;
    }
}
