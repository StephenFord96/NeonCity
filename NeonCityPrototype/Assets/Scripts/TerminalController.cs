using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalController : MonoBehaviour
{
    private LevelGenerator nexus;
    private Animator anim;
    private SecurityCameraController secCameraCall;
    private TurretController secTurretCall;
    private PlayerController player;
    private UIConversationController terminalMenu;
    private LockPickGameController gameData;
    public GameObject menu;
    public GameObject lockPickGame;
    public GameObject wireSpliceGame;
    public GameObject securityCamera;
    public GameObject turret;
    public GameObject[] attachedSecurityItems;
    public bool playerHasControl;
    public bool isOpen;
    private bool playerInRange;
    public bool playerTalkingTo;
    public bool playerPicking;
    public float[] trueCombination = new float[3];
    private int securityID;
    private bool parse;
    private int parseID;
    //private int localSlideCount;

    private int camX;
    private int camY;
    private int securityTicker;

    private int terminalGridLocationX;
    private int terminalGridLocationY;
    // 1 is camera and 2 is turret


    public CameraController playerVision;
    public GameObject[] allSecurityItems;
    
    

    // Start is called before the first frame update
    void Start()
    {
        securityTicker = 0;
        parseID = 0;

        terminalGridLocationX = Mathf.CeilToInt((transform.position.x + 4.925f) / 9.85f);
        terminalGridLocationY = Mathf.CeilToInt((transform.position.y + 1.98f) / 3.96f);

        nexus = FindObjectOfType<LevelGenerator>();

        //localSlideCount = 3;

        anim = GetComponent<Animator>();
        anim.SetBool("Hacked", false);
        anim.SetBool("Locked", true);

        playerHasControl = false;
        isOpen = false;
        playerTalkingTo = false;

        //determines how many security elements will be attached to terminal
        attachedSecurityItems = new GameObject[2];
        allSecurityItems = new GameObject[nexus.maxTerminals * 2];


        /*
         
        //spawn and assignment loop for security devices
        for (int s = 0; s < attachedSecurityItems.Length; s++)
        {
            //determines if a camera/turret/etc will spawn
            securityID = Random.Range(1, 3);

            //determines X,Y coord of security feature
            camX = Random.Range(1, nexus.buildingWidth + 1);
            camY = Random.Range(terminalGridLocationY + 1, nexus.buildingHeight + 1);

            //camera section
            if (securityID == 1)
            {

                //instantiates a security camera
                attachedSecurityItems[s] = Instantiate(securityCamera, new Vector3(camX * 9.85f - 9.85f, camY * 3.96f - 2.4f, 0f), transform.rotation);

            }

            if (securityID == 2)
            {
                //instantiates a turret
                attachedSecurityItems[s] = Instantiate(turret, new Vector3(camX * 9.85f - 9.85f, camY * 3.96f - 2.4f, 0f), transform.rotation);

            }

        } */


        trueCombination[0] = Random.Range(1, 12);
        trueCombination[1] = Random.Range(1, 12);
        trueCombination[2] = Random.Range(1, 12);

        

    }

    // Update is called once per frame
    void Update()
    {

        if (playerVision = null)
        {
            playerVision = FindObjectOfType<CameraController>();
        }

        playerVision = FindObjectOfType<CameraController>();

        anim.SetBool("Locked", !isOpen);


        if (securityTicker < attachedSecurityItems.Length)
        {
            camX = Random.Range(1, nexus.buildingWidth+1);
            camY = Random.Range(1, nexus.buildingHeight + 1);

            if (nexus.secGrid[camX, camY] == 0)
            {
                nexus.secGrid[camX, camY] = 1;

                //determines if a camera/turret/etc will spawn
                securityID = Random.Range(1, 3);

                if (securityID == 1)
                {

                    //instantiates a security camera
                    GameObject s = Instantiate(securityCamera, new Vector3(camX * 9.85f - 9.85f, camY * 3.96f - 2.4f, 0f), transform.rotation);
                    nexus.assembleSecurity(s);
                    attachedSecurityItems[securityTicker] = s;
                }

                if (securityID == 2)
                {
                    //instantiates a turret
                    GameObject s = Instantiate(turret, new Vector3(camX * 9.85f - 9.85f, camY * 3.96f - 2.4f, 0f), transform.rotation);
                    nexus.assembleSecurity(s);
                    attachedSecurityItems[securityTicker] = s;
                }

                securityTicker = securityTicker + 1;
            }
            //else { Debug.Log("Check Check"); }
        }

        if (playerInRange == true && Input.GetKeyDown("e") && playerTalkingTo == false)
        {
            playerTalkingTo = true;
            player = FindObjectOfType<PlayerController>();
            player.playerInteracting = true;
            Instantiate(menu, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1f, gameObject.transform.position.z), gameObject.transform.rotation);
            terminalMenu = FindObjectOfType<UIConversationController>();

            if(playerHasControl == true)
            {
                terminalMenu.slideID = 3;
            }
            else if(isOpen == false && playerHasControl == false)
            {
                terminalMenu.slideID = 1;
            }else if (isOpen == true && playerHasControl == false)
            {
                terminalMenu.slideID = 2;
            }
            
        }

        if (playerTalkingTo == true)
        {

            //when fiddling and trying to break in
            if (playerHasControl == false)
            {
                //block to pick terminal lock from UI
                if (Input.GetKeyDown("1") && isOpen == false)
                {
                    terminalMenu.ExitConversation();
                    playerTalkingTo = false;
                    playerPicking = true;
                    player.playerInteracting = true;
                    Instantiate(lockPickGame, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), gameObject.transform.rotation);
                    gameData = FindObjectOfType<LockPickGameController>();
                    gameData.combination[0] = trueCombination[0];
                    gameData.combination[1] = trueCombination[1];
                    gameData.combination[2] = trueCombination[2];
                }

                //block to Hack terminal from UI
                if (Input.GetKeyDown("2"))
                {
                    //instantiate hacking minigame temporarilly insta wins
                    Invoke("Hack", 0f);
                    terminalMenu.ExitConversation();
                    playerTalkingTo = false;
                    player.playerInteracting = false;
                }

                //block to splice wires from terminal UI
                if (Input.GetKeyDown("1") && isOpen == true)
                {
                    //instantiate wire splice minigame temporarily insta wins
                    playerPicking = true;
                    Instantiate(wireSpliceGame, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), transform.rotation);
                    terminalMenu.ExitConversation();
                    playerTalkingTo = false;
                    player.playerInteracting = true;
                }

            }
            //block to actually interact with terminal and shut off cameras etc
            /*if(playerHasControl == true && localSlideCount == 3)
            {
               if(localSlideCount == 3 && Input.GetKeyDown("1"))
                {
                    
                    

                    if(attachedSecurityItems.Length == 1)
                    {
                        if(attachedSecurityItems[0].gameObject.tag == "Security Camera")
                        {
                            terminalMenu.slideID = 8f;
                            localSlideCount = 8;
                        }
                        else
                        {
                            terminalMenu.slideID = 7f;
                            localSlideCount = 7;
                        }
                    }
                    else
                    {
                        if(attachedSecurityItems[0].gameObject.tag == "Security Camera" && attachedSecurityItems[1].gameObject.tag == "Security Camera")
                        {
                            terminalMenu.slideID = 9f;
                            localSlideCount = 9;
                        }else if(attachedSecurityItems[0].gameObject.tag == "Turret" && attachedSecurityItems[1].gameObject.tag == "Security Camera")
                        {
                            terminalMenu.slideID = 5f;
                            localSlideCount = 5;
                        }
                        else if (attachedSecurityItems[0].gameObject.tag == "Turret" && attachedSecurityItems[1].gameObject.tag == "Turret")
                        {
                            terminalMenu.slideID = 6f;
                            localSlideCount = 6;
                        }
                        else if (attachedSecurityItems[0].gameObject.tag == "Security Camera" && attachedSecurityItems[1].gameObject.tag == "Turret")
                        {
                            terminalMenu.slideID = 4f;
                            localSlideCount = 4;
                        }
                    }


                    
                }

                

            }*/
            // block to disable cameras local slide count beyond 4 is all the slides displaying the list of possible security features
            if (playerHasControl == true)
            {
                if (Input.GetKeyDown("1") || parse == true)
                {
                    if(parse == false)
                    {
                        parse = true;
                        parseID = 0;
                        playerVision.camFocus(allSecurityItems[parseID]);
                    }

                    if (Input.GetKeyDown("escape"))
                    {
                        parse = false;
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        parseID = parseID + 1;
                        if (parseID == allSecurityItems.Length)
                        {
                            parseID = 0;
                        }

                        playerVision.camFocus(allSecurityItems[parseID]);
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        parseID = parseID - 1;
                        if (parseID < 0)
                        {
                            parseID = allSecurityItems.Length-1;
                        }

                        playerVision.camFocus(allSecurityItems[parseID]);
                    }
                    

                    if (Input.GetKeyDown("space") && nexus.playerHackPoints >= 1)
                    {
                        if(allSecurityItems[parseID].gameObject.tag == "Security Camera")
                        {
                            secCameraCall = allSecurityItems[parseID].gameObject.GetComponent<SecurityCameraController>();
                            secCameraCall.CaptureCamera();
                        }else if(allSecurityItems[parseID].gameObject.tag == "Turret")
                        {
                            secTurretCall = allSecurityItems[parseID].gameObject.GetComponent<TurretController>();
                            secTurretCall.CaptureCamera();
                        }

                        nexus.playerHackPoints = nexus.playerHackPoints - 1;
                    }
            
                }

                


            }

            //block to exit terminal UI
            if (Input.GetKeyDown("3"))
            {
                terminalMenu.ExitConversation();
                player.playerInteracting = false;
                playerTalkingTo = false;
                //localSlideCount = 3;
            }
        }

        if(playerPicking == true && Input.GetKeyDown("escape"))
        {
            player.playerInteracting = false;
            playerPicking = false;
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    public void Lock()
    {
        isOpen = true;
        anim.SetBool("Locked", false);
        playerPicking = false;
        player.playerInteracting = false;
    }

    //pretty sure this is dead code
    public void Splice()
    {
        playerHasControl = true;
        anim.SetBool("Hacked", true);
        playerPicking = false;
        player.playerInteracting = false;
    }

    public void Hack()
    {
        playerHasControl = true;
        anim.SetBool("Hacked", true);
        playerPicking = false;
        player.playerInteracting = false;
        nexus.playerHackPoints = nexus.playerHackPoints + 3;
    }

    

}
