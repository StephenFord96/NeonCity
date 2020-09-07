using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPickGameController : MonoBehaviour
{
    public GameObject click;
    public GameObject thump;
    private GameObject[] possibleDoors;
    private DoorController callDoor;
    private GameObject[] possibleTerminals;
    private TerminalController callTerminal;
    public float pickLength;
    public float pickAngle;
    public int pick_Length;
    public int pick_Angle;
    public int pick_LengthPunched;
    public int pick_AnglePunched;
    private Animator lockPickAnim;
    public float[] combination = new float[3];
    public float[] playerCombo = new float[3];


    // Start is called before the first frame update
    void Start()
    {


        pick_Length = 3;
        pick_Angle = 6;



        lockPickAnim = GetComponent<Animator>();

        pickAngle = 6;
        pickLength = 3;


        possibleDoors = GameObject.FindGameObjectsWithTag("Locked Door");
        

        foreach(GameObject d in possibleDoors)
        {
            callDoor = d.gameObject.GetComponent<DoorController>();

            if(callDoor.playerPicking == true)
            {
                break;
            }
            else
            {
                callDoor = null;
            }
            
        }

        possibleTerminals = GameObject.FindGameObjectsWithTag("Terminal");

        foreach (GameObject t in possibleTerminals)
        {
            callTerminal = t.gameObject.GetComponent<TerminalController>();

            if (callTerminal.playerPicking == true)
            {
                break;
            }
            else
            {
                callTerminal = null;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("escape")){

            Destroy(gameObject, 0f);
        }


        if (Input.GetKeyDown("a") && pickLength < 3)
        {
            pickLength = pickLength + 1;
            pick_Length = pick_Length + 1;

        }

        if (Input.GetKeyDown("d") && pickLength > 1)
        {
            pickLength = pickLength - 1;
            pick_Length = pick_Length - 1;

        }

        if(Input.GetKeyDown("w") && pickAngle > 1)
        {
            pickAngle = pickAngle - 1;
            pick_Angle = pick_Angle - 1;
        }

        if (Input.GetKeyDown("s") && pickAngle < 11)
        {
            pickAngle = pickAngle + 1;
            pick_Angle = pick_Angle + 1;
        }

        lockPickAnim.SetFloat("Angle", pickAngle);
        lockPickAnim.SetFloat("Pick Length", pickLength);

        if (Input.GetMouseButtonDown(1))
        {
            pick_LengthPunched = pick_Length;
            pick_AnglePunched = pick_Angle;

            playerCombo[pick_LengthPunched - 1] = pick_AnglePunched;

            if(playerCombo[pick_LengthPunched -1] == combination[pick_LengthPunched - 1])
            {
                Instantiate(click, new Vector3(gameObject.transform.position.x + Random.Range(-1, 2), gameObject.transform.position.y + Random.Range(-1, 2), gameObject.transform.position.z), transform.rotation);
            }
            else
            {
                Instantiate(thump, new Vector3(gameObject.transform.position.x + Random.Range(-1, 2), gameObject.transform.position.y + Random.Range(-1, 2), gameObject.transform.position.z), transform.rotation);
            }
        }

        if(Input.GetMouseButtonDown(0) && playerCombo[0] == combination[0] && playerCombo[1] == combination[1] && playerCombo[2] == combination[2])
        {
            if (callDoor != null)
            {
                callDoor.doorUnlocked();
            }else if(callTerminal != null)
            {
                callTerminal.Lock();
                Debug.Log("Unlocked Terminal");
            }

            //Debug.Log("Unlocked!");
            Destroy(gameObject, 0.5f);
        }


    }


}
