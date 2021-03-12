using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator enemyAnim;
    private Rigidbody2D enemyRigidBody;

    private TetherController callTether;
    public GameObject myTether;
    private LevelGenerator callNexus;
    private LevelManager callMiniNexus;
    public int enemyGridLocationX;
    public int enemyGridLocationY;

    public int pGridLocationX;
    public int pGridLocationY;

    public int destinationX;
    public int destinationY;

    public bool enemyInDeadZone;
    public bool destinationInDeadZone;

    public int[] barrierLayout;
    public int[] normalStairsLayout;
    public int[] deadZoneStairsLayout;
    public bool levelDataLoaded;

    public bool onPatrol;
    public bool isStatic;
    public bool playerSpotted;
    public bool isStrangled;
    public bool isDead;
    public float enemyMoveSpeed;
    public bool[] enemyBattleData = new bool[4];
    public bool dataLR;
    public bool dataHighLow;
    public bool dataDealingDamage;
    public bool dataBlocking;
    public bool dataAttacking;
    public bool enemyMoving;
    public float difficulty;
    public bool blockLocked;
    public bool enemyStunned;
    public float enemySightDistance;
    public GameObject spottedEffect;
    public bool playerInSight;
    public bool spottedEffectVerrified;

    //this helps split up frames and prevent attack loops
    public bool attackDone;
    public bool dataFollowUpAttack;
    public bool dataAggresiveHits;


    //tether and HQ data
    public int xHQ;
    public int yHQ;
    public int xTether;
    public int yTether;
    public int squadID;
    public bool guardStatic;




    private PlayerController battleDataEnemy;

    public Vector3 playerPos;
    public bool playerLR;
    public bool playerHL;
    public bool playerAttacking;
    public bool playerBlocking;
    public bool playerMoving;
    public float rangeX;
    public float rangeY;
    public float trueRangeX;
    public float trueRangeY;
    public float trueRange;
    public bool playerStunned;
    public bool playerBlockBlip;
    private bool playerHiddenLeft;
    private bool playerHiddenRight;

    public bool collisionToVisionDelay;


    // Start is called before the first frame update
    void Start()
    {
        callMiniNexus = FindObjectOfType<LevelManager>();

        callTether = myTether.GetComponent<TetherController>();

        collisionToVisionDelay = false;

        Physics2D.queriesStartInColliders = false;

        enemyAnim = GetComponent<Animator>();
        enemyRigidBody = GetComponent<Rigidbody2D>();
        dataLR = false;
        dataHighLow = true;
        dataBlocking = false;
        dataDealingDamage = false;
        playerSpotted = false;
        dataAttacking = false;
        enemyMoving = false;
        isStrangled = false;
        difficulty = Random.Range(8f, 10f);
        enemyMoveSpeed = 3f * (9 / 10);
        enemySightDistance = 5f;

        playerLR = true;
        playerHL = false;
        playerAttacking = false;
        playerBlocking = false;
        playerMoving = false;
        playerStunned = false;
        playerInSight = false;
        spottedEffectVerrified = false;
        isDead = false;

        isStatic = false;

        destinationX = xTether;
        destinationY = yTether;

        //adds enemy to the list of living guards upon startup
        callNexus = FindObjectOfType<LevelGenerator>();
        callNexus.listMaker(gameObject);

        
    }

    // Update is called once per frame
    void Update()
    {
        // calculates if player and AI's self is in or out of a dead zone of map

        if (levelDataLoaded == true)
        {

            if (enemyGridLocationX <= barrierLayout[enemyGridLocationY] || barrierLayout[enemyGridLocationY] == 0)
            {
                enemyInDeadZone = false;
            }
            else
            {
                enemyInDeadZone = true;
            }

            if (destinationX <= barrierLayout[destinationY] || barrierLayout[destinationY] == 0)
            {
                destinationInDeadZone = false;
            }
            else
            {
                destinationInDeadZone = true;
            }

        }

        // calculates distance from player
        rangeX = gameObject.transform.position.x - playerPos.x;
        rangeY = gameObject.transform.position.y - playerPos.y;
        trueRangeX = Mathf.Abs(rangeX);
        trueRangeY = Mathf.Abs(rangeY);
        trueRange = Mathf.Sqrt(trueRangeX * trueRangeX + trueRangeY * trueRangeY);

        
        //movement and limitations

        if (dataAttacking == true || isStrangled == true || dataBlocking == true)
        {
            enemyMoving = false;
        }

        if(enemyMoving == false)
        {
            enemyRigidBody.velocity = new Vector2(0f, 0f);
        }

        if (attackDone == true)
        {
            dataAttacking = false;
            dataFollowUpAttack = false;
        }

        
        //Raycasting and Detection via LoS

        RaycastHit2D highBeam = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), transform.right, enemySightDistance);

        RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), transform.right, enemySightDistance);

        if (highBeam.collider != null)
        {
            Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + 0.5f), highBeam.point, Color.blue);

            if (highBeam.collider.tag == "HideNode" && collisionToVisionDelay == false)
            {
                //Debug.Log("working");
                if(((playerHiddenRight == true && dataLR == false) || (playerHiddenLeft == true && dataLR == true)) && playerInSight == false)
                {
                    Debug.Log("found hidden player");
                    playerInSight = true;
                    Instantiate(spottedEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
                    playerSpotted = true;

                    destinationX = pGridLocationX;
                    destinationY = pGridLocationY;

                    StartCoroutine(radioForHelp());
                }
            }

        }


        if (hitInfo.collider != null)
        {
            Debug.DrawLine(new Vector2(transform.position.x, transform.position.y), hitInfo.point, Color.red);


            if (hitInfo.collider.CompareTag("Player") && ((playerHiddenRight == true && rangeX >= 0) || (playerHiddenLeft == true && rangeX <=0) || (playerHiddenRight==false && playerHiddenLeft==false)))
            {
                
                if (playerInSight == false && isStrangled == false)
                {
                    playerInSight = true;
                    Instantiate(spottedEffect, new Vector3 (transform.position.x, transform.position.y + 0.5f, transform.position.z) ,transform.rotation);
                    playerSpotted = true;

                    destinationX = pGridLocationX;
                    destinationY = pGridLocationY;

                    StartCoroutine(radioForHelp());
                }

            }
            

        }else
        {
            Debug.DrawLine(transform.position, transform.position + transform.right * enemySightDistance, Color.green);

        }



        //Engagement AI Hierarchy
        if ((playerSpotted == true || onPatrol == true) && isStrangled == false)
        {


            //chase portion and pathfinding

            //for when on patrol and on same floor as destination
            if(enemyGridLocationY == destinationY && enemyGridLocationX != destinationX && playerSpotted == false)
            {
                if(enemyGridLocationX >= destinationX)
                {
                    enemyMoving = true;
                    dataLR = false;
                    enemyRigidBody.velocity = new Vector2(-1f * enemyMoveSpeed, 0f);
                }
                else
                {
                    enemyMoving = true;
                    dataLR = true;
                    enemyRigidBody.velocity = new Vector2(1f * enemyMoveSpeed, 0f);
                }
            }

            // block of code when player is on the same level as the AI and there is not a barrier between them aka the final portion and gap closer aka 1st to last phase
            if (enemyGridLocationY == destinationY && destinationInDeadZone == enemyInDeadZone && playerSpotted == true && dataBlocking == false)
            {
                if (rangeX > 0.8f && dataAttacking == false)
                {
                    enemyMoving = true;
                    dataLR = false;
                    enemyRigidBody.velocity = new Vector2(-1f * enemyMoveSpeed, 0f);
                }
                else if (rangeX < -0.8f && dataAttacking == false)
                {
                    enemyMoving = true;
                    dataLR = true;
                    enemyRigidBody.velocity = new Vector2(1f * enemyMoveSpeed, 0f);
                }
                else
                {
                    enemyMoving = false;
                    enemyRigidBody.velocity = new Vector2(0f, 0f);
                }
            } 

            // this is the second to last phase when the AI is on the main line and so is the player, the AI just has to jump stairs until the final phase is true                             
            if (enemyInDeadZone == false && destinationInDeadZone == false && enemyGridLocationY != destinationY || (enemyInDeadZone == false && destinationInDeadZone == true && enemyGridLocationY != destinationY + 1 && enemyGridLocationY != destinationY))
            {
                // all instances for when enemy is above player checks to see where the up stairs are a floor down to find down stairs on current floor
                if(enemyGridLocationY > destinationY)
                {
                    //this section is simple it just gets the enemy to go to the room aka grid coord which has the target stairs
                    if (normalStairsLayout[enemyGridLocationY-1] > enemyGridLocationX)
                    {
                        enemyMoving = true;
                        dataLR = true;
                        enemyRigidBody.velocity = new Vector2(1f * enemyMoveSpeed, 0f);
                    } else if (normalStairsLayout[enemyGridLocationY-1] < enemyGridLocationX)
                    {
                        enemyMoving = true;
                        dataLR = false;
                        enemyRigidBody.velocity = new Vector2(-1f * enemyMoveSpeed, 0f);
                    } else if (normalStairsLayout[enemyGridLocationY-1] == enemyGridLocationX)
                    {
                        //this block is all the logic to guide the AI from within the correct room to the staircase and when it is allowed to use the stairs
                        if(((transform.position.x + 4.925) / 9.85) + 0.5f < normalStairsLayout[enemyGridLocationY - 1] )
                        {
                            enemyMoving = true;
                            dataLR = true;
                            enemyRigidBody.velocity = new Vector2(1f * enemyMoveSpeed, 0f);
                        } else if (((transform.position.x + 4.925) / 9.85) + 0.5f > normalStairsLayout[enemyGridLocationY - 1])
                        {
                            enemyMoving = true;
                            dataLR = false;
                            enemyRigidBody.velocity = new Vector2(-1f * enemyMoveSpeed, 0f);
                        }

                        if (((transform.position.x + 4.925) / 9.85) + 0.48f <= normalStairsLayout[enemyGridLocationY - 1] && ((transform.position.x + 4.925) / 9.85) + 0.52f >= normalStairsLayout[enemyGridLocationY - 1])
                        {
                            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 3.96f, transform.position.z);
                            //Debug.Log("I'm coming for you");
                        }
                    }
                }

                //all instances for when enemy is bellow player checks to find up stairs on current floor
                if(enemyGridLocationY < destinationY)
                {
                    if (normalStairsLayout[enemyGridLocationY] > enemyGridLocationX)
                    {
                        enemyMoving = true;
                        dataLR = true;
                        enemyRigidBody.velocity = new Vector2(1f * enemyMoveSpeed, 0f);
                    }
                    else if (normalStairsLayout[enemyGridLocationY] < enemyGridLocationX)
                    {
                        enemyMoving = true;
                        dataLR = false;
                        enemyRigidBody.velocity = new Vector2(-1f * enemyMoveSpeed, 0f);
                    }
                    else if (normalStairsLayout[enemyGridLocationY] == enemyGridLocationX)
                    {
                        if (((transform.position.x + 4.925) / 9.85) + 0.5f < normalStairsLayout[enemyGridLocationY])
                        {
                            enemyMoving = true;
                            dataLR = true;
                            enemyRigidBody.velocity = new Vector2(1f * enemyMoveSpeed, 0f);
                        }
                        else if (((transform.position.x + 4.925) / 9.85) + 0.5f > normalStairsLayout[enemyGridLocationY])
                        {
                            enemyMoving = true;
                            dataLR = false;
                            enemyRigidBody.velocity = new Vector2(-1f * enemyMoveSpeed, 0f);
                        }

                        if (((transform.position.x + 4.925) / 9.85) + 0.48f <= normalStairsLayout[enemyGridLocationY] && ((transform.position.x + 4.925) / 9.85) + 0.52f >= normalStairsLayout[enemyGridLocationY])
                        {
                            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 3.96f, transform.position.z);
                            //Debug.Log("I'm coming for you");
                        }
                    }
                }
            }

            // the second if statement actuates when the AI is in a deadzone he will seek the main line to fall into the 2nd to last phase or 3rd hunting phase
            if (enemyInDeadZone == true && destinationInDeadZone == false || (enemyInDeadZone == true && destinationInDeadZone == true && enemyGridLocationY != destinationY))
            {
                if (deadZoneStairsLayout[enemyGridLocationY] > enemyGridLocationX)
                {
                    enemyMoving = true;
                    dataLR = true;
                    enemyRigidBody.velocity = new Vector2(1f * enemyMoveSpeed, 0f);
                }
                else if (deadZoneStairsLayout[enemyGridLocationY] < enemyGridLocationX)
                {
                    enemyMoving = true;
                    dataLR = false;
                    enemyRigidBody.velocity = new Vector2(-1f * enemyMoveSpeed, 0f);
                }

                if (deadZoneStairsLayout[enemyGridLocationY] == enemyGridLocationX)
                {
                    if (((transform.position.x + 4.925) / 9.85) + 0.5f < deadZoneStairsLayout[enemyGridLocationY])
                    {
                        enemyMoving = true;
                        dataLR = true;
                        enemyRigidBody.velocity = new Vector2(1f * enemyMoveSpeed, 0f);
                    }
                    else if (((transform.position.x + 4.925) / 9.85) + 0.5f > deadZoneStairsLayout[enemyGridLocationY])
                    {
                        enemyMoving = true;
                        dataLR = false;
                        enemyRigidBody.velocity = new Vector2(-1f * enemyMoveSpeed, 0f);
                    }

                    if (((transform.position.x + 4.925) / 9.85) + 0.48f <= deadZoneStairsLayout[enemyGridLocationY] && ((transform.position.x + 4.925) / 9.85) + 0.52f >= deadZoneStairsLayout[enemyGridLocationY])
                    {
                        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 3.96f, transform.position.z);
                    }

                }
            }

            // the third if statement is when the AI is on the main line and seeks the staircase into the players deadzone area aka 3rd to last phase this can lead directly to the final phase if player stays in deadzone
            if (enemyInDeadZone == false && destinationInDeadZone == true && enemyGridLocationY == destinationY + 1)
            {
                if (deadZoneStairsLayout[enemyGridLocationY-1] > enemyGridLocationX)
                {
                    enemyMoving = true;
                    dataLR = true;
                    enemyRigidBody.velocity = new Vector2(1f * enemyMoveSpeed, 0f);
                }
                else if (deadZoneStairsLayout[enemyGridLocationY-1] < enemyGridLocationX)
                {
                    enemyMoving = true;
                    dataLR = false;
                    enemyRigidBody.velocity = new Vector2(-1f * enemyMoveSpeed, 0f);
                }

                if (deadZoneStairsLayout[enemyGridLocationY-1] == enemyGridLocationX)
                {
                    if (((transform.position.x + 4.925) / 9.85) + 0.5f < deadZoneStairsLayout[enemyGridLocationY-1])
                    {
                        enemyMoving = true;
                        dataLR = true;
                        enemyRigidBody.velocity = new Vector2(1f * enemyMoveSpeed, 0f);
                    }
                    else if (((transform.position.x + 4.925) / 9.85) + 0.5f > deadZoneStairsLayout[enemyGridLocationY-1])
                    {
                        enemyMoving = true;
                        dataLR = false;
                        enemyRigidBody.velocity = new Vector2(-1f * enemyMoveSpeed, 0f);
                    }

                    if (((transform.position.x + 4.925) / 9.85) + 0.48f <= deadZoneStairsLayout[enemyGridLocationY-1] && ((transform.position.x + 4.925) / 9.85) + 0.52f >= deadZoneStairsLayout[enemyGridLocationY-1])
                    {
                        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 3.96f, transform.position.z);
                    }

                }

            }

            //fixes bug when hunting player in deadzones that caused AI's from coming bellow the player to get stuck on that floor
            if (enemyInDeadZone == false && destinationInDeadZone == true && enemyGridLocationY == destinationY)
            {
                if (normalStairsLayout[enemyGridLocationY] > enemyGridLocationX)
                {
                    enemyMoving = true;
                    dataLR = true;
                    enemyRigidBody.velocity = new Vector2(1f * enemyMoveSpeed, 0f);
                }
                else if (normalStairsLayout[enemyGridLocationY] < enemyGridLocationX)
                {
                    enemyMoving = true;
                    dataLR = false;
                    enemyRigidBody.velocity = new Vector2(-1f * enemyMoveSpeed, 0f);
                }
                else if (normalStairsLayout[enemyGridLocationY] == enemyGridLocationX)
                {
                    if (((transform.position.x + 4.925) / 9.85) + 0.5f < normalStairsLayout[enemyGridLocationY])
                    {
                        enemyMoving = true;
                        dataLR = true;
                        enemyRigidBody.velocity = new Vector2(1f * enemyMoveSpeed, 0f);
                    }
                    else if (((transform.position.x + 4.925) / 9.85) + 0.5f > normalStairsLayout[enemyGridLocationY])
                    {
                        enemyMoving = true;
                        dataLR = false;
                        enemyRigidBody.velocity = new Vector2(-1f * enemyMoveSpeed, 0f);
                    }

                    if (((transform.position.x + 4.925) / 9.85) + 0.48f <= normalStairsLayout[enemyGridLocationY] && ((transform.position.x + 4.925) / 9.85) + 0.52f >= normalStairsLayout[enemyGridLocationY])
                    {
                        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 3.96f, transform.position.z);
                        //Debug.Log("I'm coming for you");
                    }

                }
            }


            //attacking portion
            if (trueRange < 1f && playerSpotted == true)
            {
                if (playerStunned == true && dataFollowUpAttack == false)
                {
                    StartCoroutine(followUpAttack());
                    //Debug.Log("StunFollowUpAttack");
                                                 
                }

               /* if (playerLR == dataLR && attackDone == false)
                {
                    if (dataAttacking == false)
                    {
                        dataHighLow = (Random.Range(0, 2) == 0);
                        Debug.Log("CheapShot");
                    }
                    
                }*/


                //this keeps the tempo up and makes the cheapshot attack method inferior as it logically includes it now
                if (((playerBlocking == true && dataAttacking == false)||(playerAttacking == false && playerBlocking == false) || (playerLR == dataLR)) && dataAttacking == false && attackDone == false && dataAggresiveHits == false)
                {
                    StartCoroutine(aggressiveHits());
                    //Debug.Log("Tempo hits");
                }


            }


            //blocking and aiming/looking portion
            //blocklocked is essentially cooldown the AI can drop block in else{} if they get an en gard and player is no longer attacking
            //ai will only use the engard to change block position if player's HL is different
            if (blockLocked == false && dataAttacking == false && playerAttacking == true && trueRange < 1 && dataLR != playerLR &&(dataBlocking == false || playerHL != dataHighLow))
            {
                blockLocked = true;
                dataBlocking = true;

                StartCoroutine(blockCooldown());
        
                                                        
            }else if(playerAttacking == false && blockLocked == false)
            {
                dataBlocking = false;
            }




            //commented out because caused guards to moonwalk when they would be alerted and navigate around the map but not actually be fighting the player
           /* if (gameObject.transform.position.x > playerPos.x)
            {
                dataLR = false;
            }
            else { dataLR = true; }
           */

            //halves move speed if patrolling instead of searching/alert
            if(onPatrol == true && playerSpotted == false && playerInSight == false)
            {
                enemyMoveSpeed = 3f * (difficulty / 20);
                gameObject.layer = 11;
            }
            if(onPatrol == false)
            {
                gameObject.layer = 9;
            }
            if(playerInSight == true || playerSpotted == true)
            {
                enemyMoveSpeed = 3f * (difficulty / 10);
                gameObject.layer = 9;
            }

        }


        //HQ and Tether Management

        if (playerInSight == false && playerSpotted == false)
        {

            //final calibration for a guard to place themself in a tether room
            if (enemyGridLocationX == xTether && enemyGridLocationY == yTether && destinationX == xTether && destinationY == yTether)
            {

                if (gameObject.transform.position.x > ((xTether - 1) * 9.85f) - 4.925f + ((9.85f * squadID) / 5) && guardStatic == false)
                {
                    enemyMoving = true;
                    dataLR = false;
                    enemyRigidBody.velocity = new Vector2(-1f * enemyMoveSpeed, 0f);
                }

                if (gameObject.transform.position.x < ((xTether - 1) * 9.85f) - 4.925f + ((9.85f * squadID) / 5) && guardStatic == false)
                {
                    enemyMoving = true;
                    dataLR = true;
                    enemyRigidBody.velocity = new Vector2(1f * enemyMoveSpeed, 0f);
                }

                if (Mathf.Abs(gameObject.transform.position.x - (((xTether - 1) * 9.85f) - 4.925f + ((9.85f * squadID) / 5))) <= 0.50f)
                {
                    if (guardStatic == false)
                    {
                        callTether.GuardInPosition(squadID, 0);
                    }
                    guardStatic = true;
                    enemyMoving = false;
                    enemyRigidBody.velocity = new Vector2(0f, 0f);
                    onPatrol = false;
                }
                else { guardStatic = false; onPatrol = true; }
            }

            //final calibration for guards to place themselves inside an HQ room
            if (enemyGridLocationX == xHQ && enemyGridLocationY == yHQ && destinationX == xHQ && destinationY == yHQ)
            {

                if (gameObject.transform.position.x > ((xHQ - 1) * 9.85f) - 4.925f + ((9.85f * squadID) / 5) && guardStatic == false)
                {
                    enemyMoving = true;
                    dataLR = false;
                    enemyRigidBody.velocity = new Vector2(-1f * enemyMoveSpeed, 0f);
                }

                if (gameObject.transform.position.x < ((xHQ - 1) * 9.85f) - 4.925f + ((9.85f * squadID) / 5) && guardStatic == false)
                {
                    enemyMoving = true;
                    dataLR = true;
                    enemyRigidBody.velocity = new Vector2(1f * enemyMoveSpeed, 0f);
                }

                if (Mathf.Abs(gameObject.transform.position.x - (((xHQ - 1) * 9.85f) - 4.925f + ((9.85f * squadID) / 5))) <= 0.50f)
                {
                    if (guardStatic == false)
                    {
                        callTether.GuardInPosition(squadID, 1);
                    }
                    guardStatic = true;
                    enemyMoving = false;
                    enemyRigidBody.velocity = new Vector2(0f, 0f);
                    onPatrol = false;
                }
                else { guardStatic = false; onPatrol = true; }
            }

        }
        //patrolling AI

        //**OUTDATED**

        /*if (playerSpotted == false && isStrangled == false && dataBlocking == false)
        {
            enemyMoving = true;
            
            
            if (enemyRigidBody.velocity == new Vector2(0f,0f) && dataLR == false)
            {
                enemyRigidBody.velocity = new Vector2(1f * enemyMoveSpeed/2, 0f);
                //Debug.Log("Patrolling Right");
                dataLR = true;
            } 

            if (enemyRigidBody.velocity == new Vector2(0f, 0f) && dataLR == true)
            {
                enemyRigidBody.velocity = new Vector2(-1f * enemyMoveSpeed/2, 0f);
                //Debug.Log("Patrolling Left");
                dataLR = false;
            }


            // an attempt to fix busted AI walking really slowly after bumping into crowds
            if (dataLR == true && enemyRigidBody.velocity.x < 1f * enemyMoveSpeed / 2)
            {
                enemyRigidBody.velocity = new Vector2(1f * enemyMoveSpeed / 2, 0f);
            }

            if (dataLR == false && enemyRigidBody.velocity.x > -1f * enemyMoveSpeed / 2)
            {
                enemyRigidBody.velocity = new Vector2(-1f * enemyMoveSpeed / 2, 0f);
            }

        }*/



        //animations

        enemyAnim.SetBool("EnemyBlocking", dataBlocking);
        enemyAnim.SetBool("EnemyAttacking", dataAttacking);
        enemyAnim.SetBool("EnemyMoving", enemyMoving);
        enemyAnim.SetBool("BeenBlocked", enemyStunned);

        if (isStrangled == true)
        {
            enemyAnim.SetBool("Strangled", true);
        }

        if (isDead == true)
        {
            gameObject.SetActive(false);
        }

        if (dataHighLow == true)
        {
            enemyAnim.SetFloat("EnemyHL", 1);
        }

        if (dataHighLow == false)
        {
            enemyAnim.SetFloat("EnemyHL", -1);
        }

        if (dataLR == true)
        {
            enemyAnim.SetFloat("FaceLeft", 0);
            enemyAnim.SetFloat("FaceRight", 1);
            enemyAnim.SetFloat("EnemyLeftRight", 1);
        }

        if (dataLR == false)
        {
            enemyAnim.SetFloat("FaceLeft", 1);
            enemyAnim.SetFloat("FaceRight", 0);
            enemyAnim.SetFloat("EnemyLeftRight", -1);
        }



        //math to determine which room on the coordinate scale the enemy is in
        enemyGridLocationX = Mathf.CeilToInt((transform.position.x + 4.925f) / 9.85f);
        enemyGridLocationY = Mathf.CeilToInt((transform.position.y + 1.98f) / 3.96f);

        //fixes bug where 2 guards collide and ,for a single frame, hide node ray is still firing in wrong direction but dataLR is updated so it detects player
        collisionToVisionDelay = false;

    }


    //this is called when the player's sword collides with an enemy
    public void dataCheckFromPlayer(bool[] incomingData)
    {
        if (incomingData[3] == true && incomingData[1] != dataLR && incomingData[2] == dataHighLow && dataBlocking == true)
        {
            //Debug.Log("Chink! He blocks");
            battleDataEnemy = FindObjectOfType<PlayerController>();
            battleDataEnemy.hitOrBlocked(false);
        }
        if ((incomingData[3] == true) && (incomingData[2] != dataHighLow || incomingData[1] == dataLR || dataBlocking == false))
        {
            //Debug.Log("Slice! You hit him");
            battleDataEnemy = FindObjectOfType<PlayerController>();
            battleDataEnemy.hitOrBlocked(true);
            isDead = true;
            gameObject.SetActive(false);
        }


        /*
        Debug.Log("player is blocking:");
        Debug.Log(incomingData[0]);
        Debug.Log("Player is Looking Right");
        Debug.Log(incomingData[1]);
        Debug.Log("Player Is Aiming Up");
        Debug.Log(incomingData[2]);
        Debug.Log("Player Is Colliding");
        Debug.Log(incomingData[3]);
        */

    }



    //    !! HIT DETECTION !!    this is called when the enemys sword hits the player or when bodies collide



    //fixes all AI collision issues and behaviour with each other
    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.tag == "Enemy" && playerSpotted == false)
        {
            // prevents people from riding on each other if they are faster than the guy infront of them
            // prevents head ramming by verrifying the other body is actually approaching it and they are not back to back going opposite directions and stuck inside each others colliders

            if (other.gameObject.tag == "Enemy")
            {
                collisionToVisionDelay = true;

                if (dataLR == false && other.gameObject.transform.position.x < gameObject.transform.position.x)
                {
                    enemyRigidBody.velocity = new Vector2(1f * enemyMoveSpeed / 2, 0f);
                    // Debug.Log("Pardon me, going Right");
                    enemyAnim.SetFloat("FaceLeft", 0);
                    enemyAnim.SetFloat("FaceRight", 1);
                    enemyAnim.SetFloat("EnemyLeftRight", 1);
                    dataLR = true;
                }
                if (dataLR == true && other.gameObject.transform.position.x > gameObject.transform.position.x)
                {
                    enemyRigidBody.velocity = new Vector2(-1f * enemyMoveSpeed / 2, 0f);
                    // Debug.Log("Pardon me, going Left");
                    enemyAnim.SetFloat("FaceLeft", 1);
                    enemyAnim.SetFloat("FaceRight", 0);
                    enemyAnim.SetFloat("EnemyLeftRight", -1);
                    dataLR = false;

                }
            }

        }

        if (other.gameObject.tag == "Player" && dataAttacking == true && dataDealingDamage == true)
        {
            //Debug.Log("The Enemy Hits You!");

            if (playerHL == dataHighLow && playerBlocking == true && playerLR != dataLR)
            {
                enemyStunned = true;
                dataFollowUpAttack = false;

                //Debug.Log("Clink you block the enemy");

                battleDataEnemy = FindObjectOfType<PlayerController>();
                battleDataEnemy.blockSuccesCheck();

            }
            else
            {
                //Debug.Log("Ouch! The Enemy Hit you!");
                Destroy(FindObjectOfType<HideNodeManager>(), 0f);
                //other.gameObject.SetActive(false);
                //callMiniNexus.playerLiving = false;
                Debug.Log("YOU DIED");

            }

        }


    }


    public void playerPosCheck(Vector3 posUpdate, int xCoord, int yCoord)
    {
        playerPos = posUpdate;
        pGridLocationX = xCoord;
        pGridLocationY = yCoord;

        if(playerSpotted == true)
        {
            destinationX = pGridLocationX;
            destinationY = pGridLocationY;
        }

    }

    public void playerStanceCheck(bool[] stance)
    {
        playerLR = stance[0];
        playerHL = stance[1];
        playerAttacking = stance[2];
        playerBlocking = stance[3];
        playerMoving = stance[4];
        playerStunned = !stance[5];
        playerBlockBlip = stance[6];
        playerHiddenLeft = stance[7];
        playerHiddenRight = stance[8];



        
    }


    public void patrolHQ()
    {
        destinationX = xHQ;
        destinationY = yHQ;
        onPatrol = true;
    }

    public void patrolTether()
    {
        destinationX = xTether;
        destinationY = yTether;
        onPatrol = true;
    }




    //limits when enemy can change block direction and controls block aim

    IEnumerator blockCooldown()
    {
        
        dataHighLow = playerHL;

        if (Random.Range(0f, 18f) >= difficulty)
        {
            blockLocked = false;
            //Debug.Log("En Garde");
            yield break;
        }else
        {
            //Debug.Log("Too Slow!");

            yield return new WaitForSeconds(2f - (difficulty / 10f));
            blockLocked = false;
        }

    }


    IEnumerator followUpAttack()
    {
        //Debug.Log("test 1");
        dataFollowUpAttack = true;
        yield return new WaitForSeconds(0.15f);
        dataHighLow = (Random.Range(0, 2) == 0);
        dataAttacking = true;
        //Debug.Log("test 2");
        blockLocked = false;
        dataBlocking = false;
        
    }

    IEnumerator aggressiveHits()
    {
        //Debug.Log("TimerStarted");
        dataAggresiveHits = true;
        if (enemyStunned == false)
        {
            yield return new WaitForSeconds((11f - difficulty) / 9f);
        }
        if (dataAttacking == false && enemyStunned == false && dataBlocking == false && playerAttacking == false)
        {
            dataHighLow = (Random.Range(0, 2) == 0);
            dataAttacking = true;
            //Debug.Log("Time Up");
            //Debug.Log("DidATempoHit");
        }

        dataAggresiveHits = false;

    }

    // after 5 seconds initializes list in level manager script to tell all living guards to be on alert
    IEnumerator radioForHelp()
    {
        yield return new WaitForSeconds(8f);
        callNexus.guardsAlert();
    }

    //is called from level manager script after radio has been used
    public void radioAlert()
    {
        if (playerSpotted == false)
        {
            Instantiate(spottedEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
            playerSpotted = true;

            destinationX = pGridLocationX;
            destinationY = pGridLocationY;

        }
    }


    //pretty sure this is dead code
    public void spotVerify()
    {
        spottedEffectVerrified = true;
    }

    public void recieveBuildingLayout(int[]barriers, int[]normalStairs, int[]deadStairs)
    {
        barrierLayout = barriers;
        normalStairsLayout = normalStairs;
        deadZoneStairsLayout = deadStairs;

        levelDataLoaded = true;

        //Debug.Log("recieved");
    }


}
