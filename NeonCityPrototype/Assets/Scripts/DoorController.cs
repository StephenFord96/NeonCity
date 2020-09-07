using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private LockPickGameController gameData;
    private PlayerController player;
    private bool doorClosed;
    private bool playerInRange;
    private Animator doorAnim;
    private bool onCooldown;
    private bool aiCooldown;
    public bool playerPicking;
    public bool doorLocked;
    public GameObject miniGame;
    public float[] trueCombination = new float[3];

    // Start is called before the first frame update
    void Start()
    {

        doorClosed = true;
        doorAnim = GetComponent<Animator>();
        onCooldown = false;
        aiCooldown = false;
        playerPicking = false;

        if(gameObject.tag == "Locked Door")
        {
            doorLocked = true;

            trueCombination[0] = Random.Range(1, 12);
            trueCombination[1] = Random.Range(1, 12);
            trueCombination[2] = Random.Range(1, 12);

        }
        else
        {
            doorLocked = false;
        }

    }

    // Update is called once per frame
    void Update()
    {

        
        
        if (Input.GetKeyDown("e") && playerInRange == true && doorClosed == true && onCooldown == false && doorLocked == false)
        {
            onCooldown = true;
            doorClosed = false;
            doorAnim.SetBool("Door Open", true);
        }

        if (Input.GetKeyDown("e") && playerInRange == true && doorClosed == false && onCooldown == false && doorLocked == false)
        {
            onCooldown = true;
            doorClosed = true;
            doorAnim.SetBool("Door Open", false);
        }

        if (Input.GetKeyUp("e"))
        {
            onCooldown = false;
        }

        if(doorLocked == true && Input.GetKeyDown("e") && playerInRange == true && playerPicking == false)
        {
            playerPicking = true;
            Instantiate(miniGame, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            player = FindObjectOfType<PlayerController>();
            player.playerInteracting = true;
            gameData = FindObjectOfType<LockPickGameController>();
            gameData.combination[0] = trueCombination[0];
            gameData.combination[1] = trueCombination[1];
            gameData.combination[2] = trueCombination[2];

        }

        if(playerPicking == true && Input.GetKeyDown("escape"))
        {
            playerPicking = false;
            player.playerInteracting = false;
        }

        
    }




    // prevents AI from getting stuck if they are determined to get through a door and are head banging on it also opens doors
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" && aiCooldown == false)
        {
            aiCooldown = true;
           // Debug.Log("Stuck but saved");
            doorAnim.SetBool("Door Open", true);
            StartCoroutine(doorCooldown());
        }
    }


    //player may access doors due to proximity of object
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
            
        }
                      

    } 

        // closes doors and for AI only closes if they actually passed the door AND left the trigger to prevent door spasms
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
            
        }

        if (other.gameObject.tag == "Enemy")
        {
            if ((other.gameObject.GetComponent<EnemyController>().dataLR == true) && (other.gameObject.transform.position.x > gameObject.transform.position.x))
            {
                aiCooldown = true;
                doorAnim.SetBool("Door Open", false);
                StartCoroutine(doorCooldown());
            }else if((other.gameObject.GetComponent<EnemyController>().dataLR == false) && (other.gameObject.transform.position.x < gameObject.transform.position.x))
            {
                aiCooldown = true;
                doorAnim.SetBool("Door Open", false);
                StartCoroutine(doorCooldown());
            }
        }

        
    }

    public void doorUnlocked()
    {
        playerPicking = false;
        player.playerInteracting = false;
        Debug.Log("Door recieved Message");
        doorLocked = false;
    }


    //prevent door spasms and buildup from AI
    IEnumerator doorCooldown()
    {
        yield return new WaitForSeconds(0.3f);
        aiCooldown = false;
    } 

    
}



