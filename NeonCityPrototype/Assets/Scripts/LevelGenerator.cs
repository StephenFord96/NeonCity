using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private LevelManager missionData;
    private EnemyController enemy;
    private RoomSpawner sendData;
    private DoorManager manageDoors;
    public GameObject computerTerminal;
    private GameObject[] existingDoorSlots;
    private HidingSpotManager manageHidingSlots;
    private TerminalSlotManager manageTerminalSlots;
    private TerminalController manageTerminalSecurity;
    public GameObject[] existingSpawners;
    public GameObject verticalCornerRoom;
    public GameObject stairsUp;
    public GameObject stairsDown;
    public GameObject enemyRed;
    public GameObject player;
    public GameObject mainCamera;
    public GameObject[] missionObjectives = new GameObject[6];
    public GameObject hidingSpotSlot;
    public int maxHidingSpots;
    public GameObject[] existingHidingSlots;
    private bool hideSlotsDone;

    private bool playerSpawned;
    public int objectiveLocationX;
    public int objectiveLocationY;
    private bool objectiveLocated;

    //public int lockedDoorX;
    //public int lockedDoorY;
    public int[,] lockedDoors;


    public int buildingWidth;
    public int buildingHeight;
    public int maxGuards;
    public int guardCount;
    public int[] levelData = new int[4];
    public int roomsBeenSpawned;
    public bool roomGenStatus;
    public int currentFloorRoomCount;
    public int currentHeightCount;
    public int[] barrierLocations;

    private int stairGenTicker;
    public int[] normalStairsUp;
    public int[] deadZoneStairs;
    public int deadStairsTicker;

    List<object> aliveGuards = new List<object>();

    public int playerHackPoints;
    public int maxTerminals;
    private GameObject[] existingTerminals;
    private GameObject[] allTerminals;
    public GameObject terminalSlot;
    public int[,] secGrid;
    public GameObject[] existingSecurityItems;
    private int existingSecTick;

    // Start is called before the first frame update
    void Start()
    {
        playerSpawned = false;


        buildingHeight = Random.Range(3, 10);
        buildingWidth = Random.Range(5, 8);
        maxGuards = Random.Range(buildingHeight + 1, buildingHeight + buildingWidth + 3)*3;
        maxHidingSpots = (buildingHeight * buildingWidth) - buildingHeight;
        maxTerminals = Random.Range(1 + (buildingHeight / 2), buildingHeight - 1);
        secGrid = new int[buildingWidth+1, buildingHeight+1];
        existingSecurityItems = new GameObject[maxTerminals * 2];
        existingSecTick = 0;


        lockedDoors = new int[Random.Range(1+buildingHeight,buildingHeight*2), 2];


        //Debug.Log(lockedDoors.Length/2);


        objectiveLocated = false;

        barrierLocations = new int[buildingHeight + 1];
        normalStairsUp = new int[buildingHeight+1];
        deadZoneStairs = new int[buildingHeight+1];

        roomsBeenSpawned = 1;
        roomGenStatus = false;

        stairGenTicker = 1;
                  
        currentFloorRoomCount = 1;
        currentHeightCount = 1;
       

        deadStairsTicker = 1;


        hideSlotsDone = false;

    }

    // Update is called once per frame
    void Update()
    {



        //initiates after a cake layer is done and starts the next one
        if ((currentHeightCount < buildingHeight) && currentFloorRoomCount == buildingWidth)
        {
            currentFloorRoomCount = 0;
            Instantiate(verticalCornerRoom, new Vector3 (transform.position.x, transform.position.y + (3.96f * currentHeightCount), transform.position.z), transform.rotation);
            currentHeightCount = currentHeightCount + 1;
            currentFloorRoomCount = currentFloorRoomCount + 1;
            roomsBeenSpawned = roomsBeenSpawned + 1;
        }


        //feedback for loading process
        if (roomGenStatus == false && roomsBeenSpawned > (buildingWidth * buildingHeight))
        {
            //Debug.Log("Generation Broke");
            roomGenStatus = false;
        } else if (roomsBeenSpawned == buildingWidth * buildingHeight && roomGenStatus == false)
        {
            //Debug.Log("Generation Complete!");
            roomGenStatus = true;
            
        }
        else if (roomGenStatus == false)
        {
            //Debug.Log("Generation In progress");
        }

        

        
        


        existingSpawners = GameObject.FindGameObjectsWithTag("HorizontalSpawner");

        foreach (GameObject e in existingSpawners)
        {
            
            sendData = e.gameObject.GetComponent<RoomSpawner>();
            sendData.RecieveData(levelData, barrierLocations);

            
        }

        levelData[0] = buildingWidth;
        levelData[1] = buildingHeight;
        levelData[2] = currentFloorRoomCount;
        levelData[3] = currentHeightCount;


        //stairway sequence also spawns an enemy wherever a staircase is and only starts when all rooms are finished generating
        while (roomGenStatus == true && stairGenTicker < buildingHeight)
        {
            if (barrierLocations[stairGenTicker] > 0)
            {
                normalStairsUp[stairGenTicker] = Random.Range(1, barrierLocations[stairGenTicker] +1);
                if (normalStairsUp[stairGenTicker] != normalStairsUp[stairGenTicker - 1] && (normalStairsUp[stairGenTicker] <= barrierLocations[stairGenTicker+1] || barrierLocations[stairGenTicker+1] == 0))
                {
                    Instantiate(stairsUp, new Vector3(normalStairsUp[stairGenTicker] * 9.85f - 9.85f, ((stairGenTicker - 1) * 3.96f) - (0.835f), transform.position.z), transform.rotation);
                    

                    Instantiate(enemyRed, new Vector3(normalStairsUp[stairGenTicker] * 9.85f - 9.85f, ((stairGenTicker - 1) * 3.96f) - (1.3f), transform.position.z), transform.rotation);
                    guardCount = guardCount + 1;
                    stairGenTicker = stairGenTicker + 1;

                }
            }
            else
            {
                normalStairsUp[stairGenTicker] = Random.Range(1, buildingWidth + 1);

                if (normalStairsUp[stairGenTicker] != normalStairsUp[stairGenTicker - 1] && (normalStairsUp[stairGenTicker] <= barrierLocations[stairGenTicker + 1] || barrierLocations[stairGenTicker + 1] == 0))
                {
                    Instantiate(stairsUp, new Vector3(normalStairsUp[stairGenTicker] * 9.85f - 9.85f, ((stairGenTicker - 1) * 3.96f) - (0.835f), transform.position.z), transform.rotation);
                    

                    Instantiate(enemyRed, new Vector3(normalStairsUp[stairGenTicker] * 9.85f - 9.85f, ((stairGenTicker - 1) * 3.96f) - (1.3f), transform.position.z), transform.rotation);
                    guardCount = guardCount + 1;
                    stairGenTicker = stairGenTicker + 1;
                }
            }
        }

        //deadzone stairway sequence only begins when normal stairs are complete

        if (roomGenStatus == true && stairGenTicker >= buildingHeight && deadStairsTicker <= buildingHeight - 1)
        {
            
            //Debug.Log(deadStairsTicker);

            // only generates stairs on a floor with barriers
            if (barrierLocations[deadStairsTicker] > 0)
            {
                //generates possible location for stairs
                deadZoneStairs[deadStairsTicker] = Random.Range(barrierLocations[deadStairsTicker] + 1, buildingWidth + 1);

                //will not generate stairs below normal stairs that would create overlap OR generate dead zone stairs ontop of others

                if (deadStairsTicker <= buildingHeight)
                {
                    if (deadZoneStairs[deadStairsTicker] != normalStairsUp[deadStairsTicker + 1] && deadZoneStairs[deadStairsTicker] != deadZoneStairs[deadStairsTicker - 1])
                    {
                        Instantiate(stairsUp, new Vector3(deadZoneStairs[deadStairsTicker] * 9.85f - 9.85f, ((deadStairsTicker - 1) * 3.96f) - (0.835f), transform.position.z), transform.rotation);
                        deadStairsTicker = deadStairsTicker + 1;
                    }
                    else
                    {
                        Debug.Log("zoinks");
                    }
                }
            }
            else
            {
                deadStairsTicker = deadStairsTicker + 1;
            }

            
        }
        

        //enemyspawnsequence only starts after normal and dead stairs gen is complete
        while (roomGenStatus == true && stairGenTicker == buildingHeight && deadStairsTicker >= buildingHeight && maxGuards > guardCount)
        {

            Instantiate(enemyRed, new Vector3(9.85f * Random.Range(2,buildingWidth), (3.96f * Random.Range(1,buildingHeight)) - 1.3f, transform.position.z), transform.rotation);

            guardCount = guardCount + 1;


            

        }



        //furniture slot spawn sequence

        //assigns class and object array to all the existing slots, is updating 
        existingHidingSlots = GameObject.FindGameObjectsWithTag("HidingSpotSlot");
        existingTerminals = GameObject.FindGameObjectsWithTag("TerminalSlot");
        

        //length of array of slots used as ticker for instantiation check
        if (existingHidingSlots.Length < maxHidingSpots && objectiveLocated == true && hideSlotsDone == false)
        {
            Instantiate(hidingSpotSlot, new Vector3((9.85f * Random.Range(1, buildingWidth)) + Random.Range(-3f, 3f), (3.96f * Random.Range(0, buildingHeight)) -1, transform.position.z), transform.rotation);

        }

        if (existingTerminals.Length < maxTerminals && objectiveLocated == true && hideSlotsDone == false)
        {
            Instantiate(terminalSlot, new Vector3((9.85f * Random.Range(1, buildingWidth)) + Random.Range(-3f, 3f), (3.96f * Random.Range(0, buildingHeight-1)) - 1.3f, transform.position.z), transform.rotation);

        }

        //block to determine next step of generation and check with delay
        if (existingHidingSlots.Length >= maxHidingSpots && hideSlotsDone != true && existingTerminals.Length >= maxTerminals)
        {

            hideSlotsDone = true;
            StartCoroutine(slotDelay());
        }

    }

    //assembles list of living guards and when the list reaches its max aka all guards are spawned, begin feeding them level data
    public void listMaker (object guardID)
    {
        aliveGuards.Add(guardID);

        //Debug.Log(aliveGuards.Count);

        if (aliveGuards.Count == maxGuards)
        {
            foreach (GameObject guard in aliveGuards)
            {
                enemy = guard.GetComponent<EnemyController>();
                enemy.recieveBuildingLayout(barrierLocations, normalStairsUp, deadZoneStairs);
            }


            //go to obj sequence
            Invoke("objectiveLocater", 0f);


        }

        
    }


    //METHOD TO GENERATE LEVEL GOAL

    //will look 3 floors down for the first availible barrier floor then assign it as the Y value then randomly assign any X value of that floor that does not have a staircase inside said room

    private void objectiveLocater()
    {
        //for loop only set to check top 3 floors
        for (int floorCheck = buildingHeight; floorCheck > buildingHeight-3; floorCheck--)
        {
            //if a barrier is present stay inside the loop and initilize X locator
            if (barrierLocations[floorCheck] != 0)
            {
                //assign the Y since you found a barrier
                objectiveLocationY = floorCheck;

                //if first time here or last value denied will start this loop and should not ever break because barrier zones have 2 rooms min and only 1 staircase so should always have at least 1 valid spot
                while (objectiveLocationX == 0)
                {
                    //generate a possible X value
                    objectiveLocationX = Random.Range(barrierLocations[floorCheck]+1, buildingWidth+1);

                    //if the generated value would be on a staircase then retry
                    if(deadZoneStairs[floorCheck] == objectiveLocationX)
                    {
                        objectiveLocationX = 0;
                    }
                }
                break;
            }
        }

        if(objectiveLocationX == 0 && objectiveLocationY == 0)
        {
            //no suitable location found go for 2 corners
            if (normalStairsUp[buildingHeight-1] != 1 && normalStairsUp[buildingHeight-1] != buildingWidth)
            {
                objectiveLocationY = buildingHeight;
                objectiveLocationX = (Random.Range(0, 2) * (buildingWidth-1)) + 1;
                Debug.Log("randomized corner");
                
            }else if(normalStairsUp[buildingHeight - 1] == 1)
            {
                objectiveLocationY = buildingHeight;
                objectiveLocationX = buildingWidth;
                Debug.Log("right corner");
            }else if(normalStairsUp[buildingHeight -1] == buildingWidth)
            {
                objectiveLocationY = buildingHeight;
                objectiveLocationX = 1;
                Debug.Log("left corner");
            }

            if (objectiveLocationX > normalStairsUp[objectiveLocationY-1])
            {
                lockedDoors[0,0] = objectiveLocationX - 1;
            }
            else
            {
                lockedDoors[0, 0] = objectiveLocationX;
            }
            lockedDoors[0, 1] = objectiveLocationY;
        }
        else
        {
            //roll for corners or barrier spot
            Debug.Log("keeping original aka barrier place");

            if(objectiveLocationX > deadZoneStairs[objectiveLocationY])
            {
                lockedDoors[0, 0] = objectiveLocationX - 1;
            }
            else
            {
                lockedDoors[0, 0] = objectiveLocationX;
            }

            lockedDoors[0, 1] = objectiveLocationY;
        }

        missionData = FindObjectOfType<LevelManager>();

        Instantiate(missionObjectives[missionData.missionGoal], new Vector3(objectiveLocationX * 9.85f -9.85f, objectiveLocationY * 3.96f -5.52f, 0f), transform.rotation);
        objectiveLocated = true;

        
       

        

    }



    //alarm system/notification
    public void guardsAlert()
    {
        foreach (GameObject guard in aliveGuards)
        {
            enemy = guard.GetComponent<EnemyController>();
            enemy.radioAlert();
        } 
    }

    public void SpawnerInput()
    {
        roomsBeenSpawned = roomsBeenSpawned + 1;
        currentFloorRoomCount = currentFloorRoomCount + 1;

    }

    //when  the room spawner choses to instantiate a barrier room it will notify this script to record it
    public void BarrierRegistry(int[] incomingData)
    {
        barrierLocations[incomingData[1]] = incomingData[0];
        
    }

    



    //delay to check if new slots came about otherwise enact the mass call to furniture spawn
    IEnumerator slotDelay()
    {

        yield return new WaitForSeconds(0.5f);

        if (existingHidingSlots.Length >= maxHidingSpots && existingTerminals.Length >= maxTerminals)
        {
            foreach (GameObject h in existingHidingSlots)
            {
                manageHidingSlots = h.gameObject.GetComponent<HidingSpotManager>();
                manageHidingSlots.spawnFurniture();

            }

            foreach(GameObject t in existingTerminals)
            {
                manageTerminalSlots = t.gameObject.GetComponent<TerminalSlotManager>();
                manageTerminalSlots.spawnTerminals();
            }
            Invoke("spawnDoors", 0f);
            Invoke("spawnPlayer", 0.5f);
        }
        else
        {
            hideSlotsDone = false;
        }
    }


    //this method calls to all the door slots and determines if they get a lock or not
    public void spawnDoors()
    {


        for (int t = 1; t < (lockedDoors.Length/2); t++)
        {

            //the do loop will check if the generated coordinates are already in use by a previous loop or are in the spot of a barrier
            //locked doors cannot spawn on the first floor
            do
            {

                lockedDoors[t, 0] = Random.Range(1, buildingWidth);
                lockedDoors[t, 1] = Random.Range(2, buildingHeight + 1);

                //checks for previous coord use overlap and p<t so it won't count itself as a dupe!
                for (int p = 1; p < t; p++)
                {

                    if(lockedDoors[p,0] == lockedDoors[t,0] && lockedDoors[p,1] == lockedDoors[t, 1])
                    {
                        lockedDoors[t, 0] = 0;
                        lockedDoors[t, 1] = 0;

                    }
                }

                //checks for barrier overlap
                if (lockedDoors[t, 0] !=0 && lockedDoors[t, 1] != 0)
                {
                    if (barrierLocations[lockedDoors[t, 1]] == lockedDoors[t, 0])
                    {
                        lockedDoors[t, 0] = 0;
                        lockedDoors[t, 1] = 0;

                    }
                }

            } while (lockedDoors[t, 0] == 0 && lockedDoors[t, 1] == 0);

        }


        existingDoorSlots = GameObject.FindGameObjectsWithTag("Door Slot");

        //every door slot grid location is compared to every generated locked door location
        foreach (GameObject m in existingDoorSlots)
        {
            manageDoors = m.gameObject.GetComponent<DoorManager>();


            for (int a = 0; a < lockedDoors.Length/2; a++)
            {
                if(manageDoors.slotGridX == lockedDoors[a, 0] && manageDoors.slotGridY == lockedDoors[a, 1])
                {
                    manageDoors.spawner(true);
                    break;
                } else if(a == (lockedDoors.Length / 2) - 1)
                {
                    manageDoors.spawner(false);
                }
            }


        }

    }

    public void assembleSecurity(GameObject nextItem)
    {
        Debug.Log("eating");
        existingSecurityItems[existingSecTick] = nextItem;
        existingSecTick += 1;

        if(existingSecTick == existingSecurityItems.Length)
        {
            Debug.Log("compiling");
            allTerminals = GameObject.FindGameObjectsWithTag("Terminal");
            foreach(GameObject t in allTerminals)
            {
                manageTerminalSecurity = t.gameObject.GetComponent<TerminalController>();
                for (int i = 0; i < existingSecurityItems.Length; i++)
                {
                    manageTerminalSecurity.allSecurityItems[i] = existingSecurityItems[i];
                }
            }
        }
    }

    public void spawnPlayer()
    {

        // level is done spawn player and camera
        if (playerSpawned == false)
        {
            playerSpawned = true;
            Instantiate(player, new Vector3(-4f, -1.3f, 0f), transform.rotation);
            Instantiate(mainCamera, new Vector3(-4f, 0f, 0f), transform.rotation);

        }
    }

}
