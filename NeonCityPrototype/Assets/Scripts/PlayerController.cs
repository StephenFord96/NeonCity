using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Animator anim;
    public bool playerAttacking;
    private Rigidbody2D playerRigidBody;
    public float playerMoveSpeed;
    public bool playerMoving;
    public bool swordReleaser;
    public bool swordOnCooldown;
    public bool swingDone;
    public bool swordCommit;
    public bool playerBlocking;
    public bool deadZoned;
    public bool deadZoneLocked;
    public bool swordFollowThrough;
    public bool blockBlip;
    public bool blockDull;
    public bool blockLock;
    public bool stealthKilling;
    public bool playerInteracting;

    public bool[] playerBattleData = new bool[4];
    public bool dataLR;
    public bool dataHighLow;
    public bool dataDealingDamage;

    private Vector3 playerPos;
    private bool playerHL;
    

    private EnemyController battleChecker;

    private EnemyController visualSensors;
    private GameObject[] presentEnememies;
    private GameObject touchingEnemy;
    public GameObject hideNode;

    public bool[] playerData = new bool[9];




    public bool onStairsUp;
    public bool onStairsDown;

    private bool onHideSpotRight;
    private bool onHideSpotLeft;
    public bool playerHiddenLeft;
    public bool playerHiddenRight;
    public bool playerHidden;
    
    public int playerGridLocationX;
    public int playerGridLocationY;
    






    // Start is called before the first frame update
    void Start()
    {
        playerData = new bool[9];

        anim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerMoveSpeed = 3.5f;

        swordOnCooldown = false;
        anim.SetFloat("PlayerRightFace", 1f);
        anim.SetFloat("PlayerLeftFace", 0f);
        anim.SetFloat("PlayerLR", 1f);
        swordReleaser = true;
        swingDone = false;
        swordCommit = false;
        playerBlocking = false;
        deadZoned = true;
        deadZoneLocked = false;
        dataDealingDamage = false;
        swordFollowThrough = true;
        dataLR = true;
        blockBlip = false;
        


    }

    // Update is called once per frame
    void Update()
    {



        if (playerInteracting == false)
        {
            anim.SetBool("PlayerMoving", playerMoving);
        }
        else
        {
            anim.SetBool("PlayerMoving", false);
        }
        



        Vector3 mousePos = Input.mousePosition;

        //Debug.Log(mousePos.y);



        //determines mouse aim

        if ((mousePos.y >= 440) && (playerAttacking == false && playerBlocking == false) && swordFollowThrough == true)
        {

            anim.SetFloat("MouseHighLow", 1);
            playerHL = true;
            
        }

        if ((mousePos.y <= 360) && (playerAttacking == false && playerBlocking == false) && swordFollowThrough == true)
        {

            anim.SetFloat("MouseHighLow", -1);
            playerHL = false;

        }

        if ((mousePos.y > 360 && mousePos.y < 440) && (playerAttacking == false && playerBlocking == false) && swordFollowThrough == true)
        {

            anim.SetFloat("MouseHighLow", 0);
            deadZoned = true;
            
        }
        else { deadZoned = false; }



        // block checker
        // swordfollowthrough also means player is not stunned

        if ((blockBlip == true) || (Input.GetMouseButton(1) && swordCommit == false && deadZoned == false && swordFollowThrough == true && blockLock == false && playerInteracting == false))
        {
            playerBlocking = true;
            playerMoving = false;
        }
        else
        {
            playerBlocking = false;
            blockBlip = false;
            blockDull = false;
            
        }




        anim.SetBool("PlayerBlockDull", blockDull);
        
        

        anim.SetBool("PlayerBlocking", playerBlocking);
        


        //attack checker

        if ((swordCommit == true) || (Input.GetMouseButton(0) && swordOnCooldown == false && playerBlocking == false && deadZoned == false && deadZoneLocked == false && swordFollowThrough == true && playerHidden == false && playerInteracting == false))
        {
            playerAttacking = true;
            anim.SetBool("Attacking", true);



        }
        else
        {
            playerAttacking = false;
            anim.SetBool("Attacking", false);
            dataDealingDamage = false;

        }




        //mouseReleaser would be more apt
        //also cooldowns and action limitations

        if (!Input.GetMouseButton(0))
        {
            swordReleaser = true;
            deadZoneLocked = false;
            
        }
        else
        {
            swordReleaser = false;

            if (deadZoned == true)
            {
                deadZoneLocked = true;
            }
           
        }

        if (!Input.GetMouseButton(1))
        {
            blockLock = false;
        }else if(deadZoned == true) { blockLock = true; }






        if (swordReleaser == true && swingDone == false)
        {
            swordOnCooldown = false;
        }

        if ((swingDone == true || swordFollowThrough == false) && swordReleaser == false)
        {
            swordOnCooldown = true;         
        }


        anim.SetBool("BeenBlocked", !swordFollowThrough);


        //movement and running

        if (playerAttacking == false && playerBlocking == false && swordFollowThrough == true && stealthKilling == false && playerHiddenLeft == false && playerHiddenRight == false && playerInteracting == false)
        {
            
            playerRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * playerMoveSpeed, 0f);

            if (Input.GetAxisRaw("Horizontal") > 0.5)
            {
                anim.SetFloat("PlayerRightFace", 1f);
                anim.SetFloat("PlayerLeftFace", 0f);
                anim.SetFloat("PlayerLR", 1f);
                dataLR = true;

            }

            if (Input.GetAxisRaw("Horizontal") < -0.5)
            {
                anim.SetFloat("PlayerRightFace", 0f);
                anim.SetFloat("PlayerLeftFace", 1f);
                anim.SetFloat("PlayerLR", -1f);
                dataLR = false;
            }

            if (onStairsUp == true && Input.GetKeyDown("e"))
            {
                gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 3.96f, transform.position.z);
                //Debug.Log("going up");
            }

            if (onStairsDown == true && Input.GetKeyDown("e"))
            {
                gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 3.96f, transform.position.z);
                //Debug.Log("going down");
            }

        }

        //limitations for when player is allowed to move

        if (Input.GetAxisRaw("Horizontal") > -0.5f && Input.GetAxisRaw("Horizontal") < 0.5f)
        {
            playerMoving = false;
        }
        else if (playerAttacking == false && playerBlocking == false)
        {
            playerMoving = true;
        }

        if (playerMoving == false)
        {
            playerRigidBody.velocity = new Vector2(0f, 0f);
        }

        if (playerAttacking == true)
        {
            playerRigidBody.velocity = new Vector2(0f, 0f);
        }

        if (playerInteracting == true)
        {
            playerRigidBody.velocity = new Vector2(0f, 0f);
        }



        //packaged information for sword collision
        //true is right and up while false is left and down

        playerBattleData[0] = playerBlocking;
        playerBattleData[1] = dataLR;
        playerBattleData[2] = dataHighLow;
        playerBattleData[3] = dataDealingDamage;


        playerData[0] = dataLR;
        playerData[1] = playerHL;
        playerData[2] = playerAttacking;
        playerData[3] = playerBlocking;
        playerData[4] = playerMoving;
        playerData[5] = swordFollowThrough;
        playerData[6] = blockBlip;
        playerData[7] = playerHiddenLeft;
        playerData[8] = playerHiddenRight;



        playerPos = transform.position;

        presentEnememies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject e in presentEnememies)
        {
            visualSensors = e.gameObject.GetComponent<EnemyController>();
            visualSensors.playerPosCheck(playerPos, playerGridLocationX, playerGridLocationY);
            visualSensors.playerStanceCheck(playerData);

        }


       //math to determine which room on the coordinate scale the player is in
        playerGridLocationX = Mathf.CeilToInt((transform.position.x + 4.925f) / 9.85f);
        playerGridLocationY = Mathf.CeilToInt((transform.position.y + 1.98f) / 3.96f);



        //stealth kill section and while loops are used to bring the sprites togethor to make the animation cleaner
        if (touchingEnemy != null && battleChecker.dataLR == dataLR && battleChecker.playerSpotted == false && Input.GetKey("e") == true && stealthKilling == false && battleChecker.isDead == false )
        {

            Debug.Log("stealth killing");
            stealthKilling = true;
            anim.SetBool("StealthKill", true);
            playerMoving = false;
            battleChecker.isStrangled = true;                    

        }

        while (dataLR == true && stealthKilling == true && battleChecker.transform.position.x >= transform.position.x + 0.22f)
        {
            gameObject.transform.position = new Vector3(transform.position.x + 0.05f, transform.position.y, transform.position.z);
            Debug.Log("fixing1");
        }

        while (dataLR == false && stealthKilling == true && battleChecker.transform.position.x <= transform.position.x - 0.22f)
        {
            gameObject.transform.position = new Vector3(transform.position.x - 0.05f, transform.position.y, transform.position.z);
            Debug.Log("fixing2");
        }


        if (stealthKilling == false)
        {
            anim.SetBool("StealthKill", false);
        }

        //hide mechanic and input section

        if (Input.GetKeyDown("s") == true && playerHidden == false)
        {
            anim.SetBool("HidingLeft", onHideSpotLeft);
            anim.SetBool("HidingRight", onHideSpotRight);
            playerHiddenRight = onHideSpotRight;
            playerHiddenLeft = onHideSpotLeft;

            if(onHideSpotRight == true || onHideSpotLeft == true)
            {
                Instantiate(hideNode, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z), transform.rotation);
                playerHidden = true;
            }

        }

        if (Input.GetKeyUp("w") == true)
        {
            anim.SetBool("HidingLeft", false);
            anim.SetBool("HidingRight", false);
            playerHiddenLeft = false;
            playerHiddenRight = false;
            playerHidden = false;

        }
                     
    }

 

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            touchingEnemy = other.gameObject;

            //Debug.Log("You hit an Enemy");

            battleChecker = other.gameObject.GetComponent<EnemyController>();
            battleChecker.dataCheckFromPlayer(playerBattleData);


        }

        if (other.gameObject.tag == "StairsUp")
        {
            onStairsUp = true;
           
        }

        if (other.gameObject.tag == "StairsDown")
        {
            onStairsDown = true;

        }

        // hide mechanic based on object and player location
        if(other.gameObject.tag == "HidingSpot")
        {
            if(gameObject.transform.position.x >= other.gameObject.transform.position.x)
            {
                onHideSpotRight = true;
                onHideSpotLeft = false;
            } else if(gameObject.transform.position.x <= other.gameObject.transform.position.x)
            {
                onHideSpotLeft = true;
                onHideSpotRight = false;
            }
        }

    }

    void OnTriggerExit2D(Collider2D collider)
    {

        if (collider.gameObject.tag == "Enemy")
        {
            touchingEnemy = null;
        }

        if (collider.gameObject.tag == "StairsUp")
        {
            onStairsUp = false;
        }

        if (collider.gameObject.tag == "StairsDown")
        {
            onStairsDown = false;
        }

        // hide mechanic based on object and player location
        if (collider.gameObject.tag == "HidingSpot")
        {
            if (gameObject.transform.position.x >= collider.gameObject.transform.position.x)
            {
                onHideSpotRight = false;
            }
            else if (gameObject.transform.position.x <= collider.gameObject.transform.position.x)
            {
                onHideSpotLeft = false;
            }
        }
    }

    //return value if enemy blocked or not

    public void hitOrBlocked(bool hitSucces)
    {
        swordFollowThrough = hitSucces;
        
    }

    public void blockSuccesCheck()
    {
        blockBlip = false;
        
        blockDull = true;
        anim.SetBool("PlayerBlockDull", blockDull);
    }


}




