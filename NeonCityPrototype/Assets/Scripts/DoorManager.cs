using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{

    public int slotGridX;
    public int slotGridY;
    

    public GameObject RightDoor;
    public GameObject LockedDoor;
    public GameObject WallCap;
    private int roller;

    // Start is called before the first frame update
    void Start()
    {

        roller = 0;


        slotGridX = Mathf.CeilToInt((transform.position.x + 4.925f) / 9.85f);
        slotGridY = Mathf.CeilToInt((transform.position.y + 1.98f) / 3.96f);
    }

    // Update is called once per frame
    void Update()
    {
        



    }

    public void spawner(bool lockedDoor)
    {

        if (lockedDoor == true)
        {
            Instantiate(LockedDoor, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            Instantiate(WallCap, new Vector3(transform.position.x, transform.position.y+1.84f, transform.position.z), transform.rotation);
        }
        else
        {
            //rng for doorway door or open

            roller = Random.Range(0, 20);

            if(roller <= 10)
            {
                Instantiate(RightDoor, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                Instantiate(WallCap, new Vector3(transform.position.x, transform.position.y + 1.84f, transform.position.z), transform.rotation);
            }else if(roller == 11)
            {
                Instantiate(WallCap, new Vector3(transform.position.x, transform.position.y + 1.84f, transform.position.z), transform.rotation);
            }


        }

        Destroy(gameObject);

    }

    
}
