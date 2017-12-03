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
    public float jumpForce = 50;
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

    private string input_interact;
    private string input_drop;
    private string input_jump;
    private bool jumpAvailable = true;

    Animator animator;

	// Use this for initialization
	void Start () 
    {
        animator = GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        if (enableKeyboardInput) {
            input_xAxis = "Horizontal";
            input_yAxis = "Vertical";
            input_interact = "Fire1";
            input_drop = "Fire2";
            input_jump = "Jump";
        }
        else
        {
            input_xAxis = "Horizontal" + playerID.ToString();
            input_yAxis = "Vertical" + playerID.ToString();
            input_interact = "xboxX" + playerID.ToString();
            input_drop = "xboxB" + playerID.ToString();
            input_jump = "xboxA" + playerID.ToString();
        }

        moneyText.text = "PLAYER " + playerID + " MONEYS: " + playerMoney;
        initSpeed = speed;

	}

    void Update()
    {
        if (Input.GetButton(input_drop) && hasTree)
        {
            Debug.Log("drop tree " + gameObject.name + " whos ID = "  + playerID.ToString());
            DropTree();
        }
    }

	// Update is called once per frame
	void FixedUpdate () 
    {
        if (Input.GetButton(input_jump) && jumpAvailable)
        {
            //rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            jumpAvailable = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        Vector3 targetVelocity = Vector3.Normalize(new Vector3(Input.GetAxis(input_xAxis), 0, Input.GetAxis(input_yAxis)));
        //rb.velocity = targetVelocity * speed * burden;
        rb.AddForce(targetVelocity * speed * burden, ForceMode.Force);
        
        var localVelocity = gameObject.transform.InverseTransformDirection(rb.velocity);
        if (localVelocity.magnitude != 0 && targetVelocity != Vector3.zero)
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
            
            if(Input.GetButton(input_interact) && cutAvailable)
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

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            jumpAvailable = true;
        }
    }

    private void DropTree()
    {
        carriedObject.transform.parent = null;

        Wood wood = carriedObject.GetComponent<Wood>();
        Rigidbody carriedRb = carriedObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        carriedRb.mass = wood.woodMass;

        carriedCol.enabled = true;
        carriedCol = null;
        ResetPlayerStats();
    }

    public void GetOffMyLawn()
    {
        if (hasTree)
            DropTree();
        transform.position = new Vector3(40, transform.position.y, 0);
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
        audio.clip = cuttings[Random.Range(0, cuttings.Length)];
        audio.Play();
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
