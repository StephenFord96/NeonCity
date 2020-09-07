using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{

    private LevelGenerator callNexus;

    public int buildingWidth;
    public int buildingHeight;
    public int currentFloorRoomCount;
    public int currentHeightCount;
    private bool dataCheck;
    public bool identityRightLeft;
    public bool spawnBarrier;
    private int barrierChance;
    public int[] currentCoordinates = new int[2];
    public int[] barrierLocations;
    

    public GameObject RoomRL;
    public GameObject RoomR;
    public GameObject RoomL;
    public GameObject BlockRoomL;


    
    private Transform truePos;

    public Vector3 landingZone;

    public bool spawned;

    


    



    // Start is called before the first frame update
    void Start()
    {
        dataCheck = false;
        spawned = false;
        spawnBarrier = false;

        callNexus = FindObjectOfType<LevelGenerator>();

     
        //finds real position
        truePos = GetComponentInParent<Transform>();
        landingZone = truePos.position;


        //detects spawners orientation
        if (name == ("Right SpawnPoint"))
        {
            identityRightLeft = true;
        }
        else { identityRightLeft = false; }



        //calculates whether the spawner will instantiate a room that is malicious/blocked off from the main path
        barrierChance = Random.Range(1, 21);

        




    }

    // Update is called once per frame
    void Update()
    {

        currentCoordinates[0] = currentFloorRoomCount+1;
        currentCoordinates[1] = currentHeightCount;


        
        //barrier door/wall overlap fix
        if(dataCheck == true && spawned == false && barrierLocations[currentHeightCount] == currentFloorRoomCount && barrierLocations[currentHeightCount] != 0)
        {
            spawned = true;
            Instantiate(RoomR, truePos.position, truePos.rotation);
            gameObject.SetActive(false);
            callNexus.SpawnerInput();
            Destroy(gameObject);
        }

        // spawns barrier room
        if (dataCheck == true && spawned == false && spawnBarrier == true && (landingZone.x < ((buildingWidth * 9.85f) - 19.7) && landingZone.x > 0))
        {
            spawned = true;
            Instantiate(BlockRoomL, truePos.position, truePos.rotation);
            gameObject.SetActive(false);
            callNexus.SpawnerInput();
            callNexus.BarrierRegistry(currentCoordinates);
            //Debug.Log("Spawned A Barrier");
            Destroy(gameObject);

        }
        //spawns hallway room
            if (dataCheck == true && spawned == false && spawnBarrier == false && (landingZone.x < ((buildingWidth * 9.85f) - 19.7) && landingZone.x > 0))
        {
            spawned = true;                  
            Instantiate(RoomRL, truePos.position, truePos.rotation);
            gameObject.SetActive(false);
            callNexus.SpawnerInput();
            Destroy(gameObject);

        }
        else if (dataCheck == true && spawned == false && (landingZone.x < ((buildingWidth * 9.85f) - 9.85) && landingZone.x > 0))
        {
            //spawns end cap room
            spawned = true;
            Instantiate(RoomL, truePos.position, truePos.rotation);
            gameObject.SetActive(false);
            callNexus.SpawnerInput();
            Destroy(gameObject);
        }



            //if a spawner is out of bounds it detects it and destroys itself
            if (dataCheck == true && (landingZone.x > ((buildingWidth * 9.85f) - 9.85f)))
        {
            
            
            gameObject.SetActive(false);
            

        }





    }
    //LevelGenetator script constantly feeds this data into any spawner it can detect is active this also does...
    //...all the math and logic a spawner will need before it chooses what room to instantiate
    public void RecieveData(int[] incoming, int[] incomingBarrierLocation)
    {
        buildingWidth = incoming[0];
        buildingHeight = incoming[1];
        currentFloorRoomCount = incoming[2];
        currentHeightCount = incoming[3];


        barrierLocations = new int[buildingHeight];
        barrierLocations = incomingBarrierLocation;

        //this is supposed to promote barriers being spawned more to the right
        if (barrierChance > (20 - (currentFloorRoomCount * 3)))
        {
            spawnBarrier = true;
        }

        // cannot spawn barrier on the top floor
        if(buildingHeight == currentHeightCount)
        {
            spawnBarrier = false;
            //Debug.Log("prevented roof isolation");
        }

        //cannot spawn a barrier too close to edges of buildings or if there is a barrier already on current floor
        if (((buildingWidth - currentFloorRoomCount <= 2) || (barrierLocations[currentHeightCount] != 0)) && spawnBarrier == true)
        {
            spawnBarrier = false;
            //Debug.Log("Nullified Barrier Probability");
        }


        //cannot spawn barrier if there is a barrier bellow the current level/stack **TEMPORARY PATCH**
        if (currentHeightCount > 1 && barrierLocations[currentHeightCount - 1] != 0)
        {
            spawnBarrier = false;
        }

            //cannot spawn a barrier too far left of a barrier bellow it which would make a section unaccesible to the player

            if (currentHeightCount > 0)
        {
            if (barrierLocations[currentHeightCount - 1] >= currentFloorRoomCount)
            {
                spawnBarrier = false;
                //Debug.Log("Prevented Spaghettification");
            }
        }

        StartCoroutine(dataCheckConfirmation());
    }



    /// failed attempt at room collision to prevent overlapping generation likely because spawners are parents to rooms they don't collide
    /* void OnTriggerEnter2D(Collider2D other)     
    {
        if (other.gameObject.tag == "BuildingTemplate")
        {
            gameObject.SetActive(false);
            Debug.Log("Destroyed self");


            
        }
        Debug.Log("Collission occured");
    }*/

    //certain cases where datacheck was becoming true before incoming variables were updated fully caused generation failures so a delay is added to esnure proper data transfer
    IEnumerator dataCheckConfirmation()
    {
        yield return new WaitForSeconds(0.05f);
        dataCheck = true;
    }



}
